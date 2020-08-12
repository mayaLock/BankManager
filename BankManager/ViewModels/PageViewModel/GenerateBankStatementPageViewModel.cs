/*
    Dipayan Sarker
    February 10, 2020
*/

using BankManager.Helpers;
using BankManager.Models.PageModels;
using EasyMVVM.ViewModels;
using EasyMVVM.Commands;
using IronPdf;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace BankManager.ViewModels.PageViewModel
{
    /// <summary>
    /// ViewModel for GenerateBankStatementPage
    /// </summary>
    public class GenerateBankStatementPageViewModel : ViewModelBase
    {
        // private instance variables
        private GenerateBankStatementPageModel _generateBankStatementPageModel;
        private int _accNum;
        private string _clientFullName;
        private decimal _clientCurrentBalance;
        private bool _invalidateUIElements;
        private bool _canExecuteFetchButton;
        private bool _canExecuteGenerateButton;

        /// <summary>
        /// Gets FetchAccouuntInfoCommand
        /// </summary>
        public ICommand FetchAccouuntInfoCommand { get; private set; }

        /// <summary>
        /// Gets GenerateCommand
        /// </summary>
        public ICommand GenerateCommand { get; private set; }

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
                this._generateBankStatementPageModel.AccountNumber = this._accNum;
            }
        }

        /// <summary>
        /// Gets IsValidAccount
        /// </summary>
        public bool IsValidAccount { get => this._generateBankStatementPageModel.AccountExist; }

        /// <summary>
        /// Fetches account information of the User
        /// </summary>
        private void FetchAccouuntInfo()
        {
            if (!this.IsValidAccount) // if invalid account we show error and leave
            {
                CustomMessageBox.CustomMessageBox customMessageBox = new CustomMessageBox.CustomMessageBox(null, "Warning!", "Invalid account number!", CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_OK | CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_ICONWARNING);
                customMessageBox.ShowDialog();
                return;
            } // else we fill the UI elements with data
            Tuple<string, decimal> data = this._generateBankStatementPageModel.AccountNameAndAmount;
            this.ClientFullName = data.Item1;
            this.ClientCurrentBalance = data.Item2;
            this.InvalidateUIElements = true;
            this._canExecuteFetchButton = false;
            this._canExecuteGenerateButton = true;
        }

        /// <summary>
        /// Generates PDF file about all the transfer details for the current account
        /// </summary>
        private void Generate()
        {
            Tuple<int, string, string> data = this._generateBankStatementPageModel.TranstationCountAndPdfFileNameAndPdfContentString; // get data for pdf printing
            if (data.Item1 < 1) // if no transtaction then we leave
            {
                CustomMessageBox.CustomMessageBox customMessageBox = new CustomMessageBox.CustomMessageBox(null, "Info!", "No transaction found!", CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_OK | CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_ICONINFORMATION);
                customMessageBox.ShowDialog();
                return;
            } // else
            HtmlToPdf renderer = new HtmlToPdf(); // we create an instance of HtmlPdf, then try to save pdf file and on sucess or failure we show message to the user
            try
            {
                renderer.RenderHtmlAsPdf(data.Item3).SaveAs(data.Item2);
                CustomMessageBox.CustomMessageBox customMessageBox = new CustomMessageBox.CustomMessageBox(null, "info!", $"{data.Item2} has been saved!", CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_OK | CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_ICONINFORMATION);
                customMessageBox.ShowDialog();
                this._canExecuteGenerateButton = false;
                return;
            }
            catch (Exception)
            {
                CustomMessageBox.CustomMessageBox customMessageBox = new CustomMessageBox.CustomMessageBox(null, "Error!", "Couldn't save file!", CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_OK | CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_ICONERROR);
                customMessageBox.ShowDialog();
                this._canExecuteGenerateButton = false;
                return;
            }

        }

        /// <summary>
        /// Resets the currrent Page viewModel
        /// </summary>
        /// <param name="page"></param>
        private void Cancel(Page page)
        {
            this.ResetPage(page, new GenerateBankStatementPageViewModel());
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public GenerateBankStatementPageViewModel()
        {
            this._generateBankStatementPageModel = new GenerateBankStatementPageModel(); // instantiate GenerateBankStatementPageModel to get data
            // initializing all the instance variables and ICommands
            this._accNum = 0;
            this._clientFullName = string.Empty;
            this._clientCurrentBalance = 0.0m;
            this._invalidateUIElements = false;
            this._canExecuteFetchButton = true;
            this._canExecuteGenerateButton = false;
            this.FetchAccouuntInfoCommand = new RelayCommand(this.FetchAccouuntInfo, () => { return this._canExecuteFetchButton; });
            this.GenerateCommand = new RelayCommand(this.Generate, () => { return this._canExecuteGenerateButton; });
            this.CancelCommand = new RelayCommand<Page>(this.Cancel);
        }
    }
}
