using System;
using AppFinal.Cash;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppFinal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Profile : ContentPage
    {
        public Profile()
        {
            InitializeComponent();
        }
        /// <summary>
        /// logs off of the Application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        public void Logout_Clicked(object sender, EventArgs eventArgs)
        {
            CurrentUser.LogOff(); 
            Shell.Current.GoToAsync("LoginPage");
        }
    }
}