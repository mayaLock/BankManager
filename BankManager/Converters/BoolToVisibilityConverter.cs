/*
    Dipayan Sarker
    February 10, 2020
*/

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BankManager.Converters
{
    /// <summary>
    /// Converts bool to Visibility and vice-versa
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts bool value to Visibility
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility visibility = Visibility.Hidden;
            if (value != null)
            {
                visibility = (bool)value is true ? Visibility.Visible : visibility;
            }
            return visibility;
        }

        /// <summary>
        /// Converts Visiility to boolean
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Visibility)value == Visibility.Visible ? true : false;
        }
    }
}
