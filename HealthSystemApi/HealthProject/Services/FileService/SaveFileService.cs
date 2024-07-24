using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

#if ANDROID
using Android.Content;
using Android.App;
using AndroidX.Core.Content; 
#endif

#if IOS
using Foundation;
using UIKit;
#endif

namespace HealthProject.Services.FileService
{
    public class SaveFileService
    {
        public async Task<string> SaveFileAsync(byte[] fileBytes, string fileName)
        {
            string localPath = FileSystem.Current.AppDataDirectory;
            string filePath = Path.Combine(localPath, fileName);
            await File.WriteAllBytesAsync(filePath, fileBytes);
            return filePath;
        }

        public void OpenFile(string filePath)
        {
#if ANDROID
            var context = Android.App.Application.Context;
            var file = new Java.IO.File(filePath);
            var uri = AndroidX.Core.Content.FileProvider.GetUriForFile(context, $"{context.PackageName}.fileprovider", file);

            var intent = new Intent(Intent.ActionView);
            intent.SetDataAndType(uri, GetMimeType(filePath));
            intent.AddFlags(ActivityFlags.GrantReadUriPermission);

            context.StartActivity(intent);
#elif IOS
            var viewController = UIApplication.SharedApplication.KeyWindow.RootViewController;
            var documentInteractionController = new UIDocumentInteractionController
            {
                Url = NSUrl.FromFilename(filePath)
            };
            documentInteractionController.Uti = GetMimeType(filePath); // Setting UTI for iOS
            documentInteractionController.PresentOpenInMenu(viewController.View.Frame, viewController.View, true);
#endif
        }

#if ANDROID
        private string GetMimeType(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            return extension switch
            {
                ".pdf" => "application/pdf",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".png" => "image/*",
                ".jpg" => "image/*",
                ".jpeg" => "image/*",
                ".bmp" => "image/*",
                _ => "application/octet-stream",
            };
        }
#elif IOS
        private string GetMimeType(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            return extension switch
            {
                ".pdf" => "com.adobe.pdf",
                ".doc" => "com.microsoft.word.doc",
                ".docx" => "com.microsoft.word.docx",
                ".png" => "public.image",
                ".jpg" => "public.image",
                ".jpeg" => "public.image",
                ".bmp" => "public.image",
                _ => "public.data",
            };
        }
#endif
    }
}
