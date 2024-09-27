using System.Globalization;

namespace HealthProject.Converters
{
    public class SelectedItemToTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var currentItem = value as string;
            var selectedItem = parameter as string;

            if (currentItem == selectedItem)
            {
                return Color.FromArgb("#007ACC"); 
            }

            return Color.FromArgb("#000"); 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
