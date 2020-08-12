/*
    Dipayan Sarker
    February 10, 2020
*/

using BankManager.Models;
using EasyMVVM.ViewModels;
using EasyMVVM.Commands;
using System.Windows;
using System.Windows.Input;

namespace BankManager.ViewModels
{
    /// <summary>
    /// ViewModel for AboutWindow
    /// </summary>
    public class AboutWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// Gets assemby version number
        /// </summary>
        public string VersionNumber { get => this._aboutWindowModel.AssemblyVersion; }

        /// <summary>
        /// Gets WindowCloseCommand
        /// </summary>
        public ICommand WindowCloseCommand { get; private set; }

        // private instance variable
        private readonly AboutWindowModel _aboutWindowModel;

        /// <summary>
        /// Default constructor
        /// </summary>
        public AboutWindowViewModel()
        {           
            this._aboutWindowModel = new AboutWindowModel(); // instantiate AboutWindowModel
            this.WindowCloseCommand = new RelayCommand<Window>(CloseWindow); // initialze the ICommand
        }

        /// <summary>
        /// Closes the about window and sets dialog result to true
        /// </summary>
        /// <param name="window"></param>
        private void CloseWindow(Window window)
        {
            window.DialogResult = true;
            window.Close();
        }
    }
}
