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
    /// A class for DateTime to string conversion
    /// </summary>
    public class DateTimeToStringConverter : IValueConverter
    {
        /// <summary>
        /// Converts DateTime to string 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string ret = string.Empty;
            if (value != null)
            {
                ret = ((DateTime)value).ToString("yyyyMMdd");
            }
            return ret;
        }

        /// <summary>
        /// Converts string to DateTIme
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DateTime.Parse((string)value);
        }
    }
}
