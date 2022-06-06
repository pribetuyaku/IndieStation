using System;
using System.Collections;
using System.Threading.Tasks;
using AppFinal.Cash;
using AppFinal.DB.AccessClasses;
using AppFinal.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppFinal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]


    public partial class Feed : ContentPage
    {
        private ArrayList postsId = new ArrayList();

        public Feed()
        {

            InitializeComponent();
            UpdatePosts();
        }

        /// <summary>
        /// Gets all the posts in the db and add them to the screen, 
        /// </summary>
        /// <returns></returns>
        public async Task UpdatePosts()
        {
            var posts = await Post.GetAllPosts();
            foreach (var post in posts)
            {
                if (!postsId.Contains(post.Id))
                {
                    await FillPost(post);
                    postsId.Add(post.Id);
                }
                
            }
        }
        /// <summary>
        /// creates a new grid in the view with the post in the param
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        private async Task FillPost(Post post)
        {
            var newGrid = new Grid()
            {
                Margin = new Thickness(10, 0, 10, 0),
                BackgroundColor = Color.LightGray,
                Padding = new Thickness(10, 10, 10, 10),
            };
            var rowDef1 = new RowDefinition();
            var rowDef2 = new RowDefinition();
            var rowDef3 = new RowDefinition();

            rowDef1.Height = 80;
            rowDef2.Height = GridLength.Auto;
            rowDef3.Height = 50;

            newGrid.RowDefinitions.Add(rowDef1);
            newGrid.RowDefinitions.Add(rowDef2);
            newGrid.RowDefinitions.Add(rowDef3);



            var user = await new UserDbAccess().FindOne(post.UserId);

            var image = new Image
            {
                Source = (user.pictureUrl),
                Aspect = Aspect.AspectFit,
                HeightRequest = 200,
                WidthRequest = 200,
            };
            Grid.SetRow(image, 0);
            Grid.SetColumn(image, 0);
            newGrid.Children.Add(image);

            var lblUsername = new Label
            {
                Text = user.username,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = 20
            };
            Grid.SetColumnSpan(lblUsername, 2);
            Grid.SetColumn(lblUsername, 1);
            Grid.SetRow(lblUsername, 0);

            newGrid.Children.Add(lblUsername);

            var lblPostContent = new Label
            {
                Padding = new Thickness(25, 10, 25, 10),
                Text = post.Content,
                FontSize = 20,
                BackgroundColor = Color.White,

            };
            Grid.SetRow(lblPostContent, 1);
            Grid.SetColumnSpan(lblPostContent, 3);

            newGrid.Children.Add(lblPostContent);

            var btnLike = new Button
            {
                FontSize = 14,
                Text = "I like it!!!",
                BackgroundColor = Color.FromHex("#003638")
            };

            Grid.SetRow(btnLike, 2);
            Grid.SetColumn(btnLike, 0);

            newGrid.Children.Add(btnLike);

            var btnProfile = new Button
            {
                FontSize = 14,
                Text = "VISIT PROFILE",
                BackgroundColor = Color.FromHex("#003638"),


            };
            btnProfile.Clicked += async (sender, args) =>
            {

                CurrentFriend.SetUser(user);
                await AppShell.Current.GoToAsync("FriendProfileView");
            };

            Grid.SetRow(btnProfile, 2);
            Grid.SetColumn(btnProfile, 1);

            newGrid.Children.Add(btnProfile);

            var lblTimeStamp = new Label
            {
                Text = post.Date,
                FontSize = 10,
                HorizontalTextAlignment = TextAlignment.Center,
            };
            Grid.SetRow(lblTimeStamp, 2);
            Grid.SetColumn(lblTimeStamp, 2);

            newGrid.Children.Add(lblTimeStamp);
            MainLayout.Children.Insert(0, newGrid);
            // MainLayout.Children.Add(newGrid);
        }
        /// <summary>
        /// send the post to the db and updates the view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Post(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(PostTyped.Text))
            {
                var post = await CurrentUser.GetUser().Post(PostTyped.Text, null);

                await FillPost(post);
                postsId.Add(post.Id);
                PostTyped.Text = "";
            }
        }
        /// <summary>
        /// load more posts to the view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnRefreshPosts(object sender, EventArgs e)
        {
            // var newPosts = await Post.GetAllPosts();
            // foreach (var post in newPosts)
            // {
            //     
            //     if (!postsId.Contains(post.Id))
            //     {
            //         await FillPost(post);
            //         postsId.Add(post.Id);
            //     }
            // }
            UpdatePosts();
        }
    }
}