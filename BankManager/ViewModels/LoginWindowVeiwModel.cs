/*
    Dipayan Sarker
    February 10, 2020
*/

using BankManager.Models;
using EasyMVVM.ViewModels;
using EasyMVVM.Commands;
using BankManager.Views;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace BankManager.ViewModels
{
    /// <summary>
    /// ViewModel for LoginWindow
    /// </summary>
    public class LoginWindowVeiwModel : ViewModelBase
    {
        // private instance variable
        private LoginWindowModel _loginWindowModel;
        private string _username;
        private string _password;

        /// <summary>
        /// Gets DragMoveCommand
        /// </summary>
        public ICommand DragMoveCommand { get; private set; }

        /// <summary>
        /// Gets WindowCloseCommand
        /// </summary>
        public ICommand WindowCloseCommand { get; private set; }

        /// <summary>
        /// Gets ResetPasswordBoxCommand
        /// </summary>
        public ICommand ResetPasswordBoxCommand { get; private set; }

        /// <summary>
        /// Gets ButtonSubmitCommand
        /// </summary>
        public ICommand ButtonSubmitCommand { get; private set; }        
        
        /// <summary>
        /// Gets or Sets Username
        /// </summary>
        public string Username 
        {
            get => this._username;
            set
            {
                this.SetProperty(ref this._username, value); // sends INotificationPropertyChanged signal
                this._loginWindowModel.UserName = this._username;
            }
        }

        /// <summary>
        /// Gets or Sets Password
        /// </summary>
        public string Password
        {
            get => this._password;
            set
            {
                this.SetProperty(ref this._password, value);
                this._loginWindowModel.Password = this._password;
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public LoginWindowVeiwModel()
        {
            // initializing all instance variables and properties
            this._loginWindowModel = new LoginWindowModel();
            this.DragMoveCommand = new RelayCommand<Window>(this.DragMoveWindow);
            this.WindowCloseCommand = new RelayCommand<Window>(this.CloseWindow);
            this.ResetPasswordBoxCommand = new RelayCommand<object>(this.SetTempTextVisibility);
            this.ButtonSubmitCommand = new RelayCommand<PasswordBox>(this.VarifyLogin);
        }

        /// <summary>
        /// Moves the window by dragging the left mouse button
        /// </summary>
        /// <param name="window"></param>
        public void DragMoveWindow(Window window)
        {
            window.DragMove();
        }

        /// <summary>
        /// Closes the window
        /// </summary>
        /// <param name="window"></param>
        public void CloseWindow(Window window)
        {
            window.Close();
        }
        
        /// <summary>
        /// Sets the visibility of the temp text strings for username and password textboxes
        /// </summary>
        /// <param name="parameter"></param>
        private void SetTempTextVisibility(object parameter)
        {
            object[] values = (object[])parameter; // gets object array from object
            PasswordBox passwordBox = (PasswordBox)values[0]; // casts PasswordBox from array element 0
            TextBlock textBlock = (TextBlock)values[1]; // casts TextBlock from array element 1
            if (passwordBox.Password.Length > 0) // if password length is more than 0 we collapse the visibility of temp text string
            {
                textBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                textBlock.Visibility = Visibility.Visible; // else we make it visible
            }
        }

        /// <summary>
        /// Verifies the user login activity
        /// </summary>
        /// <param name="passwordBox"></param>
        private void VarifyLogin(PasswordBox passwordBox)
        {
            this.Password = passwordBox.Password; // can't directly bind with PasswordBox            
            if (this._loginWindowModel.IsSuccessfulLogin) // if login is successful
            {
                AppMainWindow appMainWindow = new AppMainWindow(); // we create the new AppMainWindow
                appMainWindow.DataContext = new AppMainWindowViewModel() { CurrentUser = this.Username }; // then set it's data context and properties
                appMainWindow.Show(); // we show the window to the user
                Window.GetWindow(passwordBox).Close(); // close the LoginWindow                
            }
            else // we show error message and reset the value of the textboxes
            {
                CustomMessageBox.CustomMessageBox customMessageBox = new CustomMessageBox.CustomMessageBox(null, "Stop!", "Invalid username or password", CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_OK | CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_ICONSTOP);
                customMessageBox.ShowDialog();
                this.Username = string.Empty;
                passwordBox.Password = string.Empty;
                return;
            }
        }
    }
}
