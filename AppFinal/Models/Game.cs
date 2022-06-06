using MongoDB.Bson;

namespace AppFinal.Models
{
    /// <summary>
    /// games collection class
    /// </summary>
    public class Game
    {
        public string id { get; private set; }
        public string name { get; private set; }
        public string thumbnailUrl { get; private set; }
        public string description { get; private set; }
        public string path { get; private set; }

        /// <summary>
        /// Instantiate a Game object
        /// </summary>
        /// <param name="id">game id</param>
        /// <param name="name">game name</param>
        /// <param name="thumbnailUrl">game thumbnail url</param>
        /// <param name="description">game description</param>
        /// <param name="path">path for xamarin to open game</param>
        public Game(string id, string name, string thumbnailUrl, string description, string path)
        {
            this.id = id;
            this.name = name;
            this.thumbnailUrl = thumbnailUrl;
            this.description = description;
            this.path = path;
        }

        /// <summary>
        /// Generate a BsonDocument from an object
        /// </summary>
        /// <returns>Bson Document</returns>
        public string GetBsonDocument()
        {

            var bsonDoc = "{" +
                "\"_id\": \""+ this.id + "\"," +
                "\"name\": \"" + this.name + "\"," +
                "\"thumbnail\": \"" + this.thumbnailUrl + "\"," +
                "\"description\": \"" + this.description + "\"," +
                "\"path\": \"" + this.path + "\"}";
        

            return bsonDoc.ToJson();
        }

        public override string ToString()
        {
            return $"{nameof(id)}: {id}, {nameof(name)}: {name}, {nameof(thumbnailUrl)}: {thumbnailUrl}, {nameof(description)}: {description}, {nameof(path)}: {path}";
        }
    }
}