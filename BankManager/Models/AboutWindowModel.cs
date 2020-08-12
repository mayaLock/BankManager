/*
    Dipayan Sarker
    February 10, 2020
*/

namespace BankManager.Models
{
    /// <summary>
    /// Model for AboutWindow Window
    /// </summary>
    public class AboutWindowModel
    {
        /// <summary>
        /// Gets AssemblyVersion
        /// </summary>
        public string AssemblyVersion { get => System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
    }
}
