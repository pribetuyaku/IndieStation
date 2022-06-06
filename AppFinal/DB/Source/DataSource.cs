using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json.Linq;

namespace AppFinal.DB.Source
{
    /// <summary>
    /// Data Source to be hashed and hidden
    /// </summary>
    public class DataSource
    {
        private static readonly DataSource Instance = new DataSource();

        private readonly HttpClient _httpClient;
        private readonly string path = "http://35.204.176.180:8080/";

        /// <summary>
        /// Instantiate the data source
        /// </summary>
        private DataSource()
        {
            this._httpClient = new HttpClient();
        }

        /// <summary>
        /// Check connection with DB and shows collections
        /// </summary>
        /// <returns>success of connection</returns>
        public bool CheckConnection()
        {
            return false;
            //try
            //{
            //    this._httpClient.RunCommandAsync((Command<BsonDocument>)"{ping: 1}").Wait();
            //    ShowCollections();
            //    return true;
            //}
            //catch (Exception)
            //{
            //    return false;
            //}
        }

        /// <summary>
        /// Show collections in DB
        /// </summary>
        private void ShowCollections()
        {
            //foreach (var collection in this._httpClient.ListCollectionNames().ToList())
            //{
            //    Console.WriteLine(collection);
            //}
        }

        /// <summary>
        /// Find all documents in collection
        /// </summary>
        /// <param name="collection">collection name</param>
        /// <returns>List of BsonDocuments</returns>
        public async Task<List<BsonDocument>> FindAll(string collection)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, this.path + "find/" + collection);
            //request.Content = new StringContent("{\"region\": \"Taubate\"}", Encoding.UTF8, "application/json");

            var responseMessage = await this._httpClient.SendAsync(request);

            var response = await responseMessage.Content.ReadAsStringAsync();
            JsonDocument json = JsonDocument.Parse(response);

            var array = BsonSerializer.Deserialize<BsonArray>(response);
            List<BsonDocument> documents = new List<BsonDocument>();
            foreach (var r in array)
            {
                documents.Add(r.AsBsonDocument);
            }
            return documents;

        }


        /// <summary>
        /// Find all documents in collection that match the given filter
        /// </summary>
        /// <param name="collection">collection name</param>
        /// <param name="filter">FilterDefinition</param>
        /// <returns>List of BsonDocument that match the filter</returns>
        public async Task<List<BsonDocument>> FindMany(string collection, string filter)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, this.path + "find/" + collection);
            request.Content = new StringContent(filter, Encoding.UTF8, "application/json");

            var responseMessage = await this._httpClient.SendAsync(request);

            var response = await responseMessage.Content.ReadAsStringAsync();
            // JsonDocument json = JsonDocument.Parse(response);

            var array = BsonSerializer.Deserialize<BsonArray>(response);
            List<BsonDocument> documents = new List<BsonDocument>();
            foreach (var r in array)
            {
                documents.Add(r.AsBsonDocument);
            }
            return documents;
        }


        /// <summary>
        /// Finds one document in collection using a filter definition
        /// </summary>
        /// <param name="collection">collection name</param>
        /// <param name="filter">FilterDefinition</param>
        /// <returns>single BsonDocument</returns>
        public async Task<BsonDocument> FindOne(string collection, string filter)
        {


            var request = new HttpRequestMessage(HttpMethod.Post, this.path + "find/" + collection);
            request.Content = new StringContent(filter, Encoding.UTF8, "application/json");
            Console.WriteLine("request: " + request.ToJson());
            var responseMessage = await this._httpClient.SendAsync(request);
            Console.WriteLine("Response Message " + responseMessage);
            var response = await responseMessage.Content.ReadAsStringAsync();
            Console.WriteLine("response: " + response);
            JsonDocument json = JsonDocument.Parse(response);

            var obj = BsonSerializer.Deserialize<BsonArray>(response);
            try
            {
                return obj.ToList()[0].AsBsonDocument;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Finds one document in collection using a filter definition
        /// </summary>
        /// <param name="collection">collection name</param>
        /// <param name="filter">FilterDefinition</param>
        /// <returns>single BsonDocument</returns>
        public async Task<BsonDocument> FindOne(string collection, ObjectId objId)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, this.path + "find/" + collection + "/" + objId);
            request.Content = new StringContent("{}", Encoding.UTF8, "application/json");
            //Console.WriteLine("request: " + request.ToJson());
            var responseMessage = await this._httpClient.SendAsync(request);

            var response = await responseMessage.Content.ReadAsStringAsync();
            //Console.WriteLine("response: " + response);
            JsonDocument json = JsonDocument.Parse(response);

            var obj = BsonSerializer.Deserialize<BsonArray>(response);
            try
            {
                return obj.ToList()[0].AsBsonDocument;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Creates a new document in a collection
        /// </summary>
        /// <param name="collection">collection name</param>
        /// <param name="document">BsonDocument to be added</param>
        /// <returns>success of insertion</returns>
        public async Task<bool> InsertOne(string collection, string document)
        {
            Console.WriteLine(document);
            Console.WriteLine("being sent: " + JObject.Parse(document));
            var request = new HttpRequestMessage(HttpMethod.Post, this.path + "insert/" + collection);
            request.Content = new StringContent(JObject.Parse(document).ToString(), Encoding.UTF8, "application/json");
            Console.WriteLine("request: " + request.ToJson());
            var responseMessage = await this._httpClient.SendAsync(request);

            var response = await responseMessage.Content.ReadAsStringAsync();
            Console.WriteLine("response: " + response);
            return responseMessage.IsSuccessStatusCode;
        }

        /// <summary>
        /// Deletes a document from a collection
        /// </summary>
        /// <param name="collection">collection name</param>
        /// <param name="filter">filter to check what document to be deleted</param>
        /// <returns>success of deletion</returns>
        public async Task<bool> DeleteOne(string collection, string filter)
        {
            Console.WriteLine(filter);
            var request = new HttpRequestMessage(HttpMethod.Delete, this.path + collection);
            request.Content = new StringContent(filter, Encoding.UTF8, "application/json");
            Console.WriteLine("request: " + request.ToJson());
            var responseMessage = await this._httpClient.SendAsync(request);

            var response = await responseMessage.Content.ReadAsStringAsync();
            Console.WriteLine("response: " + response);
            return responseMessage.IsSuccessStatusCode;
        }

        /// <summary>
        /// Updates a document in a collection
        /// </summary>
        /// <param name="collection">collection name</param>
        /// <param name="filter">FilterDefinition</param>
        /// <param name="update">UpdateDefinition</param>
        /// <returns>success of update</returns>
        public async Task<bool> UpdateOne(string collection, string id, string update)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, this.path + "update/" + collection + "/" + id);
            request.Content = new StringContent(update, Encoding.UTF8, "application/json");
            Console.WriteLine("request: " + request.ToJson());
            var responseMessage = await this._httpClient.SendAsync(request);

            var response = await responseMessage.Content.ReadAsStringAsync();
            Console.WriteLine("response: " + response);
            return responseMessage.IsSuccessStatusCode;
            //try
            //{
            //    var res = this._httpClient.GetCollection<BsonDocument>(collection).UpdateOne(filter, update);
            //    return true;
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //    return false;
            //}
        }

        /// <summary>
        /// Get Instance of DataSource
        /// </summary>
        /// <returns>Instance of DataSource</returns>
        public static DataSource GetInstance()
        {
            return Instance;
        }

        ///// <summary>
        ///// Disposes of the current connection
        ///// </summary>
        //public void DisposeConnection()
        //{
        //    this._httpClient.Client.Cluster.Dispose();
        //}
    }
}