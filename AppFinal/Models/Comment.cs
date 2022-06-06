using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppFinal.DB.AccessClasses;
using MongoDB.Bson;

namespace AppFinal.Models
{
    /// <summary>
    /// comments collection class
    /// </summary>
    public class Comment : Post
    {
        protected new static readonly CommentDbAccess DbAccess = new CommentDbAccess();
        public string PostId { get; private set; }

        /// <summary>
        /// Instantiate an existing Comment object
        /// </summary>
        /// <param name="commentId">id of comment</param>
        /// <param name="postId">id of post/comment this comment is directed to</param>
        /// <param name="userId">id of user who posted the comment</param>
        /// <param name="date">date of when comment was posted in the format dd/MM/yyy hh:mm:ss</param>
        /// <param name="content">content of the comment</param>
        /// <param name="likes">list of users who liked this comment</param>
        /// <param name="mediaUrl">url for the media within the comment</param>
        /// <param name="comments">list of ids of comments of this comment</param>
        public Comment(string commentId, string postId, string userId, string date, string content, LinkedList<string> likes, string mediaUrl, LinkedList<string> comments) : base(postId, userId, date, content, likes, mediaUrl, comments)
        {
            this.Id = commentId;
            this.PostId = postId;
        }

        /// <summary>
        /// Instantiate a new Comment object
        /// </summary>
        /// <param name="postId">id of post/comment this comment is directed to</param>
        /// <param name="userId">id of user who posted the comment</param>
        /// <param name="content">content of the comment</param>
        /// <param name="mediaUrl">url for the media within the comment</param>
        public Comment(string postId, string userId, string content, string mediaUrl) : base(userId, content, mediaUrl)
        {
            this.Id = ObjectId.GenerateNewId().ToString();
            this.PostId = postId;
        }

        /// <summary>
        /// Create new comment for the current comment
        /// </summary>
        /// <param name="newCommentUserId"></param>
        /// <param name="newCommentContent"></param>
        /// <param name="newCommentMediaUrl"></param>
        /// <returns>new comment object or null if error</returns>
        public new async Task<Comment> NewComment(string newCommentUserId, string newCommentContent, string newCommentMediaUrl)
        {
            var newComment = new Comment(this.Id, newCommentUserId, newCommentContent, newCommentMediaUrl);
            this.Comments.AddFirst(newComment.Id);
            var success = true;
            try
            {
                success &= await DbAccess.InsertOne(newComment);
                success &= await UpdateDb();
                return success ? newComment : null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        
        /// <summary>
        /// Create Bson Document based on object
        /// </summary>
        /// <returns>Bson Document</returns>
        public new string GetBsonDocument()
        {
            var likesArray = DbAccess.GetStringFromLinkedList(this.Likes);

            var commentsArray = DbAccess.GetStringFromLinkedList(this.Comments);

            var bsonDoc = "{" +
                "\"_id\": \"" + this.Id + "\"," +
                "\"postId\": \"" + this.PostId + "\"," +
                "\"userId\": \"" + this.UserId + "\"," +
                "\"date\": \"" + this.Date + "\"," +
                "\"content\": \"" + this.Content + "\"," +
                "\"likes\":" + likesArray + "," +
                "\"comments\": " + commentsArray + "}";

            if (this.MediaUrl != null)
            {
                bsonDoc = bsonDoc.Substring(0, bsonDoc.Length - 1);
                bsonDoc += ", \"media\": \"" + this.MediaUrl + "\"}";
            }

            return bsonDoc;
        }

        /// <summary>
        /// Save changes on the DB
        /// </summary>
        /// <returns>Success of update</returns>
        private async Task<bool> UpdateDb()
        {
            try
            {
                return await DbAccess.UpdateOne(this, this.Id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public override string ToString()
        {
            var likeString = "[";
            if (this.Likes.Count > 0)
            {
                foreach (var like in this.Likes)
                {
                    likeString += like + ", ";
                }

                likeString = likeString[..^2] + ']';
            }
            else
            {
                likeString += ']';
            }

            var commentString = "[";
            if (this.Comments.Count > 0)
            {

                foreach (var comment in this.Comments)
                {
                    commentString += comment + ", ";
                }

                commentString = commentString[..^2] + ']';
            }
            else
            {
                commentString += ']';
            }

            return $"{nameof(Id)}: {Id}, {nameof(PostId)}: {PostId}, {nameof(UserId)}: {UserId}, {nameof(Date)}: {Date}, {nameof(Content)}: {Content}, {nameof(Likes)} ({Likes.Count}): {likeString}, {nameof(MediaUrl)}: {MediaUrl}, {nameof(Comments)}({Comments.Count}): {commentString}";
        }
    }
}