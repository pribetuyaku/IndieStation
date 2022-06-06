using System;
using System.Runtime.CompilerServices;
using AppFinal.Cash;
using AppFinal.Views;
using Xamarin.Forms;

namespace AppFinal
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            //registering all routes for all views
            InitializeComponent();
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(Feed), typeof(Feed));
            Routing.RegisterRoute(nameof(Games), typeof(Games));
            Routing.RegisterRoute(nameof(Friends), typeof(Friends));
            Routing.RegisterRoute(nameof(Registration), typeof(Registration));
            Routing.RegisterRoute(nameof(Profile),typeof(Profile));
            Routing.RegisterRoute(nameof(FriendProfileView), typeof(FriendProfileView));
            Routing.RegisterRoute(nameof(MessagesView),typeof(MessagesView));
        }

       
        

    }
}
