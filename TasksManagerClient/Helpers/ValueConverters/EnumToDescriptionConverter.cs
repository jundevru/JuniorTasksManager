using System;
using System.Globalization;
using System.Windows.Data;

namespace TasksManagerClient.Helpers.ValueConverters
{
    class EnumToDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is Enum))
                return "Параметр не Enum";
            return EnumHelper.GetDescription(value as Enum);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
