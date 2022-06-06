using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppFinal.DB.Source;
using AppFinal.Interfaces;
using MongoDB.Bson;

namespace AppFinal.DB.AccessClasses
{
    /// <summary>
    /// Abstract class implementing Main DB connection interface
    /// </summary>
    /// <typeparam name="T">Generic type</typeparam>
    public abstract class MainDbAbstract<T> : IMainDbInterface<T> where T : class
    {
        // Instance of DataSource to connect to db
        protected static readonly DataSource Db = DataSource.GetInstance();

        // Each access class for a collection has its own collection name
        protected string CollectionName { get; set; }

        public async Task<bool> DeleteOne(string objId)
        {
            return await Db.DeleteOne(this.CollectionName, "{\"id\": \"" + objId + "\"}");
        }

        public async Task<LinkedList<T>> FindMany()
        {
            var bsonList = await Db.FindAll(this.CollectionName);
            return GetLinkedListFromBsonList(bsonList);
        }

        public async Task<LinkedList<T>> FindMany(Dictionary<string, string> filters)
        {
            var objectsBsonDocument = await Db.FindMany(this.CollectionName, GetJsonFromDictionary(filters));
            return GetLinkedListFromBsonList(objectsBsonDocument);
        }

        public async Task<T> FindOne(string objId)
        {
            var objectBson = await Db.FindOne(this.CollectionName, new ObjectId(objId));
            return GetObjectFromBsonDocument(objectBson);
        }

        public async Task<T> FindOne(Dictionary<string, string> filters)
        {
            var stringFilter = GetJsonFromDictionary(filters);
            var objBson = await Db.FindOne(this.CollectionName, stringFilter);
            return GetObjectFromBsonDocument(objBson);
        }

        /// <summary>
        /// Get FilterDefinition from a Dictionary
        /// </summary>
        /// <param name="fields">Dictionary with field name as key and value to be checked as value</param>
        /// <returns>FilterDefinition with the values in the filters</returns>
        protected string GetJsonFromDictionary(Dictionary<string, string> fields)
        {
            var filter = "{";
            foreach (var keyValuePair in fields)
            {
                Console.WriteLine(keyValuePair);
                var lineFilter = "\"" + keyValuePair.Key + "\": \"" + keyValuePair.Value + "\",";
                filter += lineFilter;
            }

            if (filter.Length > 1) filter = filter.Substring(0, filter.Length - 1);

            return filter + "}";

        }

        /// <summary>
        /// Transform a BsonDocument List to a LinkedList of object
        /// </summary>
        /// <param name="bsonList">List of BsonDocument from DB</param>
        /// <returns>LinkedList of objects</returns>
        protected LinkedList<T> GetLinkedListFromBsonList(List<BsonDocument> bsonList)
        {
            var objects = new LinkedList<T>();
            foreach (var obj in bsonList)
            {
                objects.AddLast(GetObjectFromBsonDocument(obj));
            }

            return objects;
        }

        /// <summary>
        /// Transform a BsonDocument into an object of the given class
        /// </summary>
        /// <param name="document">BsonDocument</param>
        /// <returns>Object of the class</returns>
        protected abstract T GetObjectFromBsonDocument(BsonDocument document);

        /// <summary>
        /// Generate an UpdateDefinition from an object of a given class in order to update a document in the database
        /// </summary>
        /// <param name="obj">Object of a given class</param>
        /// <returns>UpdateDefinition</returns>
        protected abstract string GetUpdateDefinition(T obj);

        public async Task<bool> InsertOne(T obj)
        {
            return await Db.InsertOne(this.CollectionName, GetBsonDocument(obj));
        }

        public async Task<bool> UpdateOne(T obj, string id)
        {
            return await Db.UpdateOne(this.CollectionName, id, GetUpdateDefinition(obj));
        }

        /// <summary>
        /// Get BsonDocument from an object of a given class
        /// </summary>
        /// <param name="obj">Object of a given class</param>
        /// <returns>BsonDocument of a given object</returns>
        protected abstract string GetBsonDocument(T obj);

        public string GetStringFromLinkedList(LinkedList<string> list)
        {
            var s = "[";
            foreach (var el in list)
            {
                s += "\"" + el + "\",";
            }

            if (s.Length > 1) s = s.Substring(0, s.Length - 1);
            s += "]";
            return s;
        }
    }
}