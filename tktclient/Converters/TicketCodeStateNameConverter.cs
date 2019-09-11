using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using tktclient.Infrastructure;

namespace tktclient.Converters
{
    public class TicketCodeStateNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((TicketCodeStates)Enum.Parse(typeof(TicketCodeStates), value.ToString()))
            {
                case TicketCodeStates.未使用:
                    return "未使用";
                case TicketCodeStates.已使用:
                    return "已使用";
                case TicketCodeStates.已过期:
                    return "已过期";
                default:
                    return "未知";
            }
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TicketCodeStateColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((TicketCodeStates)Enum.Parse(typeof(TicketCodeStates), value.ToString()))
            {
                case TicketCodeStates.未使用:
                    return new SolidColorBrush(Colors.Green);
                case TicketCodeStates.已使用:
                    return new SolidColorBrush(Colors.Red);
                case TicketCodeStates.已过期:
                    return new SolidColorBrush(Colors.Red);
                default:
                    return new SolidColorBrush(Colors.DimGray);
            }
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
