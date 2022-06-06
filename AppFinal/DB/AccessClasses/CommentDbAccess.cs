using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppFinal.Models;
using MongoDB.Bson;

namespace AppFinal.DB.AccessClasses
{
    /// <summary>
    /// Public access to comments collection
    /// </summary>
    public class CommentDbAccess : MainDbAbstract<Comment>
    {
        /// <summary>
        /// Instantiate a new comments access
        /// </summary>
        public CommentDbAccess()
        {
            this.CollectionName = "comments";
        }

        /// <summary>
        /// Get all comments of a given post
        /// </summary>
        /// <param name="postId">id of post to find comments for</param>
        /// <returns>LinkedList of comments for a given post</returns>
        public async Task<LinkedList<Comment>> GetPostComments(string postId)
        {
            var comments = new LinkedList<Comment>();

            var filter = "{\"postId\": \"" + postId + "\"}";

            var bsonComments = await Db.FindMany(this.CollectionName, filter);

            foreach (var bsonComment in bsonComments)
            {
                comments.AddLast(GetCommentFromBsonDocument(bsonComment));
            }

            return comments;
        }

        protected override Comment GetObjectFromBsonDocument(BsonDocument document)
        {
            try
            {
                var commentId = document["_id"].ToString();
                var postId = document["postId"].ToString();
                var userId = document["userId"].AsString;
                var date = document["date"].AsString;
                var content = document["content"].AsString;
                string mediaUrl = null;
                if (document.Contains("mediaUrl"))
                {
                    mediaUrl = document["mediaUrl"].AsString;
                }

                var likes = new LinkedList<string>();
                foreach (var like in document["likes"].AsBsonArray)
                {
                    likes.AddLast(like.AsString);
                }

                var comments = new LinkedList<string>();
                foreach (var comment in document["comments"].AsBsonArray)
                {
                    comments.AddLast(comment.AsString);
                }

                return new Comment(commentId, postId, userId, date, content, likes, mediaUrl, comments);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public static Comment GetCommentFromBsonDocument(BsonDocument document)
        {
            try
            {
                var commentId = document["_id"].ToString();
                var postId = document["postId"].ToString();
                var userId = document["userId"].AsString;
                var date = document["date"].AsString;
                var content = document["content"].AsString;
                string mediaUrl = null;
                if (document.Contains("mediaUrl"))
                {
                    mediaUrl = document["mediaUrl"].AsString;
                }

                var likes = new LinkedList<string>();
                foreach (var like in document["likes"].AsBsonArray)
                {
                    likes.AddLast(like.AsString);
                }

                var comments = new LinkedList<string>();
                foreach (var comment in document["comments"].AsBsonArray)
                {
                    comments.AddLast(comment.AsString);
                }

                return new Comment(commentId, postId, userId, date, content, likes, mediaUrl, comments);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        protected override string GetUpdateDefinition(Comment obj)
        {
            var update = "{\"$set\": {\"content\": \"" + obj.Content + "\"," +
                         "\"media\": \"" + obj.MediaUrl + "\",";

            var likesArray = new CommentDbAccess().GetStringFromLinkedList(obj.Likes);


            var commentsArray = new CommentDbAccess().GetStringFromLinkedList(obj.Comments);

            update += "\"likes\": " + likesArray + ",";
            update += "\"comments\": " + commentsArray + "}}";

            return update;
        }

        protected override string GetBsonDocument(Comment obj)
        {
            return obj.GetBsonDocument();
        }
    }
}