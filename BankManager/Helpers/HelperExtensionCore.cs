/*
    Dipayan Sarker
    January 30, 2020
 */

using BankManager.Models.Data;
using EasyMVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BankManager.Helpers
{
    /// <summary>
    /// Helper extension class to aid some backend function calls
    /// </summary>
    public static class HelperExtensionCore
    {
        /// <summary>
        /// Gets a list of all* the name of the countries
        /// *may not list every nation in the world
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllCountryNames()
        {
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures); // gets an array of current cultureInfo
            return cultures.Select(cult => (new RegionInfo(cult.LCID)).EnglishName).Distinct().OrderBy(q => q).ToList(); // gets list of all the unique names and sorts them in alphabatic order
        }

        /// <summary>
        /// Returns specific Control from the given name of a page
        /// </summary>
        /// <param name="page"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static FrameworkElement GetControlWithName(this Page page, string name)
        {
            return (FrameworkElement)page.FindName(name);
        }

        /// <summary>
        /// Resets a Page to its initial state
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="page"></param>
        /// <param name="newViewModel"></param>
        public static void ResetPage(this IViewModel viewModel, Page page, IViewModel newViewModel)
        {
            viewModel.RemovePreviousPage(page);
            page.DataContext = newViewModel;
        }

        /// <summary>
        /// Removes previous Page element from the Frame of given Page
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="page"></param>
        public static void RemovePreviousPage(this IViewModel viewModel, Page page)
        {
            if (page != null)
            {
                page.NavigationService.RemoveBackEntry();
            }
        }

        /// <summary>
        /// Returns age from the specifc DateTime object
        /// </summary>
        /// <param name="dOB"></param>
        /// <returns></returns>
        public static int GetAge(this DateTime dOB) //from this--> https://stackoverflow.com/questions/9/how-do-i-calculate-someones-age-in-c
        {
            DateTime today = DateTime.Today;
            int a = (today.Year * 100 + today.Month) * 100 + today.Day;
            int b = (dOB.Year * 100 + dOB.Month) * 100 + dOB.Day;            
            return (a - b) / 10000;
        }

        /// <summary>
        /// Checks if the specified DateTime object is 16 years old or not
        /// </summary>
        /// <param name="dOB"></param>
        /// <returns></returns>
        public static bool IsSixteenYearsOld(this DateTime dOB)
        {
            return dOB.GetAge() >= 16;
        }

        /// <summary>
        /// Returns the name of TransactionType as a string
        /// </summary>
        /// <param name="transactionType"></param>
        /// <returns></returns>
        public static string TransactionTypeToString(this TransactionType transactionType)
        {
            return Enum.GetName(typeof(TransactionType), transactionType); // GetName() gets the string name of an Enum from the given type and instance of TransactionType
        }
    }
}
