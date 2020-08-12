/*
    Dipayan Sarker
    February 10, 2020
*/

using BankManager.SQLiteCoreEx;

namespace BankManager.Models.PageInterfaces
{
    /// <summary>
    /// A factory class that creates an instance of FetchAccountInfo
    /// </summary>
    public static class FetchAccountFactory
    {
        /// <summary>
        /// Gets ISQLiteCore interface object to access all its capabilities
        /// </summary>
        /// <returns></returns>
        public static ISQLiteCore GetFetchAccountInfoClass()
        {
            return new FetchAccountInfo();
        }
    }
}
