/*
    Dipayan Sarker
    February 10, 2020
*/

using System;

namespace BankManager
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            BankManager.App app = new BankManager.App();
            app.InitializeComponent();
            app.Run();
        }
    }
}
