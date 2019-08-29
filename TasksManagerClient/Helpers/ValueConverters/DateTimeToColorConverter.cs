using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using TasksManagerClient.Model;

namespace TasksManagerClient.Helpers.ValueConverters
{
    class DateTimeToColorConverter : IMultiValueConverter
    {
        public SolidColorBrush ExpiriedBrush { get; set; }
        public SolidColorBrush ComingBrush { get; set; }
        public SolidColorBrush NotRushBrush { get; set; }
        public SolidColorBrush DefaultBrush { get; set; }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            WorkTaskStates state = (WorkTaskStates)values[1];
            if (state != WorkTaskStates.Work)
                return DefaultBrush;
            DateTime date = (DateTime)values[0];
            Ugrencys u = Utilits.DateTimeToUgrency(date);
            switch (u)
            {
                case Ugrencys.Expiried:
                    return ExpiriedBrush;
                case Ugrencys.Coming:
                    return ComingBrush;
            }
            return NotRushBrush;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
