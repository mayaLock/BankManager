/*
    Dipayan Sarker
    January 30, 2020
 */

namespace BankManager.Models.Data
{
    /// <summary>
    /// A struct for transfer data
    /// </summary>
    public struct TransferInfoStruct
    {
        // public variables
        public int TransactionID; //ROWID, N/A in the app
        public int AccNo;
        public string PersonNo;
        public TransactionType TransactionType;
        public string TransferDate;
        public decimal TransferAmout;
        public decimal TransferBalanceAfter;

        /// <summary>
        /// A constructor with 7 params
        /// </summary>
        /// <param name="transID"></param>
        /// <param name="accNo"></param>
        /// <param name="personNo"></param>
        /// <param name="transactionType"></param>
        /// <param name="transferDate"></param>
        /// <param name="transAmount"></param>
        /// <param name="transBalanceAfter"></param>
        public TransferInfoStruct(int transID, int accNo, string personNo, TransactionType transactionType, string transferDate, decimal transAmount, decimal transBalanceAfter)
        {
            this.TransactionID = transID;
            this.AccNo = accNo;
            this.PersonNo = personNo;
            this.TransactionType = transactionType;
            this.TransferDate = transferDate;
            this.TransferAmout = transAmount;
            this.TransferBalanceAfter = transBalanceAfter;
        }
    }
}
