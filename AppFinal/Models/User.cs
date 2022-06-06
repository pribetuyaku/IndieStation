using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AppFinal.DB.AccessClasses;
using MongoDB.Bson;

namespace AppFinal.Models
{
    /// <summary>
    /// users collection class
    /// </summary>
    public class User
    {
        public static UserDbAccess DbAccess = new UserDbAccess();
        public static MessageDbAccess MessageDbAccess = new MessageDbAccess();
        public static PostDbAccess PostDbAccess = new PostDbAccess();
        public static CommentDbAccess CommentDbAccess = new CommentDbAccess();
        public string id { get; private set; }
        public string username { get; private set; }
        public string pictureUrl { get; private set; }
        public string email { get; private set; }
        public string language { get; private set; }
        public string region { get; private set; }
        public int accountLevel { get; private set; }
        public int achievementPoints { get; private set; }
        public LinkedList<string> friends { get; }
        public LinkedList<string> achievements { get; }
        public LinkedList<string> friendsRequest { get; private set; }


        /// <summary>
        /// Instantiate existing user
        /// </summary>
        /// <param name="id">user id</param>
        /// <param name="username">username</param>
        /// <param name="pictureUrl">url of picture</param>
        /// <param name="email">email address</param>
        /// <param name="language">preferred language</param>
        /// <param name="region">current region</param>
        /// <param name="accountLevel">level of account based on achievements</param>
        /// <param name="achievementPoints">points gained from achievements</param>
        /// <param name="friends">list of friends ids</param>
        /// <param name="achievements">list of achievement ids</param>
        /// <param name="requests">list of friends requests</param>
        public User(string id, string username, string pictureUrl, string email, string language, string region, int accountLevel, int achievementPoints, LinkedList<string> friends, LinkedList<string> achievements, LinkedList<string> requests)
        {
            this.id = id;
            this.username = username;
            this.pictureUrl = pictureUrl;
            this.email = email;
            this.language = language;
            this.region = region;
            this.accountLevel = accountLevel;
            this.achievementPoints = achievementPoints;
            this.friends = friends;
            this.achievements = achievements;
            this.friendsRequest = requests;
        }

        /// <summary>
        /// Instantiate a new User
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="email">email address</param>
        public User(string username, string email)
        {
            this.id = ObjectId.GenerateNewId().ToString();
            this.username = username;
            this.pictureUrl =
                "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fstatic.vecteezy.com%2Fsystem%2Fresources%2Fpreviews%2F000%2F574%2F512%2Foriginal%2Fvector-sign-of-user-icon.jpg&f=1&nofb=1";
            this.email = email;
            this.language = "English";
            this.region = "EU";
            this.accountLevel = 0;
            this.achievementPoints = 0;
            this.friends = new LinkedList<string>();
            this.achievements = new LinkedList<string>();
            this.friendsRequest = new LinkedList<string>();
        }

        /// <summary>
        /// Update user region
        /// </summary>
        /// <param name="reg">region name</param>
        /// <returns>success of update</returns>
        public async Task<bool> UpdateRegion(string reg)
        {
            this.region = reg;
            return await Update();
        }

        /// <summary>
        /// Update user language
        /// </summary>
        /// <param name="lang">language name</param>
        /// <returns>success of update</returns>
        public async Task<bool> UpdateLanguage(string lang)
        {
            this.language = lang;
            return await Update();
        }

        /// <summary>
        /// Adds a friend to the friends list
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>success of update</returns>
        public async Task<bool> AcceptRequest(string userId)
        {
            this.friends.AddLast(userId);
            var newFriend = await DbAccess.FindOne(userId);
            this.friendsRequest.Remove(userId);
            newFriend.friends.AddLast(this.id);
            await newFriend.Update();
            return await Update();
        }

        /// <summary>
        /// Cancel a friend request by another user
        /// </summary>
        /// <param name="userId">id of user who requested friendship</param>
        /// <returns>success of update</returns>
        public async Task<bool> CancelFriendRequest(string userId)
        {
            this.friendsRequest.Remove(userId);
            return await Update();
        }

        /// <summary>
        /// Requests friendship
        /// </summary>
        /// <param name="userId">user to request friendship for</param>
        /// <returns>success of update</returns>
        public async Task<bool> RequestFriendship(string userId)
        {
            var requestFriend = await DbAccess.FindOne(userId);
            requestFriend.friendsRequest.AddLast(this.id);
            return await requestFriend.Update();
        }

        /// <summary>
        /// Update a user picture
        /// </summary>
        /// <param name="url">url of new picture</param>
        /// <returns>success of update</returns>
        public async Task<bool> ChangePicture(string url)
        {
            this.pictureUrl = url;
            return await Update();
        }

        /// <summary>
        /// Removes a friend from the friends list
        /// </summary>
        /// <param name="userId">id of friend to be removed</param>
        /// <returns>success of update</returns>
        public async Task<bool> RemoveFriend(string userId)
        {
            this.friends.Remove(userId);
            var removedFriend = await DbAccess.FindOne(userId);
            await removedFriend.RemoveFriend(this.id);
            await removedFriend.Update();
            return await Update();
        }

        /// <summary>
        /// Adds achievement points to user
        /// </summary>
        /// <param name="points">points to be added</param>
        private void AddAchievementPoints(int points)
        {
            this.achievementPoints += points;
        }

        /// <summary>
        /// Increases the account level
        /// </summary>
        private void AccountLevelUp()
        {
            this.accountLevel++;
        }

        /// <summary>
        /// Send a message
        /// </summary>
        /// <param name="receiver">id of user the message is directed to</param>
        /// <param name="content">content of message</param>
        /// <param name="mediaUrl">url of media of message</param>
        /// <returns>success of document creation</returns>
        // public async Task<bool> SendMessage(string receiver, string content, string mediaUrl)
        // {
        //     return await MessageDbAccess.InsertOne(new Message(this.id, receiver, content, mediaUrl));
        // }
        public async Task<bool> SendMessage(Message message)
        {
            return await MessageDbAccess.InsertOne(message);
        }

        /// <summary>
        /// Create a post
        /// </summary>
        /// <param name="content">post content</param>
        /// <param name="mediaUrl">url of media of post</param>
        /// <returns>success of document creation</returns>
        public async Task<Post> Post(string content, string mediaUrl)
        {
            var newPost = new Post(this.id, content, mediaUrl);
            await PostDbAccess.InsertOne(newPost);
            return newPost;
        }

        /// <summary>
        /// Comment a post or a comment
        /// </summary>
        /// <param name="postId">post or comment id</param>
        /// <param name="content">content of new comment</param>
        /// <param name="mediaUrl">url of media of comment</param>
        /// <returns>success of document creation</returns>
        public async Task<bool> Comment(string postId, string content, string mediaUrl)
        {
            return await CommentDbAccess.InsertOne(new Comment(postId, this.id, content, mediaUrl));
        }

        /// <summary>
        /// Adds achievement to achievements list and increases the account level and achievement points accordingly
        /// </summary>
        /// <param name="achievementId">id of achievement</param>
        /// <returns>success of update</returns>
        public async Task<bool> NewAchievement(string achievementId)
        {
            var achievement = await new AchievementDbAccess().FindOne(achievementId);
            var pointsToAdd = achievement.achievementPoints;
            this.achievements.AddLast(achievementId);
            CheckLevelUp(pointsToAdd);

            return await Update();
        }

        /// <summary>
        /// Level up account and increase the achievement points
        /// </summary>
        /// <param name="pointsToAdd">number of points to be added</param>
        private void CheckLevelUp(int pointsToAdd)
        {
            var gap = 20;

            var upLevel = (int)Math.Floor((double)(pointsToAdd / gap));

            var minLevel = this.accountLevel * gap;
            var difference = this.achievementPoints - minLevel;
            if (difference == 0)
            {
                for (var i = 0; i < upLevel; i++)
                {
                    this.AccountLevelUp();
                }
            }
            else
            {
                upLevel = (int)Math.Floor((double)((pointsToAdd + difference) / gap));
                for (int i = 0; i < upLevel; i++)
                {
                    this.AccountLevelUp();
                }
            }

            AddAchievementPoints(pointsToAdd);
        }

        public async Task<int> GetVictories()
        {
            var victories = 0;

            var matches = await GetGameMatches();
            foreach (var match in matches)
            {
                if (match.winnerId.Equals(this.id))
                {
                    victories++;
                }
            }

            return victories;
        }

        public async Task<LinkedList<GameMatch>> GetGameMatches()
        {
            return await new GameMatchDbAccess().GetUserGameMatches(this.id);
        }

        /// <summary>
        /// Get all messages sent and received by user
        /// </summary>
        /// <returns>LinkedList with all messages</returns>
        public async Task<LinkedList<Message>> GetAllMessages()
        {
            return await MessageDbAccess.GetUserMessages(this.id);
        }

        /// <summary>
        /// Get all sent messages by user
        /// </summary>
        /// <returns>LinkedList with sent messages</returns>
        public async Task<LinkedList<Message>> GetSentMessages()
        {
            return await MessageDbAccess.GetUserSentMessages(this.id);
        }

        /// <summary>
        /// Get all received messages by user
        /// </summary>
        /// <returns>LinkedList with received messages</returns>
        public async Task<LinkedList<Message>> GetReceivedMessages()
        {
            return await MessageDbAccess.GetUserReceivedMessages(this.id);
        }

        /// <summary>
        /// Get all unread messages by user
        /// </summary>
        /// <returns>LinkedList with unread messages</returns>
        public async Task<LinkedList<Message>> GetUnreadMessages()
        {
            return await MessageDbAccess.GetUserUnreadMessages(this.id);
        }

        /// <summary>
        /// Update document in DB
        /// </summary>
        /// <returns>success of update</returns>
        private async Task<bool> Update()
        {
            try
            {
                return await DbAccess.UpdateOne(this, this.id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        /// <summary>
        /// Check user credentials against DB
        /// </summary>
        /// <param name="email">user email address</param>
        /// <param name="password">user password</param>
        /// <returns>user if login is correct and null if it isn't</returns>
        public static async Task<User> Login(string email, string password)
        {
            return await DbAccess.Login(email, password);
        }

        /// <summary>
        /// Change a user's password
        /// </summary>
        /// <param name="oldPassword">current password</param>
        /// <param name="newPassword">new password</param>
        /// <returns>0: invalid password, 1: error in the update, 2: successful update</returns>
        public async Task<int> UpdatePassword(string oldPassword, string newPassword)
        {
            return await DbAccess.UpdatePassword(this.id, this.email, oldPassword, newPassword);
        }

        /// <summary>
        /// Create a new user with username and password
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password">plain text</param>
        /// <returns>new user or null if unsuccessful</returns>
        public static async Task<User> CreateUser(string username, string password)
        {
            var user = new User(username, password);
            var success = await DbAccess.InsertOne(user, password);
            return success ? user : null;
        }

        /// <summary>
        /// Delete user by id
        /// </summary>
        /// <param name="delId">user id to be deleted</param>
        /// <returns>success of deletion</returns>
        public static async Task<bool> DeleteUser(string delId)
        {
            return await DbAccess.DeleteOne(delId);
        }

        public override string ToString()
        {
            return $"{nameof(id)}: {id}, {nameof(username)}: {username}, {nameof(pictureUrl)}: {pictureUrl}, {nameof(email)}: {email}, {nameof(language)}: {language}, {nameof(region)}: {region}, {nameof(accountLevel)}: {accountLevel}, {nameof(achievementPoints)}: {achievementPoints}, {nameof(friends)}: {friends.Count}, {nameof(achievements)}: {achievements.Count}";
        }

        /// <summary>
        /// Get JSON document
        /// </summary>
        /// <returns></returns>
        public string GetJsonDocument()
        {

            var friendsArray = DbAccess.GetStringFromLinkedList(this.friends);

            var achievementsArray = DbAccess.GetStringFromLinkedList(this.achievements);

            var requestsArray = DbAccess.GetStringFromLinkedList(this.friendsRequest);

            var bsonDoc = "{" +
                          "\"_id\": " + "\"" + this.id + "\"," +
                          "\"username\": \"" + this.username + "\"," +
                          "\"profilePicture\": \"" + this.pictureUrl + "\"," +
                          "\"email\": \"" + this.email + "\"," +
                          "\"language\": \"" + this.language + "\"," +
                          "\"region\": \"" + this.region + "\"," +
                          "\"accountLevel\":" + this.accountLevel + "," +
                          "\"totalAchievementPoints\": " + this.achievementPoints + "," +
                          "\"friends\":" + friendsArray + "," +
                          "\"friendsRequest\": " + requestsArray + "," +
                          "\"achievements\": " + achievementsArray + "}";

            return bsonDoc;
        }
        /// <summary>
        /// get all the users that request friendship in the app
        /// </summary>
        /// <returns>User LinkedList</returns>
        public async Task<LinkedList<User>> GetFriendRequests()
        {
            var users = new LinkedList<User>();

            foreach (var id in friendsRequest)
            {
                users.AddLast(await DbAccess.FindOne(id));
            }



            return users;
        }
    }
}