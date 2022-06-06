namespace AppFinal.Models
{
    /// <summary>
    /// achievements collection class
    /// </summary>
    public class Achievement
    {
        public string id { get; private set; }
        public string name { get; private set; }
        public string thumbnailUrl { get; private set; }
        public string description { get; private set; }
        public int achievementPoints { get; private set; }

        /// <summary>
        /// Instantiate an Achievement object
        /// </summary>
        /// <param name="id">achievement id</param>
        /// <param name="name">name of achievement</param>
        /// <param name="thumbnailUrl">url for achievement thumbnail</param>
        /// <param name="description">description of achievement</param>
        /// <param name="achievementPoints">points awarded for the achievement</param>
        public Achievement(string id, string name, string thumbnailUrl, string description, int achievementPoints)
        {
            this.id = id;
            this.name = name;
            this.thumbnailUrl = thumbnailUrl;
            this.description = description;
            this.achievementPoints = achievementPoints;
        }

        /// <summary>
        /// Transform an achievement into a BsonDocument for the DB
        /// </summary>
        /// <returns>BsonDocument</returns>
        public string GetBsonDocument()
        {

            var bsonDoc = "{"+
                "\"_id\": " + "\"" + this.id + "\"," +
                "\"name\": \"" + this.name + "\"," +
                "\"thumbnail\": \"" + this.thumbnailUrl + "\"," +
                "\"description\": \"" + this.description + "\"," +
                "\"achievementPoints\": " + this.achievementPoints + "}";

            return bsonDoc;
        }

        public override string ToString()
        {
            return $"Achievement: {nameof(id)}: {id}, {nameof(name)}: {name}, {nameof(thumbnailUrl)}: {thumbnailUrl}, {nameof(description)}: {description}, {nameof(achievementPoints)}: {achievementPoints}";
        }
    }
}