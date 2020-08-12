/*
    Dipayan Sarker
    February 10, 2020
*/

using System;
using System.Globalization;
using System.Windows.Data;

namespace BankManager.Converters
{
    /// <summary>
    /// A class to convert Int32 to string
    /// </summary>
    public class Int32ToStringConverter : IValueConverter
    {
        /// <summary>
        /// Converts int32 value to string
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return ((int)value).ToString("D8");
            }            
            return string.Empty;            
        }

        /// <summary>
        /// Converts string value to int32
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int ret = 0;
            if (int.TryParse((string)value, out ret))
            {
                return ret;
            }
            return ret;
        }
    }
}
