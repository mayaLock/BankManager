/*
    Dipayan Sarker
    February 10, 2020
*/

using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace BankManager.Converters
{
    /// <summary>
    /// A class to convert string to ComboxSelectedItem
    /// </summary>
    public class StringToComBoxSelectedItemConverter : IValueConverter
    {
        /// <summary>
        /// Converts string to ComBoxSlectedItem
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new ComboBoxItem { Content = (string)value };
        }

        /// <summary>
        /// Converts ComboBoxSelectedItem to string
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string ret = string.Empty;
            if (value != null)
            {
                ret = ((ComboBoxItem)value).Content.ToString();
            }
            return ret;
        }
    }
}
