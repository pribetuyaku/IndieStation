using System;
using System.Collections.Generic;
using System.Text;
using AppFinal.Cash;
using AppFinal.Models;

namespace AppFinal.ViewModels
{
    internal class MessagesViewModel : BaseViewModel
    {
        public string UserName { get; } = CurrentFriend.GetUser().username;

        public MessagesViewModel()
        {
            Title = SetTitle();
        }

        private string SetTitle()
        {
            return UserName + "'s Messages";
        }
    }
}
