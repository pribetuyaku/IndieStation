using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using System.Xml;
using AppFinal.Cash;
using AppFinal.Models;
using AppFinal.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppFinal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Friends : ContentPage
    {
        public Friends()
        {
            InitializeComponent();
            FillGridAllFriends();
        }
        /// <summary>
        /// fill the fiew with all the friends and friend requests
        /// </summary>
        /// <returns></returns>
        public async Task FillGridAllFriends()
        {
            var userRequests = await CurrentUser.GetUser().GetFriendRequests();
            foreach (var request in userRequests)
            {
                FillRequestGrid(request);
            }
            var users = await CurrentUser.GetFriends();
            foreach (var user in users)
            {
                FillFriendGrid(user);
            }
        }
        /// <summary>
        ///creates a grid with a friend with buttons to send message or view profile
        /// </summary>
        /// <param name="user"></param>
        public void FillFriendGrid(User user)
        {
            //creates a grid and its definitions
            var newGrid = new Grid
            {
                Margin = new Thickness(10, 0, 10, 0)
            };

            var rDef1 = new RowDefinition();
            var rDef2 = new RowDefinition();
            var rDef3 = new RowDefinition();

            rDef1.Height = GridLength.Auto;
            rDef2.Height = GridLength.Auto;
            rDef3.Height = GridLength.Auto;

            newGrid.RowDefinitions.Add(rDef1);
            newGrid.RowDefinitions.Add(rDef2);
            newGrid.RowDefinitions.Add(rDef3);

            //add the image
            var image = new Image
            {
                Source = (user.pictureUrl),
                Aspect = Aspect.AspectFit,
                HeightRequest = 200,
                WidthRequest = 200
            };

            Grid.SetRowSpan(image, 2);
            Grid.SetRow(image, 0);
            Grid.SetColumn(image, 0);

            newGrid.Children.Add(image);

            //label with username
            var labelUsername = new Label
            {
                FontSize = 15,
                Text = user.username,
                Margin = new Thickness(10, 50, 20, 20)
            };
            Grid.SetColumn(labelUsername, 1);
            Grid.SetRow(labelUsername, 0);

            newGrid.Children.Add(labelUsername);

            //button to see profile
            var btnSeeProfile = new Xamarin.Forms.Button
            {
                Margin = new Thickness(10, 0, 10, 10),
                Text = "See Profile",
                BackgroundColor = Color.FromHex("#003638"),
                BindingContext = user.id.ToString()
            };
            btnSeeProfile.Clicked += async (sender, args) =>
            {
                string data = ((Button)sender).BindingContext as string;
                var friend = await CurrentUser.GetFriend(data);
                CurrentFriend.SetUser(friend);
                await AppShell.Current.GoToAsync("FriendProfileView");
            };

            Grid.SetColumn(btnSeeProfile, 1);
            Grid.SetRow(btnSeeProfile, 1);

            newGrid.Children.Add(btnSeeProfile);

            //button to see profile
            var btnSendMessage = new Button
            {
                Margin = new Thickness(10, 0, 10, 10),
                Text = "Messages",
                BackgroundColor = Color.FromHex("#003638"),
                BindingContext = user.id.ToString()
            };

            btnSendMessage.Clicked += async (sender, args) =>
            {
                string data = ((Button)sender).BindingContext as string;
                var friend = await CurrentUser.GetFriend(data);
                CurrentFriend.SetUser(friend);
                await AppShell.Current.GoToAsync("MessagesView");
            };


            Grid.SetColumn(btnSendMessage, 2);
            Grid.SetRow(btnSendMessage, 1);

            newGrid.Children.Add(btnSendMessage);

            //separator for each friend
            var separator = new Label
            {
                Text = "___________________________________________________________"
            };
            btnSeeProfile.BackgroundColor = Color.FromHex("#003638");
            Grid.SetRow(separator, 3);
            Grid.SetColumnSpan(separator, 3);

            newGrid.Children.Add(separator);


            MainLayout.Children.Add(newGrid);

        }
        /// <summary>
        /// creates a grid with a friend request with buttons to accept or decline
        /// </summary>
        /// <param name="user"></param>
        public void FillRequestGrid(User user)
        {
            //creates a grid and its definitions
            var newGrid = new Grid
            {
                Margin = new Thickness(10, 0, 10, 0),
                BackgroundColor = Color.LightGray,
                BindingContext = user.id,
            };

            var rDef1 = new RowDefinition();
            var rDef2 = new RowDefinition();
            var rDef3 = new RowDefinition();

            rDef1.Height = GridLength.Auto;
            rDef2.Height = GridLength.Auto;
            rDef3.Height = GridLength.Auto;

            newGrid.RowDefinitions.Add(rDef1);
            newGrid.RowDefinitions.Add(rDef2);
            newGrid.RowDefinitions.Add(rDef3);

            //add the image
            var image = new Image
            {
                Source = (user.pictureUrl),
                Aspect = Aspect.AspectFit,
                HeightRequest = 200,
                WidthRequest = 200
            };

            Grid.SetRowSpan(image, 2);
            Grid.SetRow(image, 0);
            Grid.SetColumn(image, 0);

            newGrid.Children.Add(image);

            //label with username
            var labelUsername = new Label
            {
                FontSize = 15,
                Text = "NEW FRIEND REQUEST FROM " + user.username,
                Margin = new Thickness(10, 50, 20, 20)
            };
            Grid.SetColumn(labelUsername, 1);
            Grid.SetColumnSpan(labelUsername, 2);
            Grid.SetRow(labelUsername, 0);
            Grid.SetRowSpan(labelUsername, 2);

            newGrid.Children.Add(labelUsername);
            var position = MainLayout.Children.Count + 1;
            //button to see profile
            var btnAcceptRequest = new Xamarin.Forms.Button
            {
                Margin = new Thickness(10, 0, 10, 10),
                Text = "Accept",
                BackgroundColor = Color.FromHex("#003638"),
                BindingContext = position.ToString()
            };
            btnAcceptRequest.Clicked += async (sender, args) =>
            {
                await CurrentUser.GetUser().AcceptRequest(user.id);
                FillFriendGrid(user);
                MainLayout.Children.Remove(newGrid);
            };

            Grid.SetColumn(btnAcceptRequest, 1);
            Grid.SetRow(btnAcceptRequest, 1);

            newGrid.Children.Add(btnAcceptRequest);

            //button to see profile
            var btnDeclineRequest = new Button
            {
                Margin = new Thickness(10, 0, 10, 10),
                Text = "Decline",
                BackgroundColor = Color.FromHex("#003638"),
                BindingContext = user.id.ToString()
            };

            btnDeclineRequest.Clicked += async (sender, args) =>
            {
                await CurrentUser.GetUser().CancelFriendRequest(user.id);
                MainLayout.Children.Remove(newGrid);
            };


            Grid.SetColumn(btnDeclineRequest, 2);
            Grid.SetRow(btnDeclineRequest, 1);

            newGrid.Children.Add(btnDeclineRequest);

            //separator for each friend
            var separator = new Label();
            separator.Text = "___________________________________________________________";
            btnAcceptRequest.BackgroundColor = Color.FromHex("#003638");
            Grid.SetRow(separator, 3);
            Grid.SetColumnSpan(separator, 3);

            newGrid.Children.Add(separator);


            MainLayout.Children.Add(newGrid);

        }

    }
}