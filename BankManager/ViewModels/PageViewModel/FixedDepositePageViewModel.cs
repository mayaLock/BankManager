/*
    Dipayan Sarker
    February 10, 2020
*/

using BankManager.Helpers;
using BankManager.Models.Data;
using BankManager.Models.PageModels;
using EasyMVVM.ViewModels;
using EasyMVVM.Commands;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace BankManager.ViewModels.PageViewModel
{
    /// <summary>
    /// ViewModel for FixedDepositePage
    /// </summary>
    public class FixedDepositePageViewModel : ViewModelBase
    {
        // private instance variables
        private FixedDepositePageModel _fixedDepositePageModel;
        private int _accNum;
        private string _clientFullName;
        private decimal _clientCurrentBalance;
        private decimal _amount;
        private bool _invalidateUIElements;
        private bool _canExecuteFetchButton;
        private int _numberOfYears;
        private decimal _interestRate;

        /// <summary>
        /// Gets FetchAccouuntInfoCommand
        /// </summary>
        public ICommand FetchAccouuntInfoCommand { get; private set; }

        /// <summary>
        /// Gets CreateCommand
        /// </summary>
        public ICommand CreateCommand { get; private set; }

        /// <summary>
        /// Gets CancelCommand
        /// </summary>
        public ICommand CancelCommand { get; private set; }

        /// <summary>
        /// Gets or Sets InvalidateUIElements 
        /// </summary>
        public bool InvalidateUIElements
        {
            get => this._invalidateUIElements;
            set => this.SetProperty(ref this._invalidateUIElements, value);
        }

        /// <summary>
        /// Gets or Sets NumberOfYearsForFixedDeposite 
        /// </summary>
        public int NumberOfYearsForFixedDeposite
        {
            get => this._numberOfYears;
            set => this.SetProperty(ref this._numberOfYears, value);
        }

        /// <summary>
        /// Gets or Sets InterestRate
        /// </summary>
        public decimal InterestRate
        {
            get => this._interestRate;
            set => this.SetProperty(ref this._interestRate, value);
        }

        /// <summary>
        /// Gets or Sets Amount
        /// </summary>
        public decimal Amount
        {
            get => this._amount;
            set => this.SetProperty(ref this._amount, value);
        }

        /// <summary>
        /// Gets or Sets ClientFullName
        /// </summary>
        public string ClientFullName
        {
            get => this._clientFullName;
            set => this.SetProperty(ref this._clientFullName, value);
        }

        /// <summary>
        /// Gets or Sets ClientCurrentBalance 
        /// </summary>
        public decimal ClientCurrentBalance
        {
            get => this._clientCurrentBalance;
            set => this.SetProperty(ref this._clientCurrentBalance, value);
        }

        /// <summary>
        /// Gets or Sets ClientAccountNumber
        /// </summary>
        public int ClientAccountNumber
        {
            get => this._accNum;
            set
            {
                this.SetProperty(ref this._accNum, value);
                this._fixedDepositePageModel.AccountNumber = this._accNum;
            }
        }

        /// <summary>
        /// Gets IsValidAccount
        /// </summary>
        public bool IsValidAccount { get => this._fixedDepositePageModel.AccountExist; }

        /// <summary>
        /// Gets the user account information data and updates associated UI element
        /// </summary>
        private void FetchAccouuntInfo()
        {
            if (!this.IsValidAccount) // if invalid account we leave
            {
                CustomMessageBox.CustomMessageBox customMessageBox = new CustomMessageBox.CustomMessageBox(null, "Warning!", "invalid account number!", CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_OK | CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_ICONWARNING);
                customMessageBox.ShowDialog();
                return;
            } // else we fill up the data
            Tuple<string, decimal> data = this._fixedDepositePageModel.AccountNameAndAmount;
            this.ClientFullName = data.Item1;
            this.ClientCurrentBalance = data.Item2;
            this.InvalidateUIElements = true;
            this._canExecuteFetchButton = false;
        }

        /// <summary>
        /// Creates Fixed deposite account
        /// </summary>
        /// <param name="page"></param>
        private void CreateFixedAccount(Page page)
        {
            if (this._fixedDepositePageModel.FixedAccountExist) // if current user already has a fixed account then we leave
            {
                CustomMessageBox.CustomMessageBox customMessageBox = new CustomMessageBox.CustomMessageBox(null, "Warning!", "Account has already fixed deposite account associated with it!", CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_OK | CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_ICONWARNING);
                customMessageBox.ShowDialog();
                return;
            } // else

            decimal balanceToBeDepositedForFixedAccount = this.Amount; // we set the fixed deposite amount
            decimal balanceToBeAfterDepoisteSenderAccount = this._clientCurrentBalance - balanceToBeDepositedForFixedAccount; // we substract current balance with the fixed deposite balance

            FixedAccountDataStruct fixedAccountDataStruct = new FixedAccountDataStruct(); // we create new FixedAccountDataStruct and fill it up with data
            fixedAccountDataStruct.AccNo = this._accNum;
            fixedAccountDataStruct.FName = this._clientFullName.Substring(0, this._clientFullName.LastIndexOf(' ')); // gets Given names such as Mott Thato Dogg => Mott Thato
            fixedAccountDataStruct.LName = this._clientFullName.Substring(this._clientFullName.LastIndexOf(' ') + 1); // gets Last name such as Mott Thato Dogg => Dogg
            fixedAccountDataStruct.FixedAmount = balanceToBeDepositedForFixedAccount;
            fixedAccountDataStruct.MaturityAmount = balanceToBeDepositedForFixedAccount; // since interest will be added at the end of each year
            fixedAccountDataStruct.MaturityDate = DateTime.Now.AddYears(this._numberOfYears).AddDays(-1).ToString("yyyy-MM-dd");
            fixedAccountDataStruct.InterestRate = this._interestRate;
            fixedAccountDataStruct.FixedAccountCreationDate = DateTime.Now.ToString("yyyy-MM-dd");

            TransferInfoStruct transferInfoStruct = new TransferInfoStruct();  // we create a TransferInfoStruct and fill with data
            transferInfoStruct.AccNo = this.ClientAccountNumber;
            transferInfoStruct.PersonNo = this._fixedDepositePageModel.PersonNumber;
            transferInfoStruct.TransactionType = TransactionType.FIXED;
            transferInfoStruct.TransferDate = DateTime.Now.ToString("yyyy-MM-dd");
            transferInfoStruct.TransferAmout = balanceToBeDepositedForFixedAccount;
            transferInfoStruct.TransferBalanceAfter = balanceToBeAfterDepoisteSenderAccount;

            if (!this._fixedDepositePageModel.CreateFixedAccount(fixedAccountDataStruct)
                || !this._fixedDepositePageModel.RegisterNewTransaction(transferInfoStruct)
                || !this._fixedDepositePageModel.UpdateBalanceToAccount(this._accNum, this._clientCurrentBalance)) // if creation of fixed account or update of transaction info fails we show error and return
            {
                CustomMessageBox.CustomMessageBox customMessageBox = new CustomMessageBox.CustomMessageBox(null, "Error!", "Couldn't create fixed deposite account!", CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_OK | CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_ICONERROR);
                customMessageBox.ShowDialog();
                return;
            } // else we show success message and reset the page

            CustomMessageBox.CustomMessageBox customMessageBox1 = new CustomMessageBox.CustomMessageBox(null, "Info!", "Done creating fixed deposite account!", CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_OK | CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_ICONINFORMATION);
            customMessageBox1.ShowDialog();
            this.ResetPage(page, new FixedDepositePageViewModel());
            return;
        }

        /// <summary>
        /// Resets the current page viewModel
        /// </summary>
        /// <param name="page"></param>
        private void Cancel(Page page)
        {
            this.ResetPage(page, new FixedDepositePageViewModel());
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public FixedDepositePageViewModel()
        {
            this._fixedDepositePageModel = new FixedDepositePageModel();
            // initializing all the instance variables and ICommands
            this._accNum = 0;
            this._amount = 0.0m;
            this._clientFullName = string.Empty;
            this._clientCurrentBalance = 0.0m;
            this._invalidateUIElements = false;
            this._canExecuteFetchButton = true;
            this._numberOfYears = 0;
            this._interestRate = 0.0m;
            this.FetchAccouuntInfoCommand = new RelayCommand(this.FetchAccouuntInfo, () => { return this._canExecuteFetchButton; });
            this.CreateCommand = new RelayCommand<Page>(this.CreateFixedAccount, (page) => { return this._amount >= 100_000.0m
                && this._clientCurrentBalance >= this._amount
                && this._interestRate > 0.0m
                && this._numberOfYears > 0
                && !this._canExecuteFetchButton; });
            this.CancelCommand = new RelayCommand<Page>(this.Cancel);
        }
    }
}
