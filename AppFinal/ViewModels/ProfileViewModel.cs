using System.Collections.Generic;
using System.Threading.Tasks;
using AppFinal.Cash;
using AppFinal.DB.AccessClasses;
using AppFinal.Models;
using AppFinal.Views;

namespace AppFinal.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        public string UserName { get; } = CurrentUser.GetUser().username;
        public string UserPicture { get; } = CurrentUser.GetUser().pictureUrl;
        public int FriendsAmt { get; } = CurrentUser.GetUser().friends.Count;
        public int Matches { get; set; } 
        public string UserRegion { get; } = CurrentUser.GetUser().region;

        public ProfileViewModel()
        {
            Title = "Profile";
            SetMatches();

        }

        private async Task SetMatches()
        {
            var matches = await CurrentUser.GetMatches();
            this.Matches = matches.Count;
        }



    }

}
