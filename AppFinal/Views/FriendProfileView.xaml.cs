using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppFinal.Cash;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppFinal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FriendProfileView : ContentPage
    {
        public FriendProfileView()
        {
            InitializeComponent();
            AddInteractionButton();
        }
        /// <summary>
        /// add the button to the view depending on the result of IsFriend Method
        /// </summary>
        public void AddInteractionButton()
        {
            if (IsFriend()) AddMessageButton();
            else
            {
                AddFriendRequestButton();
            }
        }
        /// <summary>
        /// add a button that allows the user to add the user as a friend
        /// </summary>
        /// <returns></returns>
        private async Task AddFriendRequestButton()
        {

            InteractiveBtn.Text = CurrentFriend.GetUser().friendsRequest.Contains(CurrentUser.GetUser().id) ? "Request Sent" : "Add Friend";

            if (InteractiveBtn.Text.Equals("Request Sent"))
            {
                InteractiveBtn.IsEnabled = false;
                InteractiveBtn.BackgroundColor = Color.DimGray;
                return;
            }

            InteractiveBtn.Clicked += async (sender, args) =>
            {
                await CurrentUser.GetUser().RequestFriendship(CurrentFriend.GetUser().id);
                InteractiveBtn.Text = "Request Sent";
                await DisplayAlert("Great!", "Request Sent!", "OK");
                CurrentFriend.GetUser().friendsRequest.AddLast(CurrentUser.GetUser().id);
                InteractiveBtn.IsEnabled = false;
                InteractiveBtn.BackgroundColor = Color.DimGray;


            };

        }
        /// <summary>
        /// add a button that allows the user to send a message to its friend
        /// </summary>
        private void AddMessageButton()
        {
            InteractiveBtn.Text = "Send Message";


            InteractiveBtn.Clicked += async (sender, args) =>
            {
                await AppShell.Current.GoToAsync("MessagesView");
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>true if the user is already a friend of the logged user</returns>
        private bool IsFriend()
        {
            return CurrentUser.GetUser().friends.Contains(CurrentFriend.GetUser().id);
        }


    }
}