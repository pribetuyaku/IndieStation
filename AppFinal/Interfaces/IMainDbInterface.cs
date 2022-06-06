using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppFinal.Interfaces
{
    public interface IMainDbInterface<T> where T : class
    {
        /// <summary>
        /// Get all documents in a given collection
        /// </summary>
        /// <returns>LinkedList of documents as objects of their class</returns>
        public Task<LinkedList<T>> FindMany();

        /// <summary>
        /// Get all documents in a given collection that correspond to the filters
        /// </summary>
        /// <param name="filters">Dictionary with field name as key and value to be checked as value</param>
        /// <returns>LinkedList of documents that correspond to filters as objects of their class</returns>
        public Task<LinkedList<T>> FindMany(Dictionary<string, string> filters);

        /// <summary>
        /// Get a document
        /// </summary>
        /// <param name="objId">id of document as string</param>
        /// <returns>Single object of <c>T</c> </returns>
        public Task<T> FindOne(string objId);

        /// <summary>
        /// Get a document based on filters
        /// </summary>
        /// <param name="filters">Dictionary with field name as key and value to be checked as value</param>
        /// <returns>Single object</returns>
        public Task<T> FindOne(Dictionary<string, string> filters);

        /// <summary>
        /// Updates a document in the DB with its current attributes
        /// </summary>
        /// <param name="obj">Updated object of a given class</param>
        /// <param name="objId">Id of document to be updated</param>
        /// <returns>Success of update</returns>
        public Task<bool> UpdateOne(T obj, string objId);

        /// <summary>
        /// Delete a document from a collection
        /// </summary>
        /// <param name="objId">document id as string</param>
        /// <returns>Success of deletion</returns>
        public Task<bool> DeleteOne(string objId);

        /// <summary>
        /// Creates a new document in the DB from an object of a given class
        /// </summary>
        /// <param name="obj">Object to create document in DB for</param>
        /// <returns>Success of insertion</returns>
        public Task<bool> InsertOne(T obj);

    }
}