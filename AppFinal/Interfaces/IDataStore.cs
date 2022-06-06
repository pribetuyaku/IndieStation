// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using System.Text;
//
// namespace AppFinal.Interfaces
// {
//     public interface IDataStore<T>
//     {
//         Task<bool> AddFriendAsync(T friend);
//         Task<bool> UpdateFriendAsync(T friend);
//         Task<bool> DeleteFriendAsync(string id);
//         Task<T> GetFriendAsync(string id);
//         Task<IEnumerable<T>> GetFriendAsync(bool forceRefresh = false);
//     }
// }