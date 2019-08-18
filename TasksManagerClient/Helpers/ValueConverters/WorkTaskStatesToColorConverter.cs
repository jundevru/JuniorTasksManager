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
    class WorkTaskStatesToColorConverter : IValueConverter
    {
        public SolidColorBrush CancelBrush { get; set; }
        public SolidColorBrush CompletteBrush { get; set; }
        public SolidColorBrush WorkBrush { get; set; }
        public SolidColorBrush ErrorBrush { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            WorkTaskStates state = (WorkTaskStates)value;
            switch(state)
            {
                case WorkTaskStates.Cancel:
                    return CancelBrush;
                case WorkTaskStates.Complette:
                    return CompletteBrush;
                case WorkTaskStates.Work:
                    return WorkBrush;
            }
            return ErrorBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
