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
    /// ViewModel for ViewAllClientPage
    /// </summary>
    public class ViewAllClientPageViewModel : ViewModelBase
    {
        private ViewAllClientPageModel _viewAllClientPageModel;
        private bool _isGroupBoxVisible;
        private DataView _dataView;
        private bool _canExecuteFetchButton;

        /// <summary>
        /// Gets FetchCommand
        /// </summary>
        public ICommand FetchCommand { get; private set; }

        /// <summary>
        /// GetsCancelCommand
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
        /// Fetches all account details and fills the associated UI element with them
        /// </summary>
        private void FetchViewAllClient()
        {
            this.DataViewData = this._viewAllClientPageModel.AllAccountDataTable.DefaultView;
            this.IsGroupBoxVisible = true;
            this._canExecuteFetchButton = false;
        }

        /// <summary>
        /// Resets the currrent Page viewModel
        /// </summary>
        /// <param name="page"></param>
        private void Cancel(Page page)
        {
            this.ResetPage(page, new ViewAllClientPageViewModel());
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ViewAllClientPageViewModel()
        {
            this._viewAllClientPageModel = new ViewAllClientPageModel(); // instanctiating ViewAllClientPage
            // initializing all the instance variables and ICommands
            this._canExecuteFetchButton = true;
            this._dataView = new DataView();
            this._isGroupBoxVisible = false;
            this.FetchCommand = new RelayCommand(this.FetchViewAllClient, () => { return this._canExecuteFetchButton; });
            this.CancelCommand = new RelayCommand<Page>(this.Cancel);
        }
    }
}
