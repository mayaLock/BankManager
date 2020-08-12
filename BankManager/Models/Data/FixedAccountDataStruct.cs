/*
    Dipayan Sarker
    January 30, 2020
 */

namespace BankManager.Models.Data
{
    /// <summary>
    /// A struct for Fixed account data
    /// </summary>
    public struct FixedAccountDataStruct
    {
        // public variables
        public int AccNo;
        public string FName;
        public string LName;
        public decimal FixedAmount;
        public decimal MaturityAmount;
        public string MaturityDate;
        public decimal InterestRate;
        public string FixedAccountCreationDate;

        /// <summary>
        /// A constructor with 8 params
        /// </summary>
        /// <param name="accNo"></param>
        /// <param name="fName"></param>
        /// <param name="lName"></param>
        /// <param name="fixedAmout"></param>
        /// <param name="maturityAmout"></param>
        /// <param name="maturityDate"></param>
        /// <param name="interstRate"></param>
        /// <param name="fixedAccountCreationDate"></param>
        public FixedAccountDataStruct(int accNo, string fName, string lName, decimal fixedAmout, decimal maturityAmout, string maturityDate, decimal interstRate, string fixedAccountCreationDate)
        {
            this.AccNo = accNo;
            this.FName = fName;
            this.LName = lName;
            this.FixedAmount = fixedAmout;
            this.MaturityAmount = maturityAmout;
            this.MaturityDate = maturityDate;
            this.InterestRate = interstRate;
            this.FixedAccountCreationDate = fixedAccountCreationDate;
        }
    }
}
