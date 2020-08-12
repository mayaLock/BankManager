/*
    Dipayan Sarker
    February 10, 2020
*/

using BankManager.Helpers;
using BankManager.Models.Data;
using BankManager.Models.PageModels;
using EasyMVVM.ViewModels;
using EasyMVVM.Commands;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace BankManager.ViewModels.PageViewModel
{
    /// <summary>
    /// ViewModel for UpdatePage
    /// </summary>
    public class UpdatePageViewModel : ViewModelBase
    {
        // private instance variables
        private UpdatePageModel _updatePageModel;
        private int _accNum;
        private string _fName;
        private string _lName;
        private string _dOb;
        private string _personNoSecret;
        private string _placeOfBirth;
        private string _phoneNo;
        private string _sex;
        private string _citizenship;
        private string _address;
        private string _civilStatus;
        private string _email;
        private List<string> _allCountryList;
        private List<string> _allSexList;
        private List<string> _allCivilStatusList;
        private int _selectedIndexAllCountryCombobox;
        private int _selectedIndexSex;
        private int _selectedIndexCivilStatus;
        private bool _canExecuteFetchButton;
        private bool _canExecuteEditButton;
        private bool _invalidateAccountUIElement;
        private bool _invalidateOtherUIElements;

        /// <summary>
        /// Gets FetchCommand
        /// </summary>
        public ICommand FetchCommand { get; private set; }

        /// <summary>
        /// Gets EditCommand
        /// </summary>
        public ICommand EditCommand { get; private set; }

        /// <summary>
        /// Gets UpdateCommand
        /// </summary>
        public ICommand UpdateCommand { get; private set; }

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
        /// Gets or Sets AllSexList
        /// </summary>
        public List<string> AllSexList
        {
            get => this._allSexList;
            set => this.SetProperty(ref this._allSexList, value);
        }

        /// <summary>
        /// Gets or Sets AllCivilStatusList
        /// </summary>
        public List<string> AllCivilStatusList
        {
            get => this._allCivilStatusList;
            set => this.SetProperty(ref this._allCivilStatusList, value);
        }

        /// <summary>
        /// Gets or Sets InvalidateOtherUIElements
        /// </summary>
        public bool InvalidateOtherUIElements
        {
            get => this._invalidateOtherUIElements;
            set => this.SetProperty(ref this._invalidateOtherUIElements, value);
        }

        /// <summary>
        /// Gets or Sets  Invalid
        /// </summary>
        public bool InvalidateAccountUIElement
        {
            get => this._invalidateAccountUIElement;
            set => this.SetProperty(ref this._invalidateAccountUIElement, value);
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
                this._updatePageModel.AccountNumber = this._accNum;
            }
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
        /// Gets or Sets DOB
        /// </summary>
        public string DOB
        {
            get => this._dOb;
            set => this.SetProperty(ref this._dOb, value);
        }

        /// <summary>
        /// Gets or Sets PersonNumberSecret
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
        /// Gets IsValidAccount
        /// </summary>
        public bool IsValidAccount { get => this._updatePageModel.AccountExist; }

        /// <summary>
        /// Resets the currrent Page viewModel
        /// </summary>
        /// <param name="page"></param>
        private void Cancel(Page page)
        {
            this.ResetPage(page, new UpdatePageViewModel());
        }

        /// <summary>
        /// Fetches account information details
        /// </summary>
        private void Fetch()
        {
            if (!this.IsValidAccount) // if invalid account number we show error and leave
            {
                CustomMessageBox.CustomMessageBox customMessageBox = new CustomMessageBox.CustomMessageBox(null, "Warning!", "Invalid account number!", CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_OK | CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_ICONWARNING);
                customMessageBox.ShowDialog();
                return;
            }
            this.InvalidateAccountUIElement = true; // we disable account number displaying UI element
            BankDataStruct bankDataStruct = this._updatePageModel.BankDataInfo; // we fetch BankDataStruct and fill all the UI elements binding properties
            this.FirstName = bankDataStruct.FName;
            this.LastName = bankDataStruct.LName;
            this.DOB = bankDataStruct.DOB;
            this.PersonNomberSecret = bankDataStruct.PersonNo.Substring(9);
            this.Address = bankDataStruct.Address;
            this.PlaceOfBirth = bankDataStruct.PlaceOfBirth;
            this.Citizenship = bankDataStruct.Citizenship;
            this.Sex = bankDataStruct.Sex;
            this.CivilStatus = bankDataStruct.CivilStatus;
            this.PhoneNumber = bankDataStruct.PhoneNo;
            this.Email = bankDataStruct.Email;

            this._canExecuteFetchButton = false; // we disable get details button
            this._canExecuteEditButton = true; // we enable edit button
        }

        /// <summary>
        /// Enables editing 
        /// </summary>
        private void Edit()
        {
            this.InvalidateOtherUIElements = false; // we enable all the editable UI elements
            this._canExecuteEditButton = false;
        }

        /// <summary>
        /// Updates the clent information with new or existing data
        /// </summary>
        /// <param name="page"></param>
        private void Update(Page page)
        {
            if (!this._phoneNo.All(char.IsDigit)) // if not all digits we show error and return
            {
                CustomMessageBox.CustomMessageBox customMessageBox2 = new CustomMessageBox.CustomMessageBox(null, "Warning!", "Only digits allowed!", CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_OK | CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_ICONWARNING);
                customMessageBox2.ShowDialog();
                return;
            }          

            if (!new EmailAddressAttribute().IsValid(this._email)) // if invalid email we show error and return
            {
                CustomMessageBox.CustomMessageBox customMessageBox3 = new CustomMessageBox.CustomMessageBox(null, "Warning!", "Invalid email format!", CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_OK | CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_ICONWARNING);
                customMessageBox3.ShowDialog();
                return;
            }

            List<string> updateInfoList = new List<string>(); // we create a List of string with all the update user data
            updateInfoList.Add(this._fName);
            updateInfoList.Add(this._lName);
            updateInfoList.Add(this._address);
            updateInfoList.Add(this._phoneNo);
            updateInfoList.Add(this._email);
            updateInfoList.Add(this._citizenship);
            updateInfoList.Add(this._sex);
            updateInfoList.Add(this._civilStatus);

            if (!this._updatePageModel.UpdateClientInformation(updateInfoList.ToArray())) // if update of user info fails we show error and return
            {
                CustomMessageBox.CustomMessageBox customMessageBox1 = new CustomMessageBox.CustomMessageBox(null, "Error!", "Couldn't update user account information!", CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_OK | CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_ICONERROR);
                customMessageBox1.ShowDialog();
                return;
            } // else we reset the page and show success message to the user
            this.ResetPage(page, new UpdatePageViewModel());
            CustomMessageBox.CustomMessageBox customMessageBox = new CustomMessageBox.CustomMessageBox(null, "Info!", "Done updating!", CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_OK | CustomMessageBox.CustomMessageBox.MessageBoxAttributes.MB_ICONINFORMATION);
            customMessageBox.ShowDialog();
            return;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public UpdatePageViewModel()
        {
            this._updatePageModel = new UpdatePageModel(); // instantiating UpdatePageModel
            // initializing all the instance variables and ICommands
            this._accNum = 0;
            this._fName = string.Empty;
            this._lName = string.Empty;
            this._dOb = string.Empty;
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
            this._canExecuteFetchButton = true;
            this._canExecuteEditButton = false;
            this._invalidateAccountUIElement = false;
            this._invalidateOtherUIElements = true;
            this._allCountryList = HelperExtensionCore.GetAllCountryNames();
            this._allSexList = new List<string>() { "Male", "Female", "Other" };
            this._allCivilStatusList = new List<string>() { "Single", "Maried" };
            this.FetchCommand = new RelayCommand(this.Fetch, () => { return this._canExecuteFetchButton; });
            this.EditCommand = new RelayCommand(this.Edit, () => { return this._canExecuteEditButton; });
            this.UpdateCommand = new RelayCommand<Page>(this.Update, (p) => { return
                    !string.IsNullOrEmpty(this._fName)
                    && !string.IsNullOrEmpty(this._lName)
                    && !string.IsNullOrEmpty(this._phoneNo)
                    && this._selectedIndexSex != -1
                    && this._selectedIndexAllCountryCombobox != -1
                    && !string.IsNullOrEmpty(this._address)
                    && this._selectedIndexCivilStatus != -1
                    && !string.IsNullOrEmpty(this._email)
                    && !this._canExecuteEditButton;
                                });
            this.CancelCommand = new RelayCommand<Page>(this.Cancel);
        }
    }
}
