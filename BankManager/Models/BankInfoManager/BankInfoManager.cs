/*
    Dipayan Sarker
    February 10, 2020
*/

using BankManager.Models.Data;
using BankManager.Models.PageInterfaces;
using System;
using System.Data;

namespace BankManager.Models.BankInfoManager
{
    /// <summary>
    /// A class to communicate with database to fetch or store data
    /// It's a wrapper class that interacts with the database
    /// </summary>
    public class BankInfoManager : IFetchAccountInfo
    {
        // private instance variables
        private int _accNum;
        private string _personNo;

        /// <summary>
        /// Sets AccountNumber
        /// </summary>
        public int AccountNumber { set => this._accNum = value; }

        /// <summary>
        /// Gets AccountNameAndAmount
        /// </summary>
        public Tuple<string, decimal> AccountNameAndAmount
        {
            get
            {
                Tuple<string, decimal> data = null;
                if (this.AccountExist) // if account number exists
                {
                    data = this.GetNameAndAmount(this._accNum); // we set data with the fetched value
                }
                return data;
            }
        }

        /// <summary>
        /// Returns a tuple of number of transfactions, pdf file name, and pdf file content
        /// </summary>
        public Tuple<int, string, string> TranstationCountAndPdfFileNameAndPdfContentString
        {
            get
            {
                // default values for the values that will be returned 
                int transactionCount = 0;
                string pdfFileNamme = string.Empty;
                string pdfContent = string.Empty;
                if (this.AccountExist) // if account exists in the db
                {
                    DataTable dataTable = this.GetAccountTransactionDetails(this._accNum); // we get the DataTable that contains all the transfaction data
                    transactionCount = dataTable.Rows.Count; // we set the transfaction count
                    pdfFileNamme = $"{dataTable.Rows[0].ItemArray[1]}_{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}.pdf"; // we set the pdf file name with current date and time attached to it
                    string template = string.Empty;
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        template += FormattableString.Invariant(
                            $"<tr><td>{dataTable.Rows[i].ItemArray[3]}</td><td>{dataTable.Rows[i].ItemArray[2]}</td><td>{dataTable.Rows[i].ItemArray[4]}</td><td>{dataTable.Rows[i].ItemArray[5]}</td></tr>");
                    }
                    pdfContent = $"<style>table, th, td {{ border: 2px solid #426A8A; }} h1 " +
                        $"{{ color: #FF3E42; }} h2 {{ color: #1E3C94; }} </style><h1>Account: {((Int64)(dataTable.Rows[0].ItemArray[0])).ToString("D8")}</h1><h2>" +
                        $"Person No: {dataTable.Rows[0].ItemArray[1]}</hr><h3></h3><table><tr><th>Transaction Date</th><th>Transaction Type" +
                        $"</th><th>Amount</th><th>Balance</th></tr>{template}</table>"; // we set the pdf content string with data, this is a HTML string

                }
                return new Tuple<int, string, string>(transactionCount, pdfFileNamme, pdfContent);
            }
        }

        /// <summary>
        /// Returns a DataTable of account details of all the custonmers from db
        /// </summary>
        public DataTable AllAccountDataTable
        {
            get => this.GetAccountDetailsAllClients();            
        }

        /// <summary>
        /// Returns a DataTable of fixed account details of all the customers from db
        /// </summary>
        public DataTable ALLFixedAccountDataTable
        {
            get => this.GetAccountDetailsAllFixed();
        }

        /// <summary>
        /// Gets a string of new available account number
        /// </summary>
        public string NewAccountNumberString
        {
            get => this.GetNewAccountNumberString();
        }

        /// <summary>
        /// Gets person number
        /// </summary>
        public string PersonNumber
        {
            get
            {
                string ret = string.Empty;
                if (this.AccountExist) // if account exists
                {
                    ret = this.GetPersonNumber(this._accNum); // we set the return value with the fetched data
                }
                return ret;
            }
        }

        /// <summary>
        /// Gets BankDataInfo as a struct
        /// </summary>
        public BankDataStruct BankDataInfo
        {
            get
            {
                BankDataStruct bankDataStruct = new BankDataStruct();
                if (this.AccountExist)
                {
                    bankDataStruct = this.GetBankDataStruct(this._accNum);
                }
                return bankDataStruct;
            }
        }

        /// <summary>
        /// Gets whether account number exists in the db or not
        /// </summary>
        public bool AccountExist
        {
            get
            {
                if (this._accNum != 0) // if account number is not 0 (which implies that it is an invalid account number
                {
                    return this.IsAccountExist(this._accNum); // we set the return value
                }
                return false;
            }
        }

        /// <summary>
        /// Sets person number that we will check with other method calls
        /// </summary>
        public string PersonNumberForCheckExist
        {
            set => this._personNo = value;
        }

        /// <summary>
        /// Gets whether a person number exists or not
        /// </summary>
        public bool PersonNumberExist
        {
            get
            {
                bool ret = false;
                if (!string.IsNullOrEmpty(this._personNo))
                {
                    ret = this.ExistsPersonNo(this._personNo);
                }
                return ret;
            }
        }

        /// <summary>
        /// Gets whether a fixed account number exists or not
        /// </summary>
        public bool FixedAccountExist
        {
            get
            {
                bool ret = false;
                if (this.AccountExist)
                {
                    ret = this.IsFixedAccountExist(this._accNum);
                }
                return ret;
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public BankInfoManager()
        {
            // initializing the instance variables
            this._accNum = 0;
            this._personNo = string.Empty;
        }

        /// <summary>
        /// Returns true if the update of balance is successful in the database
        /// </summary>
        /// <param name="accNum"></param>
        /// <param name="newBalance"></param>
        /// <returns></returns>
        public bool UpdateBalanceToAccount(int accNum, decimal newBalance)
        {
            bool ret = false;
            if (this.AccountExist)
            {
                ret = this.UpdateBalanceInAccount(accNum, newBalance);
            }
            return ret;
        }

        /// <summary>
        /// Returns true if the update of the customer information is successful in the db
        /// </summary>
        /// <param name="clientInfoArray"></param>
        /// <returns></returns>
        public bool UpdateClientInformation(string[] clientInfoArray)
        {
            bool ret = false;
            if (this.AccountExist)
            {
                ret = this.UpdateClientInfo(this._accNum, clientInfoArray);
            }
            return ret;
        }

        /// <summary>
        /// Returns true if the deletion of an account is successful in the db
        /// </summary>
        /// <returns></returns>
        public bool DeleteClientAccount()
        {
            bool ret = false;
            if (this.AccountExist)
            {
                ret = this.DeleteAccount(this._accNum, this.HasFixedDepositeAccount(this._accNum));
            }
            return ret;
        }

        /// <summary>
        /// Returns true if transfaction is successfully registered in the db
        /// </summary>
        /// <param name="transferInfoStruct"></param>
        /// <returns></returns>
        public bool RegisterNewTransaction(TransferInfoStruct transferInfoStruct)
        {
            bool ret = false;
            if (this.AccountExist)
            {
                ret = this.RegisterTranstation(transferInfoStruct); // if account number exists in the db we register the new account
            }
            return ret;
        }

        /// <summary>
        /// Returns true if a new fixed deposite account creation was successful
        /// </summary>
        /// <param name="fixedAccountDataStruct"></param>
        /// <returns></returns>
        public bool CreateFixedAccount(FixedAccountDataStruct fixedAccountDataStruct)
        {
            bool ret = false;
            if (this.AccountExist)
            {
                ret = this.CreateNewFixedAccount(fixedAccountDataStruct);
            }
            return ret;
        }

        /// <summary>
        /// Returns true if the creation of new bank account is successful
        /// </summary>
        /// <param name="bankDataStruct"></param>
        /// <returns></returns>
        public bool CreateBankAccount(BankDataStruct bankDataStruct)
        {
            bool ret = false;
            if (bankDataStruct.AccNo != 0) // if the account is not an invalid account number
            {
                ret = this.CreateNewBankAccount(bankDataStruct); // then we create the account
            }
            return ret;
        }
    }
}
