using System;
using System.Globalization;
using System.Windows.Data;
using TasksManagerClient.Model;

namespace TasksManagerClient.Helpers.ValueConverters
{
    class WorkTaskPriorityToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            WorkTaskPriority p = (WorkTaskPriority)value;
            return (p == WorkTaskPriority.High) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
