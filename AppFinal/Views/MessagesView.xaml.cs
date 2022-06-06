using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using AppFinal.Cash;
using AppFinal.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static AppFinal.Cash.CurrentUser;

namespace AppFinal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessagesView : ContentPage
    {
        private User currentUser = CurrentUser.GetUser();
        private User currentFriend = CurrentFriend.GetUser();
        private ArrayList messagesIds = new ArrayList();

        public MessagesView()
        {
            InitializeComponent();
            this.BackgroundImageSource = "backgroundMessages.png";
            UpdateMessages();
            //repeats the method everysecond to get new messages
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                UpdateMessages();
                return true; // True = Repeat again, False = Stop the timer
            });
        }
        /// <summary>
        /// get new messages from the database
        /// </summary>
        /// <returns></returns>
        public async Task UpdateMessages()
        {
            var newMessages = await GetMessagesFromCurrentFriend(currentFriend.id);
            foreach (var message in newMessages)
            {
                if (!messagesIds.Contains(message.id))
                {
                    FillGrid(message);
                    messagesIds.Add(message.id);
                }
            }
        }
        /// <summary>
        /// adds a message to the view, have different styles for the sender and the receiver.
        /// </summary>
        /// <param name="message"></param>
        public void FillGrid(Message message)
        {
            if (message.sender == currentUser.id)
            {
                var newGrid = new Grid
                {
                    BindingContext = message.id,
                    Margin = new Thickness(60, 0, 20, 0)
                };

                var rowDef = new RowDefinition
                {
                    Height = 20
                };
                newGrid.RowDefinitions.Add(rowDef);

                var userNameLabel = new Label
                {
                    Text = currentUser.username + " " + message.date,
                    FontAttributes = FontAttributes.Bold,
                    HorizontalTextAlignment = TextAlignment.End,
                    FontSize = 10,
                    TextColor = Color.White,
                    Margin = new Thickness(10, 10, 0, -10)
                };

                newGrid.Children.Add(userNameLabel);

                var messageLabel = new Label
                {
                    Text = message.content,
                    BackgroundColor = Color.CadetBlue,
                    HorizontalTextAlignment = TextAlignment.End
                };

                Grid.SetRow(messageLabel, 1);
                newGrid.Children.Add(messageLabel);
                MainLayout.Children.Add(newGrid);
            }
            else
            {
                var newGrid = new Grid
                {
                    BindingContext = message.id,
                    Margin = new Thickness(20, 0, 60, 0)
                };

                var rowDef = new RowDefinition
                {
                    Height = 20
                };
                newGrid.RowDefinitions.Add(rowDef);

                var userNameLabel = new Label
                {
                    Text = currentFriend.username + " " + message.date,
                    FontSize = 10,
                    TextColor = Color.White,
                    FontAttributes = FontAttributes.Bold,
                    Margin = new Thickness(0, 10, 10, -10)
                };

                newGrid.Children.Add(userNameLabel);

                var messageLabel = new Label
                {
                    Text = message.content,
                    BackgroundColor = Color.DarkCyan,
                };

                Grid.SetRow(messageLabel, 1);
                newGrid.Children.Add(messageLabel);
                MainLayout.Children.Add(newGrid);

            }
        }
        /// <summary>
        /// sends the message to the friend and database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSendMessage(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Message.Text))
            {
                var message = new Message(currentUser.id, currentFriend.id, Message.Text, null);
                Message.Text = "";
                currentUser.SendMessage(message);
            }
        }
    }
}