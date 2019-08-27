using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace TasksManagerClient.Helpers.ValueConverters
{
    class DateTimeToColorConverter : IValueConverter
    {
        public SolidColorBrush ExpiriedBrush { get; set; }
        public SolidColorBrush ComingBrush { get; set; }
        public SolidColorBrush NotRushBrush { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date = (DateTime)value;
            Ugrencys u = Utilits.DateTimeToUgrency(date);
            switch(u)
            {
                case Ugrencys.Expiried:
                    return ExpiriedBrush;
                case Ugrencys.Coming:
                    return ComingBrush;
            }
            return NotRushBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
