using System;
using System.Threading.Tasks;
using AppFinal.DB.AccessClasses;
using MongoDB.Bson;

namespace AppFinal.Models
{
    /// <summary>
    /// messages collection class
    /// </summary>
    public class Message
    {
        public string id { get; private set; }
        public string sender { get; private set; }
        public string receiver { get; private set; }
        public string content { get; private set; }
        public string mediaUrl { get; private set; }
        public string date { get; private set; }
        public MessageStatus status { get; set; }

        /// <summary>
        /// Instantiate an existing message
        /// </summary>
        /// <param name="id">message id</param>
        /// <param name="sender">sender id</param>
        /// <param name="receiver">receiver id</param>
        /// <param name="content">content of message</param>
        /// <param name="mediaUrl">media url</param>
        /// <param name="date">date of message in the format dd/MM/yyy hh:mm:ss</param>
        /// <param name="status">MessageStatus string</param>
        public Message(string id, string sender, string receiver, string content, string mediaUrl, string date, string status)
        {
            this.id = id;
            this.sender = sender;
            this.receiver = receiver;
            this.content = content;
            this.mediaUrl = mediaUrl;
            this.date = date;
            this.status = MessageStatusGetter.GetMessageStatus(status);
        }

        /// <summary>
        /// Instantiate a new message
        /// </summary>
        /// <param name="sender">sender id</param>
        /// <param name="receiver">receiver id</param>
        /// <param name="content">message content</param>
        /// <param name="mediaUrl">message media url</param>
        public Message(string sender, string receiver, string content, string mediaUrl)
        {
            this.id = ObjectId.GenerateNewId().ToString();
            this.sender = sender;
            this.receiver = receiver;
            this.content = content;
            this.mediaUrl = mediaUrl;
            this.date = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            this.status = MessageStatus.SENT;
        }

        /// <summary>
        /// Change the status of the message
        /// </summary>
        /// <param name="newStatus">MessageStatus</param>
        /// <returns>success of update</returns>
        public async Task<bool> ChangeStatus(MessageStatus newStatus)
        {
            this.status = newStatus;
            return await Update();
        }

        /// <summary>
        /// Delete message
        /// </summary>
        /// <returns>success of deletion</returns>
        public async Task<bool> DeleteMessage()
        {
            return await new MessageDbAccess().DeleteOne(this.id);
        }

        /// <summary>
        /// Update current object document in the DB
        /// </summary>
        /// <returns>success of update</returns>
        private async Task<bool> Update()
        {
            try
            {
                return await new MessageDbAccess().UpdateOne(this, this.id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public override string ToString()
        {
            return $"{nameof(id)}: {id}, {nameof(sender)}: {sender}, {nameof(receiver)}: {receiver}, {nameof(content)}: {content}, {nameof(mediaUrl)}: {mediaUrl}, {nameof(date)}: {date}, {nameof(status)}: {status.ToString()}";
        }

        public string GetBsonDocument()
        {
            var bsonDoc = "{" +
                "\"_id\": \"" + this.id + "\"," +
                "\"by\": \"" + this.sender + "\"," +
                "\"to\": \"" + this.receiver + "\"," +
                "\"content\": \"" + this.content + "\"," +
                "\"date\": \"" + this.date + "\"," +
                "\"messageStatus\": \"" + MessageStatusGetter.GetMessageStatus(this.status) + "\"}";

            if (this.mediaUrl != null)
            {
                bsonDoc = bsonDoc.Substring(0, bsonDoc.Length - 1);
                bsonDoc += ", \"mediaUrl\": \"" + this.mediaUrl + "\"}";
            }

            return bsonDoc;
        }

    }
}