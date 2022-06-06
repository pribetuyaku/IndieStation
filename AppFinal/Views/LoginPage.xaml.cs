using AppFinal.ViewModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AppFinal.Cash;
using AppFinal.DB.AccessClasses;
using AppFinal.Interfaces;
using AppFinal.Models;
using Newtonsoft.Json;
using Refit;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace AppFinal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = new LoginViewModel();
        }
        
        /// <summary>
        /// executes the login and alert the user if the credentials are not correct
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Btn_Login(object sender, EventArgs e)
        {
            
            if (string.IsNullOrEmpty(TxtUsername.Text))
            {
                await DisplayAlert("Ops!", "Email can't be empty", "OK");
            }
            else if (string.IsNullOrEmpty(TxtPassword.Text))
            {
                await DisplayAlert("Ops!", "Password can't be empty", "OK");
            }
            else
            {
                try
                {
                    
                    CurrentUser.SetUser(await User.Login(TxtUsername.Text.ToLower().Trim(), TxtPassword.Text));

                    Console.WriteLine(CurrentUser.GetUser());
                    if (CurrentUser.GetUser() != null)
                    {
                        await Shell.Current.GoToAsync("Profile");
                    }
                    else
                    {
                        await DisplayAlert("Ops!", "Wrong Email or Password", "OK");
                    }

                }
                catch (Exception exception)
                {
                    
                    Console.WriteLine(exception);
                    
                    throw;

                }
                
            
            }
           
        }
        /// <summary>
        /// Opens the registration page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync( "Registration");
        }

    }
}