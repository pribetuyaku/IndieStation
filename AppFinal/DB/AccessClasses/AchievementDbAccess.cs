using System;
using System.Threading.Tasks;
using AppFinal.Models;
using MongoDB.Bson;

namespace AppFinal.DB.AccessClasses
{
    /// <summary>
    /// Public access to achievements collection
    /// </summary>
    public class AchievementDbAccess : MainDbAbstract<Achievement>
    {

        /// <summary>
        /// Instantiate a new achievements access
        /// </summary>
        public AchievementDbAccess()
        {
            this.CollectionName = "achievements";
        }

        /// <summary>
        /// Get an achievement by its name
        /// </summary>
        /// <param name="name">name of achievement</param>
        /// <returns>Achievement object</returns>
        public async Task<Achievement> GetAchievementByName(string name)
        {
            var filter = "{\"name\": \"" + name + "\"}";
            var bson = await Db.FindOne(this.CollectionName, filter);
            return GetObjectFromBsonDocument(bson);

        }

        protected override Achievement GetObjectFromBsonDocument(BsonDocument document)
        {
            try
            {
                var postId = document["_id"].ToString();
                var name = document["name"].AsString;
                var thumbnailUrl = document["thumbnail"].AsString;
                var description = document["description"].AsString;
                var achievementPoints = document["achievementPoints"].AsInt32;

                return new Achievement(postId, name, thumbnailUrl, description, achievementPoints);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }

        }

        protected override string GetUpdateDefinition(Achievement obj)
        {
            var update = "{\"$set\": {\"thumbnail\": \"" + obj.thumbnailUrl + "\"," +
            "\"description\": \"" + obj.description + "\"," +
            "\"achievementPoints\": \"" + obj.achievementPoints + "\"}}";

            return update;
        }

        protected override string GetBsonDocument(Achievement obj)
        {
            return obj.GetBsonDocument();
        }
    }
}