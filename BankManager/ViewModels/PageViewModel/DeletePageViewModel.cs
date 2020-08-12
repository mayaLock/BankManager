/*
    Dipayan Sarker
    February 10, 2020
*/

using BankManager.Helpers;
using BankManager.Models.PageModels;
using EasyMVVM.ViewModels;
using EasyMVVM.Commands;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace BankManager.ViewModels.PageViewModel
{
    /// <summary>
    /// VeiwModel for DeletePage
    /// </summary>
    public class DeletePageViewModel : ViewModelBase
    {
        // private instance variables
        private DeletePageModel _deletePageModel;
        private int _accNum;
        private string _clientFullName;
        private decimal _clientCurrentBalance;
        private bool _invalidateUIElements;
        private bool _canExecuteFetchButton;
        private bool _canExecuteDeleteButton;

        /// <summary>
        /// Gets FetchAccouuntInfoCommand
        /// </summary>
        public ICommand FetchAccouuntInfoCommand { get; private set; }

        /// <summary>
        /// Gets DeleteCommand
        /// </summary>
        public ICommand DeleteCommand { get; private set; }

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
                this._deletePageModel.AccountNumber = this._accNum;
            }
        }

        /// <summary>
        /// Gets IsValidAccount
        /// </summary>
        public bool IsValidAccount { get => this._deletePageModel.AccountExist; }

        /// <summary>
        /// Gets user account information
        /// </summary>
        private void FetchAccouuntInfo()
        {
            if (!this.IsValidAccount) // if invalid account we return
            {
                CustomMessageBox.CustomMessageBox customMessageBox = new CustomMessageBox.CustomMessageBox(null, "Warning!", "Invalid account number!", CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_OK | CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_ICONWARNING);
                customMessageBox.ShowDialog();
                return;
            } // else we update the UI
            Tuple<string, decimal> data = this._deletePageModel.AccountNameAndAmount;
            this.ClientFullName = data.Item1;
            this.ClientCurrentBalance = data.Item2;
            this.InvalidateUIElements = true;
            this._canExecuteFetchButton = false;
            this._canExecuteDeleteButton = true;
        }

        /// <summary>
        /// Deletes an account from the db
        /// </summary>
        /// <param name="page"></param>
        private void Delete(Page page)
        {
            if (!this._deletePageModel.DeleteClientAccount()) // if delete fails then we show error message
            {
                CustomMessageBox.CustomMessageBox customMessageBox = new CustomMessageBox.CustomMessageBox(null, "Error!", "Couldn't perform deletion operation!", CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_OK | CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_ICONERROR);
                customMessageBox.ShowDialog();
                return;
            }
            // else we show the success message and reset the Page to initial state
            this.ResetPage(page, new DeletePageViewModel());
            CustomMessageBox.CustomMessageBox customMessageBox1 = new CustomMessageBox.CustomMessageBox(null, "Info!", "Done deleting account!", CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_OK | CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_ICONINFORMATION);
            customMessageBox1.ShowDialog();
            return;
        }

        /// <summary>
        /// Resets the page to initial state
        /// </summary>
        /// <param name="page"></param>
        private void Cancel(Page page)
        {
            this.ResetPage(page, new DeletePageViewModel());
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public DeletePageViewModel()
        {
            this._deletePageModel = new DeletePageModel();
            // initializing all the instance variables and ICommands
            this._accNum = 0;
            this._clientFullName = string.Empty;
            this._clientCurrentBalance = 0.0m;
            this._invalidateUIElements = false;
            this._canExecuteFetchButton = true;
            this._canExecuteDeleteButton = false;
            this.FetchAccouuntInfoCommand = new RelayCommand(this.FetchAccouuntInfo, () => { return this._canExecuteFetchButton; });
            this.DeleteCommand = new RelayCommand<Page>(this.Delete, (p) => { return this._canExecuteDeleteButton; });
            this.CancelCommand = new RelayCommand<Page>(this.Cancel);
        }
    }
}
