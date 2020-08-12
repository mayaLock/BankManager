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
    /// ViewModel for WithdrawPage
    /// </summary>
    public class WithdrawPageViewModel : ViewModelBase
    {
        // private instance variables
        private WithdrawPageModel _withdrawPageModel;
        private int _accNum;
        private string _clientFullName;
        private decimal _clientCurrentBalance;
        private decimal _amount;
        private bool _invalidateUIElements;
        private bool _canExecuteFetchButton;

        /// <summary>
        /// Gets FetchAccouuntInfoCommand
        /// </summary>
        public ICommand FetchAccouuntInfoCommand { get; private set; }

        /// <summary>
        /// Gets WithdrawCommand
        /// </summary>
        public ICommand WithdrawCommand { get; private set; }

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
                this._withdrawPageModel.AccountNumber = this._accNum;
            }
        }

        /// <summary>
        /// Gets IsValidAccount
        /// </summary>
        public bool IsValidAccount { get => this._withdrawPageModel.AccountExist; }

        /// <summary>
        /// Gets the account information such as client's full name and current account balance
        /// </summary>
        private void FetchAccouuntInfo()
        {
            if (!this.IsValidAccount) // if invalid account we show error message and return
            {
                CustomMessageBox.CustomMessageBox customMessageBox = new CustomMessageBox.CustomMessageBox(null, "Stop!", "Invalid account number!", CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_OK | CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_ICONSTOP);
                customMessageBox.ShowDialog();
                return;
            }
            Tuple<string, decimal> data = this._withdrawPageModel.AccountNameAndAmount; // Tuple with client name and balance
            this.ClientFullName = data.Item1;
            this.ClientCurrentBalance = data.Item2;
            this.InvalidateUIElements = true; // we signal that UI look should change
            this._canExecuteFetchButton = false; // we disable the fetch button
        }

        /// <summary>
        /// This method is called to withdraw amount from the current client's account
        /// </summary>
        private void WithdrawAccount()
        {
            decimal balanceToBeWithdrawn = this.ClientCurrentBalance - this.Amount; // we substract amount to be withdrawn from current account balance
            TransferInfoStruct transferInfoStruct = new TransferInfoStruct(); // we instantiate a new TransferInfoStruct and fill all the required variables
            transferInfoStruct.AccNo = this.ClientAccountNumber;
            transferInfoStruct.PersonNo = this._withdrawPageModel.PersonNumber;
            transferInfoStruct.TransactionType = TransactionType.WITHDRAW;
            transferInfoStruct.TransferDate = DateTime.Now.ToString("yyyy-MM-dd");
            transferInfoStruct.TransferAmout = this.Amount;
            transferInfoStruct.TransferBalanceAfter = balanceToBeWithdrawn;

            if (!this._withdrawPageModel.UpdateBalanceToAccount(this.ClientAccountNumber, balanceToBeWithdrawn) 
                || !this._withdrawPageModel.RegisterNewTransaction(transferInfoStruct)) // if update or registration of new transaction fails we show error message and return
            {
                CustomMessageBox.CustomMessageBox customMessageBox = new CustomMessageBox.CustomMessageBox(null, "Error!", "Couldn't perform withdraw operation!", CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_OK | CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_ICONERROR);
                customMessageBox.ShowDialog();
                return;
            }

            this.ClientCurrentBalance = balanceToBeWithdrawn; // finally we update the UI (since ClientCurrentBalance is bind to the UI element that shows the current user balance
        }

        /// <summary>
        /// Resets the viewModel to its initial state
        /// </summary>
        /// <param name="page"></param>
        private void Cancel(Page page)
        {
            this.ResetPage(page, new WithdrawPageViewModel());
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public WithdrawPageViewModel()
        {
            this._withdrawPageModel = new WithdrawPageModel(); // instantiating a new WithdrawPageModel to get data
            // initializing all the instance variables and ICommands
            this._accNum = 0;
            this._amount = 0.0m;
            this._clientFullName = string.Empty;
            this._clientCurrentBalance = 0.0m;
            this._invalidateUIElements = false;
            this._canExecuteFetchButton = true;
            this.FetchAccouuntInfoCommand = new RelayCommand(this.FetchAccouuntInfo, () => { return this._canExecuteFetchButton; });
            this.WithdrawCommand = new RelayCommand(this.WithdrawAccount, () => { return this._amount > 0.0m && !this._canExecuteFetchButton; });
            this.CancelCommand = new RelayCommand<Page>(this.Cancel);
        }
    }
}
