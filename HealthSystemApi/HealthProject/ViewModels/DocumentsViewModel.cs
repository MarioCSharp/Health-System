using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Models;
using HealthProject.Services.AuthenticationService;
using HealthProject.Services.DocumentService;
using HealthProject.Views;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace HealthProject.ViewModels
{
    public partial class DocumetnsViewModel : ObservableObject
    {
        private IDocumentService documentService;
        private IAuthenticationService authenticationService;

        [ObservableProperty]
        private ObservableCollection<DocumentViewModel> documents;

        public ICommand AddDocumentCommand { get; }
        public ICommand DeleteDocumentCommand { get; }
        public ICommand NavigateToDocumentDetailsCommand { get; }

        public DocumetnsViewModel(IDocumentService documentService,
                                  IAuthenticationService authenticationService)
        {
            this.documentService = documentService;
            this.authenticationService = authenticationService;

            AddDocumentCommand = new AsyncRelayCommand(NavigateToAddPage);
            DeleteDocumentCommand = new AsyncRelayCommand<object>(DeleteAsync);
            NavigateToDocumentDetailsCommand = new AsyncRelayCommand<object>(DetailsAsync);

            LoadUserDocuments();
        }

        private async Task NavigateToAddPage() 
        {
            await Shell.Current.GoToAsync($"//{nameof(DocumentAddPage)}");
        }

        private async Task DeleteAsync(object parameter)
        {
            if (parameter is int id)
            {
                await documentService.RemoveAsync(id);
                await Shell.Current.GoToAsync($"//{nameof(DocumentViewPage)}");
            }
        }

        private async Task DetailsAsync(object parameter)
        {
            if (parameter is int id)
            {
                var doc = await documentService.DetailsAsync(id);

                var docJson = JsonConvert.SerializeObject(doc);
                var encodedDocJson = Uri.EscapeDataString(docJson);

                await Shell.Current.GoToAsync($"DocumentDetailsPage?documentJson={encodedDocJson}");
            }
        }

        public async void LoadUserDocuments()
        {
            var auth = await authenticationService.IsAuthenticated();

            if (!auth.IsAuthenticated)
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }

            var docs = await documentService.GetUserDocuments(auth.UserId);

            Documents = new ObservableCollection<DocumentViewModel>(docs);
        }
    }
}
