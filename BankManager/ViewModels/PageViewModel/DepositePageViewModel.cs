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
    /// ViewModel for DepostePage
    /// </summary>
    public class DepositePageViewModel : ViewModelBase
    {
        // private instance variables
        private DepositePageModel _depositePageModel;
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
        /// Gets DepositeCommand
        /// </summary>
        public ICommand DepositeCommand { get; private set; }

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
                this._depositePageModel.AccountNumber = this._accNum;
            }
        }

        /// <summary>
        /// Gets IsValidAccount
        /// </summary>
        public bool IsValidAccount { get => this._depositePageModel.AccountExist; }

        /// <summary>
        /// Fetches user account information
        /// </summary>
        private void FetchAccouuntInfo()
        {
            if (!this.IsValidAccount) // if invalid show error
            {
                CustomMessageBox.CustomMessageBox customMessageBox = new CustomMessageBox.CustomMessageBox(null, "Warning!", "Invalid account number!", CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_OK | CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_ICONWARNING);
                customMessageBox.ShowDialog();
                return;
            } // else update UI with data
            Tuple<string, decimal> data = this._depositePageModel.AccountNameAndAmount;
            this.ClientFullName = data.Item1;
            this.ClientCurrentBalance = data.Item2;
            this.InvalidateUIElements = true;
            this._canExecuteFetchButton = false;            
        }

        /// <summary>
        /// Deposites amount to the current user account
        /// </summary>
        private void DepositeAccount()
        {
            decimal balanceToBeDeposited = this.ClientCurrentBalance + this.Amount; // add current user balance and the new amount
            TransferInfoStruct transferInfoStruct = new TransferInfoStruct(); // create TransferInfoStruct and fill it with data
            transferInfoStruct.AccNo = this.ClientAccountNumber;
            transferInfoStruct.PersonNo = this._depositePageModel.PersonNumber;
            transferInfoStruct.TransactionType = TransactionType.DEPOSITE;
            transferInfoStruct.TransferDate = DateTime.Now.ToString("yyyy-MM-dd");
            transferInfoStruct.TransferAmout = this.Amount;
            transferInfoStruct.TransferBalanceAfter = balanceToBeDeposited;

            if (!this._depositePageModel.UpdateBalanceToAccount(this.ClientAccountNumber, balanceToBeDeposited)
                || !this._depositePageModel.RegisterNewTransaction(transferInfoStruct)) // if Update of user balace or the regstration of transfer info fails then we show error message and leave
            {
                CustomMessageBox.CustomMessageBox customMessageBox = new CustomMessageBox.CustomMessageBox(null, "Error!", "Couldn't perform deposite operation!", CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_OK | CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_ICONERROR);
                customMessageBox.ShowDialog();
                return;
            }
            // else we Update the UI associated with current balance with new balance
            this.ClientCurrentBalance = balanceToBeDeposited;
        }

        /// <summary>
        /// Resets the current page to initial state
        /// </summary>
        /// <param name="page"></param>
        private void Cancel(Page page)
        {
            this.ResetPage(page, new DepositePageViewModel());
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public DepositePageViewModel()
        {
            this._depositePageModel = new DepositePageModel();
            // initializing all the instance variables and ICommands
            this._accNum = 0;
            this._amount = 0.0m;
            this._clientFullName = string.Empty;
            this._clientCurrentBalance = 0.0m;
            this._invalidateUIElements = false;
            this._canExecuteFetchButton = true;
            this.FetchAccouuntInfoCommand = new RelayCommand(this.FetchAccouuntInfo, () => { return this._canExecuteFetchButton; });
            this.DepositeCommand = new RelayCommand(this.DepositeAccount, () => { return this._amount > 0.0m && !this._canExecuteFetchButton; });
            this.CancelCommand = new RelayCommand<Page>(this.Cancel);
        }
    }
}
