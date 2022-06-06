using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AppFinal.Models;

namespace AppFinal.Cash
{

    /*
     * Class to hold the user other than the logged user in focus, used to add friends, send messages and visit profiles
     */
    static class CurrentFriend
    {
        public static User _currentFriend;

        /// <summary>
        /// sets the current Friend
        /// </summary>
        /// <param name="user"></param>
        public static void SetUser(User user)
        {
            _currentFriend = user;
        }
        /// <summary>
        /// returns the current friend
        /// </summary>
        /// <returns>User</returns>
        public static User GetUser()
        {
            return _currentFriend;
        }
        /// <summary>
        /// gets the game matches of the user to be used in it profile
        /// </summary>
        /// <returns>GameMatch</returns>
        public static async Task<LinkedList<GameMatch>> GetMatches()
        {
            return await _currentFriend.GetGameMatches();
        }
    }
}
