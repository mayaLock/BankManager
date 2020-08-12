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
    /// ViewModel for TransferPage
    /// </summary>
    public class TransferPageViewModel : ViewModelBase
    {
        // private instance variables
        private TransferPageModel _transferPageModel;
        private decimal _amount;
        private int _accNumSender;
        private int _accNumReceiver;
        private decimal _currentBalanceSender;
        private decimal _currentBalanceReceiver;
        private string _clientFullNameSender;
        private string _clientFullNameReceiver;
        private bool _invalidateUIElementsSender;
        private bool _invalidateUIElementsReceiver;
        private bool _canExecuteFetchButtonSender;
        private bool _canExecuteFetchButtonReceiver;
        private string _personNumberSender;
        private string _personNumberReceiver;
        private bool _isValidAccountSender;
        private bool _isValidAccountReceiver;

        /// <summary>
        /// Gets FetchAccouuntInfoSenderCommand
        /// </summary>
        public ICommand FetchAccouuntInfoSenderCommand { get; private set; }

        /// <summary>
        /// Gets FetchAccouuntInfoReceiverCommand
        /// </summary>
        public ICommand FetchAccouuntInfoReceiverCommand { get; private set; }

        /// <summary>
        /// Gets TransferCommand
        /// </summary>
        public ICommand TransferCommand { get; private set; }

        /// <summary>
        /// Gets CancelCommad
        /// </summary>
        public ICommand CancelCommand { get; private set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public TransferPageViewModel()
        {
            this._transferPageModel = new TransferPageModel(); // instantiating TransferPageModel to get data
            // initializing all the instance variables and ICommands
            this._accNumSender = 0;
            this._accNumReceiver = 0;
            this._amount = 0.0m;
            this._currentBalanceSender = 0.0m;
            this._currentBalanceReceiver = 0.0m;
            this._clientFullNameSender = string.Empty;
            this._clientFullNameReceiver = string.Empty;
            this._invalidateUIElementsSender = false;
            this._invalidateUIElementsReceiver = false;
            this._canExecuteFetchButtonSender = true;
            this._canExecuteFetchButtonReceiver = false;
            this._isValidAccountSender = false;
            this._isValidAccountReceiver = false;
            this.FetchAccouuntInfoSenderCommand = new RelayCommand(this.FetchAccountInfoSender, () => { return this._canExecuteFetchButtonSender; });
            this.FetchAccouuntInfoReceiverCommand = new RelayCommand(this.FetchAccountInfoReceiver, () => { return this._canExecuteFetchButtonReceiver; });
            this.TransferCommand = new RelayCommand(this.TransferAccount, () => { return this._amount > 0.0m 
                && this._amount <= this.CurrentBalanceSender 
                && !this._canExecuteFetchButtonSender 
                && !this._canExecuteFetchButtonReceiver; });
            this.CancelCommand = new RelayCommand<Page>(this.Cancel);
        }

        /// <summary>
        /// Gets or Sets InvalidateUIElementsSender
        /// </summary>
        public bool InvalidateUIElementsSender
        {
            get => this._invalidateUIElementsSender;
            set => this.SetProperty(ref this._invalidateUIElementsSender, value);
        }

        /// <summary>
        /// Gets or Sets InvalidateUIElementsReceiver 
        /// </summary>
        public bool InvalidateUIElementsReceiver
        {
            get => this._invalidateUIElementsReceiver;
            set => this.SetProperty(ref this._invalidateUIElementsReceiver, value);
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
        /// Gets or Sets ClientFullNameSender 
        /// </summary>
        public string ClientFullNameSender
        {
            get => this._clientFullNameSender;
            set => this.SetProperty(ref this._clientFullNameSender, value);
        }

        /// <summary>
        /// Gets or Sets ClientFullNameReceiver 
        /// </summary>
        public string ClientFullNameReceiver
        {
            get => this._clientFullNameReceiver;
            set => this.SetProperty(ref this._clientFullNameReceiver, value);
        }

        /// <summary>
        /// Gets or Sets CurrentBalanceSender
        /// </summary>
        public decimal CurrentBalanceSender
        {
            get => this._currentBalanceSender;
            set => this.SetProperty(ref this._currentBalanceSender, value);
        }

        /// <summary>
        /// Gets or Sets CurrentBalanceReceiver
        /// </summary>
        public decimal CurrentBalanceReceiver
        {
            get => this._currentBalanceReceiver;
            set => this.SetProperty(ref this._currentBalanceReceiver, value);
        }

        /// <summary>
        /// Gets or Sets AccountSender
        /// </summary>
        public int AccountSender
        {
            get => this._accNumSender;
            set
            {
                this.SetProperty(ref this._accNumSender, value);
                this._transferPageModel.AccountNumber = this._accNumSender;
            }
        }

        /// <summary>
        /// Gets or Sets AccountReceiver
        /// </summary>
        public int AccountReceiver
        {
            get => this._accNumReceiver;
            set
            {
                this.SetProperty(ref this._accNumReceiver, value);
                this._transferPageModel.AccountNumber = this._accNumReceiver;
            }
        }
        
        /// <summary>
        /// Fetches sender user account information
        /// </summary>
        private void FetchAccountInfoSender()
        {
            this._isValidAccountSender = this._transferPageModel.AccountExist; // we first check if the account exists in the db or not
            if (!this._isValidAccountSender) // if not a valid account we show error and leave
            {
                CustomMessageBox.CustomMessageBox customMessageBox = new CustomMessageBox.CustomMessageBox(null, "Warning!", "Invalid sender account number!", CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_OK | CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_ICONWARNING);
                customMessageBox.ShowDialog();
                return;
            }
            // else we get data and fill the associated UI element
            this._personNumberSender = this._transferPageModel.PersonNumber;
            Tuple<string, decimal> data = this._transferPageModel.AccountNameAndAmount;
            this.ClientFullNameSender = data.Item1;
            this.CurrentBalanceSender = data.Item2;
            this.InvalidateUIElementsSender = true;
            this._canExecuteFetchButtonSender = false;
            this._canExecuteFetchButtonReceiver = true;
        }

        /// <summary>
        /// Fetches Reveiver user account information
        /// </summary>
        private void FetchAccountInfoReceiver()
        {
            if (this._accNumReceiver == 0) // if invalid Account number we leave
            {
                CustomMessageBox.CustomMessageBox customMessageBox = new CustomMessageBox.CustomMessageBox(null, "Warning!", "Invalid receiver account number!", CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_OK | CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_ICONWARNING);
                customMessageBox.ShowDialog();
                return;
            }
            this._isValidAccountReceiver = this._transferPageModel.AccountExist; // if account doesn't exist we leave
            if (!this._isValidAccountReceiver)
            {
                CustomMessageBox.CustomMessageBox customMessageBox = new CustomMessageBox.CustomMessageBox(null, "Warning!", "Invalid sender account number!", CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_OK | CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_ICONWARNING);
                customMessageBox.ShowDialog();
                return;
            }
            if (this._accNumSender == this._accNumReceiver) // we try to send to same account we leave
            {
                CustomMessageBox.CustomMessageBox customMessageBox = new CustomMessageBox.CustomMessageBox(null, "Warning", "Can't transfer money to same account!", CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_OK | CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_ICONWARNING);
                customMessageBox.ShowDialog();
                return;
            } // else we udate the UI element with associated ata
            this._personNumberReceiver = this._transferPageModel.PersonNumber;
            Tuple<string, decimal> data = this._transferPageModel.AccountNameAndAmount;
            this.ClientFullNameReceiver = data.Item1;
            this.CurrentBalanceReceiver = data.Item2;
            this.InvalidateUIElementsReceiver = true;
            this._canExecuteFetchButtonReceiver = false;
        }

        /// <summary>
        /// Performs transfer of money from one account to another
        /// </summary>
        private void TransferAccount()
        {
            decimal balanceToBeSender = this.CurrentBalanceSender - this.Amount; // substract sender's balance with amount to be transfered
            decimal balanceToBeReceiver = this.CurrentBalanceReceiver + this.Amount; // add receiver's balance with amount to be transfered

            TransferInfoStruct transferInfoStructSender = new TransferInfoStruct(); // create and fill TransferInfoStruct of sender
            transferInfoStructSender.AccNo = this.AccountSender;
            transferInfoStructSender.PersonNo = this._personNumberSender;
            transferInfoStructSender.TransactionType = TransactionType.TRANSFER_TO;
            transferInfoStructSender.TransferDate = DateTime.Now.ToString("yyyy-MM-dd");
            transferInfoStructSender.TransferAmout = this.Amount;
            transferInfoStructSender.TransferBalanceAfter = this._currentBalanceSender;

            TransferInfoStruct transferInfoStructReceiver = new TransferInfoStruct(); // create and fill TransferInfoStruct of receiver
            transferInfoStructReceiver.AccNo = this.AccountReceiver;
            transferInfoStructReceiver.PersonNo = this._personNumberReceiver;
            transferInfoStructReceiver.TransactionType = TransactionType.TRANSFER_FROM;
            transferInfoStructReceiver.TransferDate = DateTime.Now.ToString("yyyy-MM-dd");
            transferInfoStructReceiver.TransferAmout = this.Amount;
            transferInfoStructReceiver.TransferBalanceAfter = this._currentBalanceReceiver;

            if (!this._transferPageModel.UpdateBalanceToAccount(this.AccountSender, balanceToBeSender) 
                || !this._transferPageModel.UpdateBalanceToAccount(this.AccountReceiver, balanceToBeReceiver)
                || !this._transferPageModel.RegisterNewTransaction(transferInfoStructSender)
                || !this._transferPageModel.RegisterNewTransaction(transferInfoStructReceiver)) // if update and registration of both sender and receiver TransferInfoStruct fails then we show error message and return
            {
                CustomMessageBox.CustomMessageBox customMessageBox = new CustomMessageBox.CustomMessageBox(null, "Error!", "Couldn't perform transfer operation!", CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_OK | CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_ICONERROR);
                customMessageBox.ShowDialog();
                return;
            }

            this.CurrentBalanceSender = balanceToBeSender; // update current balance UI element of sender
            this.CurrentBalanceReceiver = balanceToBeReceiver; // update current balance UI element of receiver
        }

        /// <summary>
        /// Resets the currrent Page viewModel
        /// </summary>
        /// <param name="page"></param>
        private void Cancel(Page page)
        {
            this.ResetPage(page, new TransferPageViewModel());
        }
    }
}
