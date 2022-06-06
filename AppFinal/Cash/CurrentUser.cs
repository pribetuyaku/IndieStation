using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AppFinal.DB.AccessClasses;
using AppFinal.Models;

namespace AppFinal.Cash
{
    /// <summary>
    /// Class to hold the logged user in cash to save time getting information in the database
    /// </summary>
    static class CurrentUser
    {
        private static User _currentUser;
        /// <summary>
        /// checks if the user in the param is the logged user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static Boolean IsLoggedIn(User user)
        {
            if (_currentUser == user)
                return true;
            return false;
        }
        /// <summary>
        /// set the current logged user
        /// </summary>
        /// <param name="user"></param>
        public static void SetUser(User user)
        {
            _currentUser = user;
        }
        /// <summary>
        /// log the user off
        /// </summary>
        public static void LogOff()
        {
            _currentUser = null;
            CurrentFriend._currentFriend = null;

        }

        /// <summary>
        /// returns the current user
        /// </summary>
        /// <returns>User</returns>
        public static User GetUser()
        {
            return _currentUser;
        }
        /// <summary>
        /// returns the GameMatches of the logged user
        /// </summary>
        /// <returns>LinkedList GameMatch </returns>
        public static async Task<LinkedList<GameMatch>> GetMatches()
        {
            return await _currentUser.GetGameMatches();
        }
        /// <summary>
        /// returns all the friends from the logged user
        /// </summary>
        /// <returns>User Linked List</returns>
        public static async Task<LinkedList<User>> GetFriends()
        {
            return await new UserDbAccess().GetUserFriends(CurrentUser.GetUser().id);
        }
        /// <summary>
        /// get one single friend from the db
        /// </summary>
        /// <param name="id"></param>
        /// <returns>User</returns>
        public static async Task<User> GetFriend(string id)
        {
            foreach (var user in await GetFriends())
            {
                if (user.id == id)
                    return user;
            }

            return null;
        }
        /// <summary>
        /// get the messages exchanged between CurrenUser and the friend selected 
        /// </summary>
        /// <param name="friendId"></param>
        /// <returns>Message Linked List</returns>
        public static async Task<LinkedList<Message>> GetMessagesFromCurrentFriend(string friendId)
        {

            return await new MessageDbAccess().GetUserMessages(CurrentUser.GetUser().id, friendId);
        }
    }
}
