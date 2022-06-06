using System;
using System.Collections.Generic;
using AppFinal.Models;
using MongoDB.Bson;

namespace AppFinal.DB.AccessClasses
{
    /// <summary>
    /// Public access to posts collection
    /// </summary>
    public class PostDbAccess : MainDbAbstract<Post>
    {
        /// <summary>
        /// Instantiate a new posts access
        /// </summary>
        public PostDbAccess()
        {
            this.CollectionName = "posts";
        }

        protected override Post GetObjectFromBsonDocument(BsonDocument document)
        {
            try
            {
                var postId = document["_id"].ToString();
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

                return new Post(postId, userId, date, content, likes, mediaUrl, comments);
            }
            catch (Exception)
            {
                return null;
            }
        }

        protected override string GetUpdateDefinition(Post post)
        {
            var update = "{\"$set\": {\"content\": \"" + post.Content + "\"," +
                         "\"media\": \"" + post.MediaUrl + "\",";

            var likesArray = GetStringFromLinkedList(post.Likes);

            var commentsArray = GetStringFromLinkedList(post.Comments);

            update += "\"likes\": " + likesArray + ",";
            update += "\"comments\": " + commentsArray + "}}";

            return update;
        }

        protected override string GetBsonDocument(Post obj)
        {
            return obj.GetBsonDocument();
        }
    }
}