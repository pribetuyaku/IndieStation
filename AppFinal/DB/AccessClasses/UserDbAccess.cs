using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AppFinal.Models;
using MongoDB.Bson;

namespace AppFinal.DB.AccessClasses
{
    /// <summary>
    /// Public access to users collection
    /// </summary>
    public class UserDbAccess : MainDbAbstract<User>
    {
        /// <summary>
        /// Instantiate a new users access
        /// </summary>
        public UserDbAccess()
        {
            this.CollectionName = "users";
        }

        /// <summary>
        /// User InsertOne(User user, string password) to create a new user
        /// </summary>
        [Obsolete("Don't use this method when creating a new user, use the method 'InsertOne(User user, string password)' to hash and salt the user's password", true)]
        public new bool InsertOne(User user)
        {
            return false;
        }

        /// <summary>
        /// Create a new user document
        /// </summary>
        /// <param name="user">user to be created</param>
        /// <param name="password">plain text password to be hashed and salted</param>
        /// <returns>success of creation</returns>
        public async Task<bool> InsertOne(User user, string password)
        {
            var salt = GenerateRandomSalt();
            Console.WriteLine("salt: " + Convert.ToBase64String(salt));

            var hashed = new Rfc2898DeriveBytes(password, salt).GetBytes(64);
            Console.WriteLine("hash: " + Convert.ToBase64String(hashed));

            var bson = user.GetJsonDocument();

            bson = bson.Substring(0, bson.Length - 1);
            bson += ", \"password\": \"" + Convert.ToBase64String(hashed) + "\",";
            bson += "\"salt\": \"" + Convert.ToBase64String(salt) + "\"}";

            return await Db.InsertOne(this.CollectionName, bson);
        }

        /// <summary>
        /// Generate a random salt
        /// </summary>
        /// <returns>random salt as byte array</returns>
        /// <source>https://stackoverflow.com/questions/7272771/encrypting-the-password-using-salt-in-c-sharp</source>
        public static byte[] GenerateRandomSalt()
        {
            var rng = new RNGCryptoServiceProvider();
            var bytes = new byte[64];
            rng.GetBytes(bytes);
            return bytes;
        }

        /// <summary>
        /// Get password and salt from a user from the DB
        /// </summary>
        /// <param name="email">user email address</param>
        /// <returns>dictionary keys: salt, pass. Values: byte arrays</returns>
        public async Task<Dictionary<string, byte[]>> GetHashedPasswordAndSalt(string email)
        {
            var dict = new Dictionary<string, byte[]>();
            var filter = "{\"email\": \"" + email + "\"}";
            var bson = await Db.FindOne(this.CollectionName, filter);

            dict.Add("salt", GetBytesFromString(bson["salt"].AsString));
            dict.Add("pass", GetBytesFromString(bson["password"].AsString));

            return dict;
        }

        private byte[] GetBytesFromString(string s)
        {
            return Convert.FromBase64String(s);
        }

        /// <summary>
        /// Check credentials against DB
        /// </summary>
        /// <param name="email">user email</param>
        /// <param name="password">user plain text password</param>
        /// <returns>the user if log in is correct and null if it isn't</returns>
        public async Task<User> Login(string email, string password)
        {

            var user = await Db.FindOne(this.CollectionName, "{\"email\": \"" + email + "\"}");
            if (user == null)
                return null;
            var pass = GetBytesFromString(user["password"].AsString);
            var salt = GetBytesFromString(user["salt"].AsString);

            var checkHash = new Rfc2898DeriveBytes(password, salt).GetBytes(64);
            return Convert.ToBase64String(checkHash).Equals(Convert.ToBase64String(pass)) ? GetObjectFromBsonDocument(user) : null;
            //var user = await Db.FindOne(this.CollectionName, "{\"email\": \"" + email + "\"}");
            //Console.WriteLine("bson user" + user);
            //var passAndHash = await GetHashedPasswordAndSalt(email);
            //Console.WriteLine("from DB:");
            //foreach (var a in passAndHash)
            //{
            //    Console.WriteLine(a.Key + ": " + a.Value);
            //}
            //var checkHash = new Rfc2898DeriveBytes(password, passAndHash["salt"]).GetBytes(64);
            //Console.WriteLine("From user: " + Convert.ToBase64String(checkHash));
            //return Convert.ToBase64String(checkHash).Equals(Convert.ToBase64String(passAndHash["pass"])) ? GetObjectFromBsonDocument(user) : null;

        }

        /// <summary>
        /// Change a user's password
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="email">user email address</param>
        /// <param name="oldPassword">current password</param>
        /// <param name="newPassword">new password</param>
        /// <returns>invalid credentials: 0, update error: 1, success: 2</returns>
        public async Task<int> UpdatePassword(string userId, string email, string oldPassword, string newPassword)
        {
            if (await Login(email, oldPassword) == null) return 0;

            var salt = GenerateRandomSalt();
            var hashed = new Rfc2898DeriveBytes(newPassword, salt).GetBytes(64);

            var update = "\"{$set\": {\"salt\": \"" + salt + "\", \"password\": \"" + hashed + "\"}";

            return await Db.UpdateOne(this.CollectionName, "{ \"_id\": \"" + new ObjectId(userId) + "\"}", update) ? 2 : 1;
        }


        /// <summary>
        /// Get all friends of a given user
        /// </summary>
        /// <param name="userId">id of the user to find friends of</param>
        /// <returns>LinkedList with all friends of a given user</returns>
        public async Task<LinkedList<User>> GetUserFriends(string userId)
        {
            var filter = "{\"friends\": \"" + userId + "\"}";
            var friendsBsonDocument = await Db.FindMany(this.CollectionName, filter);

            return GetLinkedListFromBsonList(friendsBsonDocument);
        }

        public User GetUserFromBson(BsonDocument bson)
        {
            return GetObjectFromBsonDocument(bson);
        }

        /// <summary>
        /// Converts BsonDocument to a User object
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        protected override User GetObjectFromBsonDocument(BsonDocument document)
        {
            try
            {
                var id = document["_id"].ToString();
                var username = document["username"].AsString;
                var pictureUrl = document["profilePicture"].AsString;
                var email = document["email"].AsString;
                var language = document["language"].AsString;
                var region = document["region"].AsString;
                var accountLevel = document["accountLevel"].AsInt32;
                var achievementPoints = document["totalAchievementPoints"].AsInt32;

                var friends = new LinkedList<string>();
                foreach (var friend in document["friends"].AsBsonArray)
                {
                    friends.AddLast(friend.AsString);
                }

                var achievements = new LinkedList<string>();
                foreach (var achievement in document["achievements"].AsBsonArray)
                {
                    achievements.AddLast(achievement.AsString);
                }

                var friendsRequest = new LinkedList<string>();
                if (document.Contains("friendsRequest"))
                {
                    foreach (var req in document["friendsRequest"].AsBsonArray)
                    {
                        friendsRequest.AddLast(req.AsString);
                    }
                }

                return new User(id, username, pictureUrl, email, language, region, accountLevel, achievementPoints,
                    friends, achievements, friendsRequest);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        /// <summary>
        /// Creates an update definition for mongodb {$set: {definition}}
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        protected override string GetUpdateDefinition(User user)
        {
            var friendsArray = GetStringFromLinkedList(user.friends);

            var achievementsArray = GetStringFromLinkedList(user.achievements);

            var requestsArray = GetStringFromLinkedList(user.friendsRequest);

            var update = "{\"$set\": {\"username\": \"" + user.username + "\"," +
                "\"profilePicture\": \"" + user.pictureUrl + "\"," +
                "\"email\": \"" + user.email + "\"," +
                "\"language\": \"" + user.language + "\"," +
                "\"region\": \"" + user.region + "\"," +
                "\"accountLevel\": " + user.accountLevel + "," +
                "\"totalAchievementPoints\": " + user.achievementPoints + "," +
                "\"friends\": " + friendsArray + "," +
                "\"achievements\": " + achievementsArray + "," +
                "\"friendsRequest\": " + requestsArray + "}}";

            return update;
        }

        protected override string GetBsonDocument(User obj)
        {
            return obj.GetJsonDocument();
        }
    }
}