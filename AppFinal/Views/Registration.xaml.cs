using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppFinal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Registration : ContentPage
    {
        public Registration()
        {
            InitializeComponent();
        }


        /// <summary>
        /// goes back to loginPage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("LoginPage");
        }
        /// <summary>
        /// not implemented function to register a user
        /// </summary>
        private void Btn_Register()
        {
            //Not implemented yet
        }
    }

}