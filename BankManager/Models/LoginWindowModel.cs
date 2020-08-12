/*
    Dipayan Sarker
    February 10, 2020
*/

using BankManager.SQLiteCoreEx;

namespace BankManager.Models
{
    /// <summary>
    /// Model for LoginWindow Window
    /// </summary>
    public class LoginWindowModel : ISQLiteCore
    {
        /// <summary>
        /// Gets or Sets UserName
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or Sets Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Returns true of login was successful
        /// </summary>
        public bool IsSuccessfulLogin
        {
            get => this.IsSuccessfulLogin(this.UserName, this.Password);            
        }        
    }
}
