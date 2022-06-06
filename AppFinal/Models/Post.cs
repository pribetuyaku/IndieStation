using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppFinal.DB.AccessClasses;
using MongoDB.Bson;

namespace AppFinal.Models
{
    /// <summary>
    /// posts collection class
    /// </summary>
    public class Post
    {
        protected static PostDbAccess DbAccess = new PostDbAccess();
        private static readonly CommentDbAccess CommentAccess = new CommentDbAccess();

        public string Id { get; protected set; }
        public string UserId { get; protected set; }
        public string Date { get; protected set; }
        public string Content { get; protected set; }
        public LinkedList<string> Likes { get; protected set; }
        public string MediaUrl { get; protected set; }
        public LinkedList<string> Comments { get; protected set; }

        /// <summary>
        /// Instantiate an existing Post
        /// </summary>
        /// <param name="postId">post id</param>
        /// <param name="userId">id of user who posted</param>
        /// <param name="date">date of post in the format dd/MM/yyy hh:mm:ss</param>
        /// <param name="content">content of post</param>
        /// <param name="likes">list of users who liked the comment</param>
        /// <param name="mediaUrl">url for media of comment</param>
        /// <param name="comments">list of id of comments of this comment</param>
        public Post(string postId, string userId, string date, string content, LinkedList<string> likes, string mediaUrl, LinkedList<string> comments)
        {
            this.Id = postId;
            this.UserId = userId;
            this.Date = date;
            this.Content = content;
            this.Likes = likes;
            this.MediaUrl = mediaUrl;
            this.Comments = comments;
        }

        /// <summary>
        /// Instantiate a new Post
        /// </summary>
        /// <param name="userId">id of user posting</param>
        /// <param name="content">content of post</param>
        /// <param name="mediaUrl">url of media in the post</param>
        public Post(string userId, string content, string mediaUrl)
        {
            this.Id = ObjectId.GenerateNewId().ToString();
            this.UserId = userId;
            this.Date = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            this.Content = content;
            this.Likes = new LinkedList<string>();
            this.MediaUrl = mediaUrl;
            this.Comments = new LinkedList<string>();
        }

        /// <summary>
        /// Add a user id to likes in the post and update DB
        /// </summary>
        /// <param name="userId">id to add</param>
        /// <returns>success of update</returns>
        public async Task<bool> Like(string userId)
        {
            this.Likes.AddLast(userId);
            return await UpdateDb();
        }

        /// <summary>
        /// Remove a user id from likes in the post and update DB
        /// </summary>
        /// <param name="userId">id to remove</param>
        /// <returns>success of update</returns>
        public async Task<bool> Unlike(string userId)
        {
            this.Likes.Remove(userId);
            return await UpdateDb();
        }

        public async Task<bool> Delete()
        {
            return await DbAccess.DeleteOne(this.Id);
        }

        /// <summary>
        /// Get LinkedList of Comment objects from comment ids
        /// </summary>
        /// <returns>LinkedList of Comment objects</returns>
        public async Task<LinkedList<Comment>> GetComments()
        {
            return await CommentAccess.GetPostComments(this.Id);
        }

        /// <summary>
        /// Insert comment to current post
        /// </summary>
        /// <param name="userId">id of commenter</param>
        /// <param name="content">content of comment</param>
        /// <param name="mediaUrl">url of media of comment</param>
        /// <returns>object comment or null if error</returns>
        public async Task<Comment> NewComment(string userId, string content, string mediaUrl)
        {
            var newComment = new Comment(this.Id, userId, content, mediaUrl);
            this.Comments.AddFirst(newComment.Id);
            bool success = true;
            try
            {
                success &= await CommentAccess.InsertOne(newComment);
                success &= await UpdateDb();
                if (success)
                {
                    return newComment;
                }
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        /// <summary>
        /// Change content of post
        /// </summary>
        /// <param name="content"></param>
        /// <returns>success of update</returns>
        public async Task<bool> EditPost(string content)
        {
            this.Content = content;
            return await UpdateDb();

        }

        /// <summary>
        /// Saves current state of object in the DB
        /// </summary>
        /// <returns>success of update</returns>
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

                likeString = likeString.Substring(0, likeString.Length - 2) + ']';
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

                commentString = commentString.Substring(0, commentString.Length - 2) + ']';
            }
            else
            {
                commentString += ']';
            }

            return $"{nameof(Id)}: {Id}, {nameof(UserId)}: {UserId}, {nameof(Date)}: {Date}, {nameof(Content)}: {Content}, {nameof(Likes)}({Likes.Count}): {likeString}, {nameof(MediaUrl)}: {MediaUrl}, {nameof(Comments)}({Comments.Count}): {commentString}";
        }

        public string GetBsonDocument()
        {
            var likesArray = DbAccess.GetStringFromLinkedList(this.Likes);

            var commentsArray = DbAccess.GetStringFromLinkedList(this.Comments);

            var bsonDoc = "{" +
                "\"_id\": \"" + this.Id + "\"," +
                "\"userId\": \"" + this.UserId + "\"," +
                "\"date\": \"" + this.Date + "\"," +
                "\"content\": \"" + this.Content + "\"," +
                "\"likes\":" + likesArray + "," +
                "\"comments\":" + commentsArray + "}";

            if (this.MediaUrl != null)
            {
                bsonDoc = bsonDoc.Substring(0, bsonDoc.Length - 1);
                bsonDoc += ", \"media\": \"" + this.MediaUrl + "\"}";
            }

            return bsonDoc;
        }

        public static async Task<LinkedList<Post>> GetAllPosts()
        {
            return await DbAccess.FindMany();
        }
    }
}