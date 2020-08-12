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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace BankManager.ViewModels.PageViewModel
{
    /// <summary>
    /// ViewModel for CreateNewAccountPage
    /// </summary>
    public class CreateNewAccountPageViewModel : ViewModelBase
    {
        // private instance variables
        private CreateNewAccountPageModel _createNewAccountPageModel;
        private string _accNumString;
        private string _fName;
        private string _lName;
        private DateTime _dObPicker;
        private string _personNoSecret;
        private string _placeOfBirth;
        private string _phoneNo;
        private string _sex;
        private string _citizenship;
        private string _address;
        private string _civilStatus;
        private string _email;
        private List<string> _allCountryList;
        private int _selectedIndexAllCountryCombobox;
        private int _selectedIndexSex;
        private int _selectedIndexCivilStatus;

        /// <summary>
        /// Gets CreateCommand
        /// </summary>
        public ICommand CreateCommand { get; private set; }

        /// <summary>
        /// Gets CancelCommand
        /// </summary>
        public ICommand CancelCommand { get; private set; }

        /// <summary>
        /// Gets or Sets AllCountryList
        /// </summary>
        public List<string> AllCountryList
        {
            get => this._allCountryList;
            set => this.SetProperty(ref this._allCountryList, value);
        }

        /// <summary>
        /// Gets or Sets SelectedIndexCivilStatus
        /// </summary>
        public int SelectedIndexCivilStatus
        {
            get => this._selectedIndexCivilStatus;
            set => this.SetProperty(ref this._selectedIndexCivilStatus, value);
        }

        /// <summary>
        /// Gets or Sets SelectedIndexSex
        /// </summary>
        public int SelectedIndexSex
        {
            get => this._selectedIndexSex;
            set => this.SetProperty(ref this._selectedIndexSex, value);
        }

        /// <summary>
        /// Gets or Sets SelectedIndexAllCountryCombobox
        /// </summary>
        public int SelectedIndexAllCountryCombobox
        {
            get => this._selectedIndexAllCountryCombobox;
            set => this.SetProperty(ref this._selectedIndexAllCountryCombobox, value);
        }

        /// <summary>
        /// Gets or Sets AccountNumberString 
        /// </summary>
        public string AccountNumberString
        {
            get => this._accNumString;
            set => this.SetProperty(ref this._accNumString, value);
        }

        /// <summary>
        /// Gets or Sets FirstName
        /// </summary>
        public string FirstName
        {
            get => this._fName;
            set => this.SetProperty(ref this._fName, value);
        }

        /// <summary>
        /// Gets or Sets LastName
        /// </summary>
        public string LastName
        {
            get => this._lName;
            set => this.SetProperty(ref this._lName, value);
        }

        /// <summary>
        /// Gets or Sets DoBPicker
        /// </summary>
        public DateTime DoBPicker
        {
            get => this._dObPicker;
            set => this.SetProperty(ref this._dObPicker, value);
        }

        /// <summary>
        /// Gets or Sets PersonNomberSecret 
        /// </summary>
        public string PersonNomberSecret
        {
            get => this._personNoSecret;
            set => this.SetProperty(ref this._personNoSecret, value);
        }

        /// <summary>
        /// Gets or Sets PlaceOfBirth
        /// </summary>
        public string PlaceOfBirth
        {
            get => this._placeOfBirth;
            set => this.SetProperty(ref this._placeOfBirth, value);
        }

        /// <summary>
        /// Gets or Sets PhoneNumber
        /// </summary>
        public string PhoneNumber
        {
            get => this._phoneNo;
            set => this.SetProperty(ref this._phoneNo, value);
        }

        /// <summary>
        /// Gets or Sets Sex
        /// </summary>
        public string Sex
        {
            get => this._sex;
            set => this.SetProperty(ref this._sex, value);
        }

        /// <summary>
        /// Gets or Sets Citizenship
        /// </summary>
        public string Citizenship
        {
            get => this._citizenship;
            set => this.SetProperty(ref this._citizenship, value);
        }

        /// <summary>
        /// Gets or Sets Address
        /// </summary>
        public string Address
        {
            get => this._address;
            set => this.SetProperty(ref this._address, value);
        }

        /// <summary>
        /// Gets or Sets CivilStatus
        /// </summary>
        public string CivilStatus
        {
            get => this._civilStatus;
            set => this.SetProperty(ref this._civilStatus, value);
        }

        /// <summary>
        /// Gets or Sets Email
        /// </summary>
        public string Email
        {
            get => this._email;
            set => this.SetProperty(ref this._email, value);
        }

        /// <summary>
        /// Resets the current page to initial state
        /// </summary>
        /// <param name="page"></param>
        private void Cancel(Page page)
        {
            this.ResetPage(page, new CreateNewAccountPageViewModel());
        }

        /// <summary>
        /// Creaets a new account
        /// </summary>
        /// <param name="page"></param>
        private void Create(Page page)
        {
            if (!this._personNoSecret.All(char.IsDigit) || !this._phoneNo.All(char.IsDigit)) // if invalid phone or person number secret code we show error and return
            {
                CustomMessageBox.CustomMessageBox customMessageBox = new CustomMessageBox.CustomMessageBox(null, "Warning!", "Only digits are allowed!", CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_OK | CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_ICONWARNING);
                customMessageBox.ShowDialog();
                return;
            }

            if (this._personNoSecret.Length < 4) // if length of personnumber secret code is less than 4 we return
            {
                CustomMessageBox.CustomMessageBox customMessageBox = new CustomMessageBox.CustomMessageBox(null, "Warning!", "Secret number can have only four digits!", CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_OK | CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_ICONWARNING);
                customMessageBox.ShowDialog();
                return;
            }

            if (!new EmailAddressAttribute().IsValid(this._email)) // if invalid email we return
            {
                CustomMessageBox.CustomMessageBox customMessageBox = new CustomMessageBox.CustomMessageBox(null, "Warning!", "Invalid email format!", CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_OK | CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_ICONWARNING);
                customMessageBox.ShowDialog();
                return;
            }

            string personNo = $"{this._dObPicker.ToString("yyyyMMdd")}-{this._personNoSecret}";
            this._createNewAccountPageModel.PersonNumberForCheckExist = personNo;

            if (this._createNewAccountPageModel.PersonNumberExist) // if person number already exists we return
            {
                CustomMessageBox.CustomMessageBox customMessageBox = new CustomMessageBox.CustomMessageBox(null, "Warning!", "This person number already has a account!", CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_OK | CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_ICONWARNING);
                customMessageBox.ShowDialog();
                return;
            }

            if (!this._dObPicker.IsSixteenYearsOld()) // if account holder is younger than 16 we returb
            {
                CustomMessageBox.CustomMessageBox customMessageBox = new CustomMessageBox.CustomMessageBox(null, "Warning!", "Account holder must be at least sixteen years old!", CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_OK | CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_ICONWARNING);
                customMessageBox.ShowDialog();
                return;
            }

            BankDataStruct bankDataStruct = new BankDataStruct(); // we create a new BackData struct and we fill all the bank info data
            bankDataStruct.AccNo = int.Parse(this._accNumString); // since value is not gonna change so no need TryParse()
            bankDataStruct.FName = this._fName;
            bankDataStruct.LName = this._lName;
            bankDataStruct.DOB = this._dObPicker.ToString("yyyyMMdd");
            bankDataStruct.PersonNo = personNo;
            bankDataStruct.PlaceOfBirth = this._placeOfBirth;
            bankDataStruct.PhoneNo = this._phoneNo;
            bankDataStruct.Sex = this._sex;
            bankDataStruct.Citizenship = this._citizenship;
            bankDataStruct.Address = this._address;
            bankDataStruct.CivilStatus = this._civilStatus;
            bankDataStruct.Amount = 0.0m;
            bankDataStruct.HasFixedAccount = false;
            bankDataStruct.Email = this._email;
            bankDataStruct.AccCreationDate = DateTime.Now.ToString("yyyy-MM-dd");

            if (!this._createNewAccountPageModel.CreateBankAccount(bankDataStruct)) // if account is not created we show error and return
            {
                CustomMessageBox.CustomMessageBox customMessageBox = new CustomMessageBox.CustomMessageBox(null, "Error!", "Couldn't create new Account!", CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_OK | CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_ICONERROR);
                customMessageBox.ShowDialog();
                return;
            } // else we reset the page and show success message
            this.ResetPage(page, new CreateNewAccountPageViewModel());
            CustomMessageBox.CustomMessageBox customMessageBox1 = new CustomMessageBox.CustomMessageBox(null, "Info!", "Done creating new account!", CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_OK | CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_ICONINFORMATION);
            customMessageBox1.ShowDialog();
            return;
        }

        /// <summary>
        /// Default constroctor
        /// </summary>
        public CreateNewAccountPageViewModel()
        {
            this._createNewAccountPageModel = new CreateNewAccountPageModel();
            // initializing all the instance variables and ICommands
            this._accNumString = this._createNewAccountPageModel.NewAccountNumberString;
            this._fName = string.Empty;
            this._lName = string.Empty;
            this._dObPicker = DateTime.Now;
            this._personNoSecret = string.Empty; ;
            this._placeOfBirth = string.Empty;
            this._phoneNo = string.Empty;
            this._sex = string.Empty;
            this._citizenship = string.Empty;
            this._address = string.Empty;
            this._civilStatus = string.Empty;
            this._email = string.Empty;
            this._selectedIndexAllCountryCombobox = -1;
            this._selectedIndexSex = -1;
            this._selectedIndexCivilStatus = -1;
            this._allCountryList = HelperExtensionCore.GetAllCountryNames();
            this.CreateCommand = new RelayCommand<Page>(this.Create, (p) => { return 
                    !string.IsNullOrEmpty(this._fName)
                    && !string.IsNullOrEmpty(this._lName)
                    && !string.IsNullOrEmpty(this._personNoSecret)
                    && !string.IsNullOrEmpty(this._placeOfBirth)
                    && !string.IsNullOrEmpty(this._phoneNo)
                    && this._selectedIndexSex != -1
                    && this._selectedIndexAllCountryCombobox != -1
                    && !string.IsNullOrEmpty(this._address)
                    && this._selectedIndexCivilStatus != -1
                    && !string.IsNullOrEmpty(this._email);
                });
            this.CancelCommand = new RelayCommand<Page>(this.Cancel);
        }
    }
}
