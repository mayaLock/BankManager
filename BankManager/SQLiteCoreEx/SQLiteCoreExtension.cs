/*
    Dipayan Sarker
    January 30, 2020
 */

using BankManager.Helpers;
using BankManager.Models.Data;
using Dapper;
using System;
using System.Configuration;
using System.Data;
using System.Data.SQLite;


// some of the fuctions are repetitive, could've been used some base private functions; however the project itself is quite scaled
// therefore, such optimizations were not performed

namespace BankManager.SQLiteCoreEx
{
    /// <summary>
    /// Implementation details of ISQLiteCore interface
    /// </summary>
    public static class SQLiteCoreExtension
    {
        /// <summary>
        /// Gets the connectionstring from the app config file
        /// </summary>
        /// <param name="sQLiteCore"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private static string _GetConnectionString(this ISQLiteCore sQLiteCore, string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        /// <summary>
        /// Checks if the given username and password matches in the database
        /// </summary>
        /// <param name="sQLiteCore"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool IsSuccessfulLogin(this ISQLiteCore sQLiteCore, string username, string password)
        {
            bool ret = false; // set defualt return value
            using (IDbConnection conn = new SQLiteConnection(sQLiteCore._GetConnectionString())) // create connection to the database
            {
                conn.Open(); // open the connection (may not needed, since we use "using" statement)
                CommandDefinition commandDefinition = new CommandDefinition($"SELECT CASE WHEN EXISTS(SELECT * FROM Login WHERE username='{username}' AND password='{password}') THEN 1 ELSE 0 END;");
                using (IDataReader dataReader = conn.ExecuteReader(commandDefinition)) // perform the above SQL command
                {
                    while (dataReader.Read())
                    {
                        ret = (dataReader.GetValue(0).ToString() == "1" ? true : false); // if the fetched value is "1" then we set return value to true else to false
                    }
                }
                conn.Close(); // close the connection (may not needed, since we use "using" statement)
            }
            return ret; // return the return value
        }

        /// <summary>
        /// Checks if the given account exists in the database or not
        /// </summary>
        /// <param name="sQLiteCore"></param>
        /// <param name="dbName"></param>
        /// <param name="accNo"></param>
        /// <returns></returns>
        public static bool IsAccountExist(this ISQLiteCore sQLiteCore, string dbName, int accNo)
        {
            bool ret = false;
            using (IDbConnection conn = new SQLiteConnection(sQLiteCore._GetConnectionString()))
            {
                conn.Open();
                CommandDefinition commandDefinition = new CommandDefinition($"SELECT CASE WHEN EXISTS(SELECT * FROM {dbName} WHERE AccNum={accNo}) THEN 1 ELSE 0 END;");
                using (IDataReader dataReader = conn.ExecuteReader(commandDefinition))
                {
                    while (dataReader.Read())
                    {
                        ret = (dataReader.GetValue(0).ToString() == "1" ? true : false);
                    }
                }
                conn.Close();
            }
            return ret;
        }

        /// <summary>
        /// Checks if a given person number exists in the database or not
        /// </summary>
        /// <param name="sQLiteCore"></param>
        /// <param name="personNo"></param>
        /// <returns></returns>
        public static bool ExistsPersonNumber(this ISQLiteCore sQLiteCore, string personNo)
        {
            bool ret = false;
            using (IDbConnection conn = new SQLiteConnection(sQLiteCore._GetConnectionString()))
            {
                conn.Open();
                CommandDefinition commandDefinition = new CommandDefinition($"SELECT CASE WHEN EXISTS(SELECT * FROM bm_accCore WHERE PersonNo='{personNo}') THEN 1 ELSE 0 END;");
                using (IDataReader dataReader = conn.ExecuteReader(commandDefinition))
                {
                    while (dataReader.Read())
                    {
                        ret = (dataReader.GetValue(0).ToString() == "1" ? true : false);
                    }
                }
                conn.Close();
            }
            return ret;
        }

        /// <summary>
        /// Gets a string of new possible account number after querying the database
        /// </summary>
        /// <param name="sQLiteCore"></param>
        /// <returns></returns>
        public static string GetNewAccountNumber(this ISQLiteCore sQLiteCore)
        {
            Int64 count = 0; // an initial counter to store current account number
            using (IDbConnection conn = new SQLiteConnection(sQLiteCore._GetConnectionString()))
            {
                conn.Open();
                CommandDefinition commandDefinition = new CommandDefinition($"SELECT AccNum FROM bm_accCore ORDER BY AccNum DESC LIMIT 1;");
                using (IDataReader dataReader = conn.ExecuteReader(commandDefinition))
                {
                    while (dataReader.Read())
                    {
                        count = (Int64)dataReader.GetValue(0); // set the new value to count (Int64) cast is used since SQLite database stores integer as a 64 bit integer
                    }
                }
                conn.Close();
            }
            count++; // so we increment the initial count by one
            return count.ToString("D8"); // "D8" makes sure that the output should be eight character long and empty characters will be padded with Zero(0)s. e.g. -> 1 => 00000001
        }

        /// <summary>
        /// Creates a new account from the given BankDataStruct
        /// </summary>
        /// <param name="sQLiteCore"></param>
        /// <param name="bankDataStruct"></param>
        /// <returns></returns>
        public static bool CreateNewAccount(this ISQLiteCore sQLiteCore, BankDataStruct bankDataStruct)
        {
            bool ret = false;
            using (IDbConnection conn = new SQLiteConnection(sQLiteCore._GetConnectionString()))
            {
                conn.Open();
                CommandDefinition commandDefinition = new CommandDefinition($"INSERT INTO bm_accCore values ({bankDataStruct.AccNo}, '{bankDataStruct.FName}'," +
                    $" '{bankDataStruct.LName}', '{bankDataStruct.DOB}', '{bankDataStruct.PersonNo}', '{bankDataStruct.Address}', '{bankDataStruct.PlaceOfBirth}', " +
                    $"'{bankDataStruct.Citizenship}', '{bankDataStruct.PhoneNo}', '{bankDataStruct.Email}', '{bankDataStruct.Sex}', {bankDataStruct.Amount.ToString(System.Globalization.CultureInfo.InvariantCulture)}, " +
                    $"'{bankDataStruct.HasFixedAccount.ToString()}', '{bankDataStruct.AccCreationDate}', '{bankDataStruct.CivilStatus}');");
                ret = conn.Execute(commandDefinition) > 0; // Execute returns the number of cell it has infected, so if the value is greater than 0, meaning the SQL command was successful
                conn.Close();
            }
            return ret;
        }

        /// <summary>
        /// Registers a transaction in the database from the TransferInfoStruct
        /// </summary>
        /// <param name="sQLiteCore"></param>
        /// <param name="transferInfoStruct"></param>
        /// <returns></returns>
        public static bool RegisterTransaction(this ISQLiteCore sQLiteCore, TransferInfoStruct transferInfoStruct)
        {
            bool ret = false;
            using (IDbConnection conn = new SQLiteConnection(sQLiteCore._GetConnectionString()))
            {
                conn.Open();
                CommandDefinition commandDefinition = new CommandDefinition($"INSERT INTO bm_transferCore values ({transferInfoStruct.AccNo}, '{transferInfoStruct.PersonNo}'," +
                    $" '{transferInfoStruct.TransactionType.TransactionTypeToString()}', '{transferInfoStruct.TransferDate}', '{transferInfoStruct.TransferAmout.ToString(System.Globalization.CultureInfo.InvariantCulture)}'," +
                    $" '{transferInfoStruct.TransferBalanceAfter.ToString(System.Globalization.CultureInfo.InvariantCulture)}');");
                ret = conn.Execute(commandDefinition) > 0;
                conn.Close();
            }
            return ret;
        }

        /// <summary>
        /// Creates a fixed account in the database from the given FixedAccountDataStruct
        /// </summary>
        /// <param name="sQLiteCore"></param>
        /// <param name="fixedAccountDataStruct"></param>
        /// <returns></returns>
        public static bool CreateNewFixedAccount(this ISQLiteCore sQLiteCore, FixedAccountDataStruct fixedAccountDataStruct)
        {
            bool ret = false;
            using (IDbConnection conn = new SQLiteConnection(sQLiteCore._GetConnectionString()))
            {
                conn.Open();
                CommandDefinition commandDefinition = new CommandDefinition($"INSERT INTO bm_fixCore values ({fixedAccountDataStruct.AccNo}, '{fixedAccountDataStruct.FName}'," +
                    $" '{fixedAccountDataStruct.LName}', '{fixedAccountDataStruct.FixedAmount.ToString(System.Globalization.CultureInfo.InvariantCulture)}'," +
                    $" '{fixedAccountDataStruct.MaturityAmount.ToString(System.Globalization.CultureInfo.InvariantCulture)}', '{fixedAccountDataStruct.MaturityDate}', '{fixedAccountDataStruct.InterestRate.ToString(System.Globalization.CultureInfo.InvariantCulture)}', " +
                    $"'{fixedAccountDataStruct.FixedAccountCreationDate}');");
                ret = conn.Execute(commandDefinition) > 0; // infected cell , first create a new entry in bm_fileCore table 
                CommandDefinition commandDefinitionUpdateHasFixed = new CommandDefinition($"UPDATE bm_accCore SET HasFixedAcc='True' WHERE AccNum={fixedAccountDataStruct.AccNo};");
                ret = conn.Execute(commandDefinitionUpdateHasFixed) > 0; // then update the value of HasFixedAcc to ture in bm_accCore table
                conn.Close();
            }
            return ret;
        }

        /// <summary>
        /// Updates the account balance of a given account number with provided values
        /// </summary>
        /// <param name="sQLiteCore"></param>
        /// <param name="accNo"></param>
        /// <param name="newBalance"></param>
        /// <returns></returns>
        public static bool UpdateBalanceInAccount(this ISQLiteCore sQLiteCore, int accNo, decimal newBalance)
        {
            bool ret = false;
            using (IDbConnection conn = new SQLiteConnection(sQLiteCore._GetConnectionString()))
            {
                conn.Open();
                CommandDefinition commandDefinition = new CommandDefinition($"UPDATE bm_accCore SET Amount={newBalance.ToString(System.Globalization.CultureInfo.InvariantCulture)} WHERE AccNum={accNo};");
                ret = conn.Execute(commandDefinition) > 0;
                conn.Close();
            }
            return ret;
        }

        /// <summary>
        /// Checks if the provided account number has a fixed account or not
        /// </summary>
        /// <param name="sQLiteCore"></param>
        /// <param name="accNo"></param>
        /// <returns></returns>
        public static bool HasFixedDepositeAccount(this ISQLiteCore sQLiteCore, int accNo)
        {
            bool ret = false;
            using (IDbConnection conn = new SQLiteConnection(sQLiteCore._GetConnectionString()))
            {
                conn.Open();
                CommandDefinition commandDefinition = new CommandDefinition($"SELECT CASE WHEN EXISTS(SELECT * FROM bm_accCore WHERE (AccNum={accNo} AND HasFixedAcc='True')) THEN 1 ELSE 0 END;");
                using (IDataReader dataReader = conn.ExecuteReader(commandDefinition))
                {
                    while (dataReader.Read())
                    {
                        ret = (dataReader.GetValue(0).ToString() == "1" ? true : false);
                    }
                }
                conn.Close();
            }
            return ret;
        }

        /// <summary>
        /// Deletes the given account from the database
        /// If the account has associated Fixed account then that is also gets deleted
        /// </summary>
        /// <param name="sQLiteCore"></param>
        /// <param name="accNo"></param>
        /// <param name="hasFixedDeposite"></param>
        /// <returns></returns>
        public static bool DeleteAccount(this ISQLiteCore sQLiteCore, int accNo, bool hasFixedDeposite)
        {
            bool ret = false;

            if (hasFixedDeposite) // if the user has fixed account
            {
                using (IDbConnection conn = new SQLiteConnection(sQLiteCore._GetConnectionString()))
                {
                    conn.Open();
                    CommandDefinition commandDefinition = new CommandDefinition($"DELETE FROM bm_fixCore WHERE AccNum={accNo};");
                    conn.Execute(commandDefinition); // first delete the account from bm_fixCore
                    conn.Close();
                }
            }

            using (IDbConnection conn = new SQLiteConnection(sQLiteCore._GetConnectionString()))
            {
                conn.Open();
                CommandDefinition commandDefinition = new CommandDefinition($"DELETE FROM bm_accCore WHERE AccNum={accNo};");
                ret = conn.Execute(commandDefinition) > 0; // then delete account from bm_accCore table
                conn.Close();
            }
            return ret;
        }

        /// <summary>
        /// Updates the client's information of the specific account from the given clentInfo array
        /// </summary>
        /// <param name="sQLiteCore"></param>
        /// <param name="accNo"></param>
        /// <param name="clientInfoArray"></param>
        /// <returns></returns>
        public static bool UpdateClientInfo(this ISQLiteCore sQLiteCore, int accNo, string[] clientInfoArray)
        {
            bool ret = false;
            using (IDbConnection conn = new SQLiteConnection(sQLiteCore._GetConnectionString()))
            {
                conn.Open();
                CommandDefinition commandDefinition = new CommandDefinition($"UPDATE bm_accCore SET FName='{clientInfoArray[0]}', LName='{clientInfoArray[1]}'," +
                    $" Address='{clientInfoArray[2]}', PhoneNo='{clientInfoArray[3]}', Email='{clientInfoArray[4]}', Citizenship='{clientInfoArray[5]}', Sex='{clientInfoArray[6]}', CivilStat='{clientInfoArray[7]}' WHERE AccNum={accNo};");
                ret = conn.Execute(commandDefinition) > 0;
                conn.Close();
            }
            return ret;
        }

        /// <summary>
        /// Gets BankDataStruct of a specific account
        /// </summary>
        /// <param name="sQLiteCore"></param>
        /// <param name="accNo"></param>
        /// <returns></returns>
        public static BankDataStruct GetBankDataStruct(this ISQLiteCore sQLiteCore, int accNo)
        {
            BankDataStruct bankDataStruct = new BankDataStruct();
            object[] tempObjArray = new object[15]; // total 15 elements in one database row
            using (IDbConnection conn = new SQLiteConnection(sQLiteCore._GetConnectionString()))
            {
                conn.Open();
                CommandDefinition commandDefinition = new CommandDefinition($"SELECT * FROM bm_accCore WHERE AccNum={accNo};");
                using (IDataReader dataReader = conn.ExecuteReader(commandDefinition)) // we ask for all the info of a given account number
                {
                    while (dataReader.Read())
                    {
                        dataReader.GetValues(tempObjArray); // GetValues() fills the tempObjArray with all the fetched values
                    }
                }
                conn.Close();
            }

            // we fill the BankDataStuct wtih feched data as well perform required cast if needed
            bankDataStruct.AccNo = (int)((Int64)tempObjArray[0]); // first we cast the object to Int64 then we cast it again to int
            bankDataStruct.FName = tempObjArray[1].ToString();
            bankDataStruct.LName = tempObjArray[2].ToString();
            bankDataStruct.DOB = tempObjArray[3].ToString();
            bankDataStruct.PersonNo = tempObjArray[4].ToString();
            bankDataStruct.Address = tempObjArray[5].ToString();
            bankDataStruct.PlaceOfBirth = tempObjArray[6].ToString();
            bankDataStruct.Citizenship = tempObjArray[7].ToString();
            bankDataStruct.PhoneNo = tempObjArray[8].ToString();
            bankDataStruct.Email = tempObjArray[9].ToString();
            bankDataStruct.Sex = tempObjArray[10].ToString();
            bankDataStruct.Amount = (decimal)((double)tempObjArray[11]); // first we cast the object to double (since SQLite stores real values as double) then we cast it again to decimal
            bankDataStruct.HasFixedAccount = tempObjArray[12].ToString().Equals("True") ? true : false;
            bankDataStruct.AccCreationDate = tempObjArray[13].ToString();
            bankDataStruct.CivilStatus = tempObjArray[14].ToString();
            return bankDataStruct;
        }

        /// <summary>
        /// Gents a Tuple of name of the account holder as well as the current balance
        /// </summary>
        /// <param name="sQLiteCore"></param>
        /// <param name="accNo"></param>
        /// <returns></returns>
        public static Tuple<string, decimal> GetNameAndAmount(this ISQLiteCore sQLiteCore, int accNo)
        {
            // we set the initial default values
            string fullName = string.Empty;
            decimal amount = 0.0m;

            using (IDbConnection conn = new SQLiteConnection(sQLiteCore._GetConnectionString()))
            {
                conn.Open();
                CommandDefinition commandDefinition = new CommandDefinition($"SELECT Amount, FName || ' ' || LName AS expr1 FROM bm_accCore WHERE AccNum={accNo};"); // we concat FName and LName with ' ' in beetween
                using (IDataReader dataReader = conn.ExecuteReader(commandDefinition))
                {
                    while (dataReader.Read())
                    {
                        amount = (decimal)((double)dataReader.GetValue(0));
                        fullName = dataReader.GetValue(1).ToString();
                    }
                }
                conn.Close();
            }
            return new Tuple<string, decimal>(fullName, amount); // we create a new Tuple with the fullName and amount and then return it
        }

        /// <summary>
        /// Returns the personal number of a specific account as string
        /// </summary>
        /// <param name="sQLiteCore"></param>
        /// <param name="accNo"></param>
        /// <returns></returns>
        public static string GetPersonNumber(this ISQLiteCore sQLiteCore, int accNo)//SELECT PersonNo FROM bm_accCore WHERE AccNum=1;
        {
            string personNo = string.Empty;
            using (IDbConnection conn = new SQLiteConnection(sQLiteCore._GetConnectionString()))
            {
                conn.Open();
                CommandDefinition commandDefinition = new CommandDefinition($"SELECT PersonNo FROM bm_accCore WHERE AccNum={accNo};");
                using (IDataReader dataReader = conn.ExecuteReader(commandDefinition))
                {
                    while (dataReader.Read())
                    {
                        personNo = dataReader.GetValue(0).ToString();
                    }
                }
                conn.Close();
            }
            return personNo;
        }

        /// <summary>
        /// Returns a Datatable with all the rows and columns of the entire table (tableName) from the database
        /// </summary>
        /// <param name="sQLiteCore"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static DataTable GetAccountDetailsAll(this ISQLiteCore sQLiteCore, string tableName)
        {
            DataTable dataTable = new DataTable(); // we create an empty instance of DataTable object
            using (IDbConnection conn = new SQLiteConnection(sQLiteCore._GetConnectionString()))
            {
                conn.Open();
                CommandDefinition commandDefinition = new CommandDefinition($"SELECT * FROM {tableName};");
                using (IDataReader dataReader = conn.ExecuteReader(commandDefinition))
                {
                    dataTable.Load(dataReader); // Load() fills the dataTable by using dataReader object
                }
                conn.Close();
            }
            return dataTable;
        }

        /// <summary>
        /// Gets a DataTable with all the account transaction info of a specific account
        /// </summary>
        /// <param name="sQLiteCore"></param>
        /// <param name="accNo"></param>
        /// <returns></returns>
        public static DataTable GetAccountTransactionDetails(this ISQLiteCore sQLiteCore, int accNo)
        {
            DataTable dataTable = new DataTable();
            using (IDbConnection conn = new SQLiteConnection(sQLiteCore._GetConnectionString()))
            {
                conn.Open();
                CommandDefinition commandDefinition = new CommandDefinition($"SELECT * FROM bm_transferCore WHERE AccNum='{accNo}';");
                using (IDataReader dataReader = conn.ExecuteReader(commandDefinition))
                {
                    dataTable.Load(dataReader);
                }
                conn.Close();
            }
            return dataTable;
        }
    }
}
