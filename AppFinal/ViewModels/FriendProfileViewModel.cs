using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AppFinal.Cash;

namespace AppFinal.ViewModels
{
    internal class FriendProfileViewModel : BaseViewModel
    {
        public string UserName { get; } = CurrentFriend.GetUser().username;
        public string UserPicture { get; } = CurrentFriend.GetUser().pictureUrl;
        public int FriendsAmt { get; } = CurrentFriend.GetUser().friends.Count;
        public int Matches { get; set; }
        public string UserRegion { get; } = CurrentFriend.GetUser().region;

        public FriendProfileViewModel()
        {
            Title = SetTitle();
            SetMatches();

        }
        /// <summary>
        /// set the matches for the current friend in his rofile
        /// </summary>
        /// <returns></returns>
        private async Task SetMatches()
        {
            var matches = await CurrentFriend.GetMatches();
            this.Matches = matches.Count;
        }
        /// <summary>
        /// set the title with the username
        /// </summary>
        /// <returns></returns>
        private string SetTitle()
        {
            return UserName + "'s Profile" ;
        }
    }
}
