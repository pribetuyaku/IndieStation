using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using AppFinal.Cash;
using AppFinal.Models;
using AppFinal.Views;
using Xamarin.Forms;

namespace AppFinal.ViewModels
{
    public class FriendsViewModel : BaseViewModel
    {

        public LinkedList<User> friends { get; set; }

        public FriendsViewModel()
        {
            Title = "Your Friends";

        }


    }
}
