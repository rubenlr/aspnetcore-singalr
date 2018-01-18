using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;

namespace SiganlR.Core
{
    class UserDataStore
    {
        private readonly IDictionary<string, UserData> _userData = new ConcurrentDictionary<string, UserData>();

        public void Store(UserData user)
        {
            if (_userData.ContainsKey(user.Id))
                _userData[user.Id] = user;
            else
                _userData.Add(user.Id, user);
        }

        public string GetName(string id) 
            => _userData.ContainsKey(id) ? _userData[id].Nome : "<Desconhecido>";

        public string GetName(HubCallerContext context) 
            => context.ConnectionId;
    }
}