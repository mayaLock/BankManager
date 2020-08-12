/*
    Dipayan Sarker
    February 10, 2020
*/

using BankManager.ViewModels;
using System.Windows;

namespace BankManager.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            this.DataContext = new LoginWindowVeiwModel(); // setting data context to its view model
        }
    }
}
