/*
    Dipayan Sarker
    January 30, 2020
 */

namespace BankManager.Models.Data
{
    /// <summary>
    /// A struct with all the bank account info variables
    /// </summary>
    public struct BankDataStruct
    {
        // public variables
        public int AccNo;
        public string FName;
        public string LName;
        public string DOB;
        public string PersonNo;
        public string Address;
        public string PhoneNo;
        public string Email;
        public string PlaceOfBirth;
        public string Citizenship;
        public string Sex;
        public string CivilStatus;
        public decimal Amount;
        public bool HasFixedAccount;
        public string AccCreationDate;

        /// <summary>
        /// A constructor wih 15 params
        /// </summary>
        /// <param name="accNo"></param>
        /// <param name="fName"></param>
        /// <param name="lName"></param>
        /// <param name="dOb"></param>
        /// <param name="personNo"></param>
        /// <param name="address"></param>
        /// <param name="phoneNo"></param>
        /// <param name="email"></param>
        /// <param name="placeOfBirth"></param>
        /// <param name="citizenship"></param>
        /// <param name="sex"></param>
        /// <param name="civilStat"></param>
        /// <param name="amount"></param>
        /// <param name="hasFixedAcc"></param>
        /// <param name="accCreattionDate"></param>
        public BankDataStruct(int accNo, string fName, string lName, string dOb, string personNo, string address, string phoneNo, string email, string placeOfBirth, string citizenship, string sex, string civilStat, decimal amount, bool hasFixedAcc, string accCreattionDate)
        {
            this.AccNo = accNo;
            this.FName = fName;
            this.LName = lName;
            this.DOB = dOb;
            this.PersonNo = personNo;
            this.Address = address;
            this.PhoneNo = phoneNo;
            this.Email = email;
            this.PlaceOfBirth = placeOfBirth;
            this.Citizenship = citizenship;
            this.Sex = sex;
            this.CivilStatus = civilStat;
            this.Amount = amount;
            this.HasFixedAccount = hasFixedAcc;
            this.AccCreationDate = accCreattionDate;
        }
    }
}
