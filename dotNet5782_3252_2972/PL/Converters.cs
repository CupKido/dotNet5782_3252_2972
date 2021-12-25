using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows;
using System.Windows.Media;

namespace PL
{
    internal class BoolVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value == true)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class AllowDeleteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)value != -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class ColorByPercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((Double)value > 70)
            {
                return Brushes.Green;
            }
            else if ((Double)value > 50)
            {
                return Brushes.GreenYellow;
            }
            else if ((Double)value > 20)
            {
                return Brushes.Yellow;
            }
            else if ((Double)value > 10)
            {
                return Brushes.Red;
            }
            else
            {
                return Brushes.DarkRed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }

    internal class IntByEnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}