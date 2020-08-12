/*
    Dipayan Sarker
    February 10, 2020
*/

using BankManager.Helpers;
using EasyMVVM.ViewModels;
using EasyMVVM.Commands;
using BankManager.ViewModels.PageViewModel;
using BankManager.Views;
using BankManager.Views.Pages;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BankManager.ViewModels
{
    /// <summary>
    /// ViewModel for AppMainWinodwViewModel
    /// </summary>
    public class AppMainWindowViewModel : ViewModelBase
    {
        // private instance variable 
        private Page _currentPage; // current Page that is displayed by the main window

        /// <summary>
        /// Gets or Sets CurrentUser
        /// </summary>
        public string CurrentUser { get; set; }

        /// <summary>
        /// Gets or Sets CurrentPage
        /// </summary>
        public Page CurrentPage
        {
            get => this._currentPage;
            set => this.SetProperty(ref this._currentPage, value);            
        }

        /// <summary>
        /// Gets WindowCloseCommand
        /// </summary>
        public ICommand WindowCloseCommand { get; private set; }

        /// <summary>
        /// Gets CreateAboutWindowCommand
        /// </summary>
        public ICommand CreateAboutWindowCommand { get; private set; }

        /// <summary>
        /// Gets CreateDipostePageCommand 
        /// </summary>
        public ICommand CreateDipostePageCommand { get; private set; }

        /// <summary>
        /// Gets CreateWithdrawPageCommand
        /// </summary>
        public ICommand CreateWithdrawPageCommand { get; private set; }

        /// <summary>
        /// Gets CreateTransferPageCommand
        /// </summary>
        public ICommand CreateTransferPageCommand { get; private set; }

        /// <summary>
        /// Gets CreateFixedDepositePageCommand
        /// </summary>
        public ICommand CreateFixedDepositePageCommand { get; private set; }

        /// <summary>
        /// Gets CreateGenerateBankStatementPageCommand
        /// </summary>
        public ICommand CreateGenerateBankStatementPageCommand { get; private set; }

        /// <summary>
        /// Gets CreateViewAllClientPageCommand
        /// </summary>
        public ICommand CreateViewAllClientPageCommand { get; private set; }

        /// <summary>
        /// Gets CreateViewFixedDepositeClientPageCommand
        /// </summary>
        public ICommand CreateViewFixedDepositeClientPageCommand { get; private set; }

        /// <summary>
        /// Gets CreateCreateNewAccountPageCommand
        /// </summary>
        public ICommand CreateCreateNewAccountPageCommand { get; private set; }

        /// <summary>
        /// Gets CreateUpdatePageCommand
        /// </summary>
        public ICommand CreateUpdatePageCommand { get; private set; }

        /// <summary>
        /// Gets CreateDeletePageCommand
        /// </summary>
        public ICommand CreateDeletePageCommand { get; private set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public AppMainWindowViewModel()
        {
            // initialze all variables and ICommands
            this._currentPage = null;
            this.WindowCloseCommand = new RelayCommand<Window>(this.CloseWindow);
            this.CreateAboutWindowCommand = new RelayCommand<Window>(this.CreateAboutWindow);
            this.CreateDipostePageCommand = new RelayCommand(this.CreateDipositePage);
            this.CreateWithdrawPageCommand = new RelayCommand(this.CreateWithdrawPage);
            this.CreateTransferPageCommand = new RelayCommand(this.CreateTransferPage);
            this.CreateFixedDepositePageCommand = new RelayCommand(this.CreateFixedDepositePage);
            this.CreateGenerateBankStatementPageCommand = new RelayCommand(this.CreateGenerateBankStatementPage);
            this.CreateViewAllClientPageCommand = new RelayCommand(this.CreateViewAllClientPage);
            this.CreateViewFixedDepositeClientPageCommand = new RelayCommand(this.CreateViewFixedDepositeClientPage);
            this.CreateCreateNewAccountPageCommand = new RelayCommand(this.CreateCreateNewAccountPage);
            this.CreateUpdatePageCommand = new RelayCommand(this.CreateUpdatePage);
            this.CreateDeletePageCommand = new RelayCommand(this.CreateDeletePage);
        }

        /// <summary>
        /// A constructor with one value
        /// </summary>
        /// <param name="window"></param>
        public void CreateAboutWindow(Window window)
        {
            AboutWindow aboutWindow = new AboutWindow() { Owner = window }; // instantiate About window and set its owner property to center it on the screen
            aboutWindow.DataContext = new AboutWindowViewModel(); // set its data context
            aboutWindow.ShowDialog();
        }

        /// <summary>
        /// Creates new DipositePage
        /// </summary>
        private void CreateDipositePage()
        {
            DipositePage dipositePage = new DipositePage(); // instantiate the DipositePage
            dipositePage.DataContext = new DepositePageViewModel(); // instantiate its data context
            this.RemovePreviousPage(this.CurrentPage); // remove previous page, if any
            this.CurrentPage = dipositePage; // set the diposite page as the dsiplay page of the window
        }

        /// <summary>
        /// Creates new WithdrawPage
        /// </summary>
        private void CreateWithdrawPage()
        {
            // similar as above
            WithdrawPage withdrawPage = new WithdrawPage();
            withdrawPage.DataContext = new WithdrawPageViewModel();
            this.RemovePreviousPage(this.CurrentPage);
            this.CurrentPage = withdrawPage;
        }

        /// <summary>
        /// Creates new TransferPage
        /// </summary>
        private void CreateTransferPage()
        {
            // similar as above
            TransferPage transferPage = new TransferPage();
            transferPage.DataContext = new TransferPageViewModel();
            this.RemovePreviousPage(this.CurrentPage);
            this.CurrentPage = transferPage;
        }

        /// <summary>
        /// Creates new FixedDepositePage
        /// </summary>
        private void CreateFixedDepositePage()
        {
            // similar as above
            FixedDepositePage fixedDepositePage = new FixedDepositePage();
            fixedDepositePage.DataContext = new FixedDepositePageViewModel();
            this.RemovePreviousPage(this.CurrentPage);
            this.CurrentPage = fixedDepositePage;
        }

        /// <summary>
        /// Creates new GenerateBankStatementPage
        /// </summary>
        private void CreateGenerateBankStatementPage()
        {
            // similar as above
            GenerateBankStatementPage generateBankStatementPage = new GenerateBankStatementPage();
            generateBankStatementPage.DataContext = new GenerateBankStatementPageViewModel();
            this.RemovePreviousPage(this.CurrentPage);
            this.CurrentPage = generateBankStatementPage;
        }

        /// <summary>
        /// Creates new ViewAllClientPage
        /// </summary>
        private void CreateViewAllClientPage()
        {
            // similar as above
            ViewAllClientPage viewAllClientPage = new ViewAllClientPage();
            viewAllClientPage.DataContext = new ViewAllClientPageViewModel();
            this.RemovePreviousPage(this.CurrentPage);
            this.CurrentPage = viewAllClientPage;
        }

        /// <summary>
        /// Creates new ViewFixedDepositeClientPage
        /// </summary>
        private void CreateViewFixedDepositeClientPage()
        {
            // similar as above
            ViewFixedDepositeClientPage viewFixedDepositeClientPage = new ViewFixedDepositeClientPage();
            viewFixedDepositeClientPage.DataContext = new ViewFixedDepositeClientPageViewModel();
            this.RemovePreviousPage(this.CurrentPage);
            this.CurrentPage = viewFixedDepositeClientPage;
        }

        /// <summary>
        /// Creates new CreateNewAccountPage
        /// </summary>
        private void CreateCreateNewAccountPage()
        {
            // similar as above
            CreateNewAccountPage createNewAccountPage = new CreateNewAccountPage();
            createNewAccountPage.DataContext = new CreateNewAccountPageViewModel();
            this.RemovePreviousPage(this.CurrentPage);
            this.CurrentPage = createNewAccountPage;
        }

        /// <summary>
        /// Creates new UpdatePage
        /// </summary>
        private void CreateUpdatePage()
        {
            // similar as above
            UpdatePage updatePage = new UpdatePage();
            updatePage.DataContext = new UpdatePageViewModel();
            this.RemovePreviousPage(this.CurrentPage);
            this.CurrentPage = updatePage;
        }

        /// <summary>
        /// Creates new DeletePage
        /// </summary>
        private void CreateDeletePage()
        {
            // similar as above
            DeletePage deletePage = new DeletePage();
            deletePage.DataContext = new DeletePageViewModel();
            this.RemovePreviousPage(this.CurrentPage);
            this.CurrentPage = deletePage;
        }

        /// <summary>
        /// Closes the window
        /// </summary>
        /// <param name="window"></param>
        private void CloseWindow(Window window)
        {
            window.Close();
        }
    }
}
