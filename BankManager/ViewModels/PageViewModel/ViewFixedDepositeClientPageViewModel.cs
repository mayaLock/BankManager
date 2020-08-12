/*
    Dipayan Sarker
    February 10, 2020
*/

using BankManager.Helpers;
using BankManager.Models.PageModels;
using EasyMVVM.ViewModels;
using EasyMVVM.Commands;
using System.Data;
using System.Windows.Controls;
using System.Windows.Input;

namespace BankManager.ViewModels.PageViewModel
{
    /// <summary>
    /// ViewModel for ViewFixedDepositeClientPage
    /// </summary>
    public class ViewFixedDepositeClientPageViewModel : ViewModelBase
    {
        // private instance variable
        private ViewFixedDepositeClientPageModel _viewFixedDepositeClientPageModel;
        private bool _isGroupBoxVisible;
        private DataView _dataView;
        private bool _canExecuteFetchButton;

        /// <summary>
        /// Gets FetchCommand
        /// </summary>
        public ICommand FetchCommand { get; private set; }

        /// <summary>
        /// Gets CancelCommand
        /// </summary>
        public ICommand CancelCommand { get; private set; }

        /// <summary>
        /// Gets or Sets IsGroupBoxVisible
        /// </summary>
        public bool IsGroupBoxVisible
        {
            get => this._isGroupBoxVisible;
            set => this.SetProperty(ref this._isGroupBoxVisible, value);
        }

        /// <summary>
        /// Gets or Sets DataViewData
        /// </summary>
        public DataView DataViewData
        {
            get => this._dataView;
            set => this.SetProperty(ref this._dataView, value);
        }

        /// <summary>
        /// Fetches all fixed account details and fills the associated UI element with them
        /// </summary>
        private void FetchViewAllFixedClient()
        {
            this.DataViewData = this._viewFixedDepositeClientPageModel.ALLFixedAccountDataTable.DefaultView;
            this.IsGroupBoxVisible = true;
            this._canExecuteFetchButton = false;
        }

        /// <summary>
        /// Resets the currrent Page viewModel
        /// </summary>
        /// <param name="page"></param>
        private void Cancel(Page page)
        {
            this.ResetPage(page, new ViewFixedDepositeClientPageViewModel());
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ViewFixedDepositeClientPageViewModel()
        {
            this._viewFixedDepositeClientPageModel = new ViewFixedDepositeClientPageModel(); // instantiate the new ViewFixedDepositeClientPageModel
            // initializing all the instance variables and ICommands
            this._canExecuteFetchButton = true;
            this._dataView = new DataView();
            this._isGroupBoxVisible = false;
            this.FetchCommand = new RelayCommand(this.FetchViewAllFixedClient, () => { return this._canExecuteFetchButton; });
            this.CancelCommand = new RelayCommand<Page>(this.Cancel);
        }
    }
}
