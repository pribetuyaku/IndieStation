using System;
using AppFinal.Interfaces;
using Xamarin.Forms;

namespace AppFinal.Views
{

    public partial class Games : ContentPage
    {
        public Games()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Opens the treasure hunting game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Open_Game(object sender, EventArgs e)
        {
            var gameName = "com.DefaultCompany.com.unity.template.mobile2D";
            DependencyService.Get<IOpenGame>().OpenGame(gameName);

        }
    }
}