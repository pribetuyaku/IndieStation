// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using AppFinal.Models;
// using AppFinal.Interfaces;
//
//
// namespace AppFinal.Interfaces
// {
//     public class MockDataStore : IDataStore<Friend>
//     {
//         readonly List<Friend> friends;
//
//         public MockDataStore()
//         {
//             friends = new List<Friend>()
//             {
//                 new Friend { Id = Guid.NewGuid().ToString(), Name = "Friend 1", Description="2 victories" },
//                 new Friend { Id = Guid.NewGuid().ToString(), Name = "Friend 2", Description="5 victories" },
//                 new Friend { Id = Guid.NewGuid().ToString(), Name = "Friend 3", Description="12 victories" },
//                 new Friend { Id = Guid.NewGuid().ToString(), Name = "Friend 4", Description="15 victories" },
//                 new Friend { Id = Guid.NewGuid().ToString(), Name = "Friend 5", Description="6 victories" },
//                 new Friend { Id = Guid.NewGuid().ToString(), Name = "Friend 6", Description="9 victories" }
//             };
//         }
//
//         public async Task<bool> AddFriendAsync(Friend friend)
//         {
//             friends.Add(friend);
//
//             return await Task.FromResult(true);
//         }
//
//         public async Task<bool> UpdateFriendAsync(Friend friend)
//         {
//             var oldItem = friends.Where((Friend arg) => arg.Id == friend.Id).FirstOrDefault();
//             friends.Remove(oldItem);
//             friends.Add(friend);
//
//             return await Task.FromResult(true);
//         }
//
//         public async Task<bool> DeleteFriendAsync(string id)
//         {
//             var oldItem = friends.Where((Friend arg) => arg.Id == id).FirstOrDefault();
//             friends.Remove(oldItem);
//
//             return await Task.FromResult(true);
//         }
//
//         public async Task<Friend> GetFriendAsync(string id)
//         {
//             return await Task.FromResult(friends.FirstOrDefault(s => s.Id == id));
//         }
//
//         public async Task<IEnumerable<Friend>> GetFriendAsync(bool forceRefresh = false)
//         {
//             return await Task.FromResult(friends);
//         }
//     }
// }