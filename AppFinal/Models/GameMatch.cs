using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppFinal.DB.AccessClasses;
using MongoDB.Bson;

namespace AppFinal.Models
{
    /// <summary>
    /// gameMatch collection class
    /// </summary>
    public class GameMatch
    {
        public string id { get; private set; }
        public string gameId { get; private set; }
        public string date { get; private set; }
        public LinkedList<string> userIds { get; private set; }
        public string winnerId { get; private set; }

        /// <summary>
        /// Instantiate an existing GameMatch object
        /// </summary>
        /// <param name="id">gameMatch id</param>
        /// <param name="gameId">game played id</param>
        /// <param name="date">date of game</param>
        /// <param name="userIds">ids of all users that played in the game</param>
        /// <param name="winnerId">id of winner of the game</param>
        public GameMatch(string id, string gameId, string date, LinkedList<string> userIds, string winnerId)
        {
            this.id = id;
            this.gameId = gameId;
            this.date = date;
            this.userIds = userIds;
            this.winnerId = winnerId;
        }

        /// <summary>
        /// Instantiate a new GameMatch object
        /// </summary>
        /// <param name="gameId">game played id</param>
        /// <param name="userIds">ids of all users that played in the game</param>
        public GameMatch(string gameId, LinkedList<string> userIds)
        {
            this.id = ObjectId.GenerateNewId().ToString();
            this.gameId = gameId;
            this.date = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            this.userIds = userIds;
        }

        /// <summary>
        /// Defines the winner of the game match
        /// </summary>
        /// <param name="winId">winner id</param>
        /// <returns>Success of update</returns>
        public async Task<bool> SetWinner(string winId)
        {
            this.winnerId = winId;
            return await new GameMatchDbAccess().UpdateOne(this, this.id);
        }

        public string GetBsonDocument()
        {

            var userIdsArray = new GameMatchDbAccess().GetStringFromLinkedList(this.userIds);

            var bsonDoc = "{" +
                "\"_id\": \"" + this.id + "\"," +
                "\"gameId\": \"" + this.gameId + "\"," +
                "\"date\": \"" + this.date + "\"," +
                "\"userIds\": \"" + userIdsArray + "\"," +
                "\"winnerId\": \"" + this.winnerId + "\"}";

            return bsonDoc;
        }

        public override string ToString()
        {
            return $"{nameof(id)}: {id}, {nameof(gameId)}: {gameId}, {nameof(date)}: {date}, {nameof(userIds)}: {userIds.Count}, {nameof(winnerId)}: {winnerId}";
        }
    }
}