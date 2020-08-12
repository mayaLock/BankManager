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
    /// A class for Dacimal value to string conversion
    /// </summary>
    public class DecimalToStringConverter : IValueConverter
    {
        /// <summary>
        /// Converts decimal value to string
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
                return FormattableString.Invariant($"{(decimal)value:0.00}");
            }
            return string.Empty;
        }

        /// <summary>
        /// Converts string value to decimal
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal ret = 0.0m;
            if (decimal.TryParse((string)value, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.InvariantCulture, out ret))
            {
                return ret;
            }
            return ret;
        }
    }
}
