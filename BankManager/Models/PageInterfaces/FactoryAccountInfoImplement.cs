/*
    Dipayan Sarker
    February 10, 2020
*/

using BankManager.Models.Data;
using BankManager.SQLiteCoreEx;
using System;
using System.Data;

namespace BankManager.Models.PageInterfaces
{
    /// <summary>
    /// An extension class with implementation details of IFetchAccountInfo interface
    /// </summary>
    public static class FactoryAccountInfoImplement
    {
        // private static variable that gets ISQLiteCore inerface to communicate with the database
        private static ISQLiteCore _sQLiteCore = FetchAccountFactory.GetFetchAccountInfoClass();

        /// <summary>
        /// Returns true if the account number exists in the database
        /// </summary>
        /// <param name="fatchAccountInfo"></param>
        /// <param name="accNum"></param>
        /// <returns></returns>
        public static bool IsAccountExist(this IFetchAccountInfo fatchAccountInfo, int accNum)
        {
            return _sQLiteCore.IsAccountExist("bm_accCore", accNum); // looks in the bm_accCore table
        }

        /// <summary>
        /// Returns a tuple of Account holder's name and amount
        /// </summary>
        /// <param name="fatchAccountInfo"></param>
        /// <param name="accNum"></param>
        /// <returns></returns>
        public static Tuple<string, decimal> GetNameAndAmount(this IFetchAccountInfo fatchAccountInfo, int accNum)
        {
            return _sQLiteCore.GetNameAndAmount(accNum);
        }

        /// <summary>
        /// Gets the person number of a given account
        /// </summary>
        /// <param name="fetchAccountInfo"></param>
        /// <param name="accNum"></param>
        /// <returns></returns>
        public static string GetPersonNumber(this IFetchAccountInfo fetchAccountInfo, int accNum)
        {
            return _sQLiteCore.GetPersonNumber(accNum);
        }

        /// <summary>
        /// Returns true if the update of balance is successful in the database
        /// </summary>
        /// <param name="fetchAccountInfo"></param>
        /// <param name="accNum"></param>
        /// <param name="newBalance"></param>
        /// <returns></returns>
        public static bool UpdateBalanceInAccount(this IFetchAccountInfo fetchAccountInfo, int accNum, decimal newBalance)
        {
            return _sQLiteCore.UpdateBalanceInAccount(accNum, newBalance);
        }

        /// <summary>
        /// Returns true if transtaction is successfully registered in the db
        /// </summary>
        /// <param name="fetchAccountInfo"></param>
        /// <param name="transferInfoStruct"></param>
        /// <returns></returns>
        public static bool RegisterTranstation(this IFetchAccountInfo fetchAccountInfo, TransferInfoStruct transferInfoStruct)
        {
            return _sQLiteCore.RegisterTransaction(transferInfoStruct);
        }

        /// <summary>
        /// Returns true if a new fixed deposite account creation is successful
        /// </summary>
        /// <param name="fetchAccountInfo"></param>
        /// <param name="fixedAccountDataStruct"></param>
        /// <returns></returns>
        public static bool CreateNewFixedAccount(this IFetchAccountInfo fetchAccountInfo, FixedAccountDataStruct fixedAccountDataStruct)
        {
            return _sQLiteCore.CreateNewFixedAccount(fixedAccountDataStruct);
        }

        /// <summary>
        /// Returns true if a fixed deposite exists or not from a given account number
        /// </summary>
        /// <param name="fetchAccountInfo"></param>
        /// <param name="accNum"></param>
        /// <returns></returns>
        public static bool IsFixedAccountExist(this IFetchAccountInfo fetchAccountInfo, int accNum)
        {
            return _sQLiteCore.IsAccountExist("bm_fixCore", accNum);
        }

        /// <summary>
        /// Returns a DataTable of account transaction details for a given account number from the db
        /// </summary>
        /// <param name="fetchAccountInfo"></param>
        /// <param name="accNum"></param>
        /// <returns></returns>
        public static DataTable GetAccountTransactionDetails(this IFetchAccountInfo fetchAccountInfo, int accNum)
        {
            return _sQLiteCore.GetAccountTransactionDetails(accNum);
        }

        /// <summary>
        /// Returns a DataTable of account details of all the customer from the db
        /// </summary>
        /// <param name="fetchAccountInfo"></param>
        /// <returns></returns>
        public static DataTable GetAccountDetailsAllClients(this IFetchAccountInfo fetchAccountInfo)
        {
            return _sQLiteCore.GetAccountDetailsAll("bm_accCore");
        }

        /// <summary>
        /// Returns a DataTable of account details of all fixed accounts from the db
        /// </summary>
        /// <param name="fetchAccountInfo"></param>
        /// <returns></returns>
        public static DataTable GetAccountDetailsAllFixed(this IFetchAccountInfo fetchAccountInfo)
        {
            return _sQLiteCore.GetAccountDetailsAll("bm_fixCore");
        }

        /// <summary>
        /// Gets a string of new available account number
        /// </summary>
        /// <param name="fetchAccountInfo"></param>
        /// <returns></returns>
        public static string GetNewAccountNumberString(this IFetchAccountInfo fetchAccountInfo)
        {
            return _sQLiteCore.GetNewAccountNumber();
        }

        /// <summary>
        /// Returns true if the creation of a new account is successful in the db
        /// </summary>
        /// <param name="fetchAccountInfo"></param>
        /// <param name="bankDataStruct"></param>
        /// <returns></returns>
        public static bool CreateNewBankAccount(this IFetchAccountInfo fetchAccountInfo, BankDataStruct bankDataStruct)
        {
            return _sQLiteCore.CreateNewAccount(bankDataStruct);
        }

        /// <summary>
        /// Returns true if a given person number exists in the db
        /// </summary>
        /// <param name="fetchAccountInfo"></param>
        /// <param name="personNo"></param>
        /// <returns></returns>
        public static bool ExistsPersonNo(this IFetchAccountInfo fetchAccountInfo, string personNo)
        {
            return _sQLiteCore.ExistsPersonNumber(personNo);
        }

        /// <summary>
        /// Gets BankDataStruct of a given account from the db
        /// </summary>
        /// <param name="fetchAccountInfo"></param>
        /// <param name="accNum"></param>
        /// <returns></returns>
        public static BankDataStruct GetBankDataStruct(this IFetchAccountInfo fetchAccountInfo, int accNum)
        {
            return _sQLiteCore.GetBankDataStruct(accNum);
        }

        /// <summary>
        /// Returns true if the update of customer information is successful in the db
        /// </summary>
        /// <param name="fetchAccountInfo"></param>
        /// <param name="accNum"></param>
        /// <param name="clientInfoArray"></param>
        /// <returns></returns>
        public static bool UpdateClientInfo(this IFetchAccountInfo fetchAccountInfo, int accNum, string[] clientInfoArray)
        {
            return _sQLiteCore.UpdateClientInfo(accNum, clientInfoArray);
        }

        /// <summary>
        /// Returns true if the deletion of an account is successful in the db
        /// </summary>
        /// <param name="fetchAccountInfo"></param>
        /// <param name="accNum"></param>
        /// <param name="hasFixedDeposite"></param>
        /// <returns></returns>
        public static bool DeleteAccount(this IFetchAccountInfo fetchAccountInfo, int accNum, bool hasFixedDeposite)
        {
            return _sQLiteCore.DeleteAccount(accNum, hasFixedDeposite);
        }

        /// <summary>
        /// Returns true if a fixed deposite account exists from given account number
        /// </summary>
        /// <param name="fetchAccountInfo"></param>
        /// <param name="accNum"></param>
        /// <returns></returns>
        public static bool HasFixedDepositeAccount(this IFetchAccountInfo fetchAccountInfo, int accNum)
        {
            return _sQLiteCore.HasFixedDepositeAccount(accNum);
        }
    }
}
