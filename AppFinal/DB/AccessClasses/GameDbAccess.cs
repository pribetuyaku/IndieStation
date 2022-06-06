using System;
using AppFinal.Models;
using MongoDB.Bson;

namespace AppFinal.DB.AccessClasses
{
    /// <summary>
    /// Public access to games collection
    /// </summary>
    public class GameDbAccess : MainDbAbstract<Game>
    {
        /// <summary>
        /// Instantiate a new games access
        /// </summary>
        public GameDbAccess()
        {
            this.CollectionName = "games";
        }

        protected override Game GetObjectFromBsonDocument(BsonDocument document)
        {
            try
            {
                var id = document["_id"].ToString();
                var name = document["name"].AsString;
                var thumbnailUrl = document["thumbnail"].AsString;
                var description = document["description"].AsString;
                var path = document["path"].AsString;

                return new Game(id, name, thumbnailUrl, description, path);
            }
            catch (Exception)
            {
                return null;
            }
        }

        protected override string GetUpdateDefinition(Game obj)
        {
            var update = "{\"$set\": {\"name\": \"" + obj.name + "\"," +
            "\"thumbnail\": \"" + obj.thumbnailUrl + "\"," +
            "\"description\": \"" + obj.description + "\"," +
            "\"path\": \"" + obj.path + "\"}}";

            return update;
        }

        protected override string GetBsonDocument(Game obj)
        {
            return obj.GetBsonDocument();
        }
    }
}