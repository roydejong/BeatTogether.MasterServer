﻿using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeatTogether.MasterServer.Data.Abstractions.Repositories;
using BeatTogether.MasterServer.Data.Entities;

namespace BeatTogether.MasterServer.Data.Implementations.Repositories
{
    public class MemoryServerRepository : IServerRepository
    {
        private static ConcurrentDictionary<string, Server> _servers = new();
        private static ConcurrentDictionary<string, Server> _serversByCode = new();
        private static ConcurrentDictionary<string, Server> _quickPlayServers = new();

        public Task<Server> GetServer(string secret)
        {
            if (!_servers.TryGetValue(secret, out var server))
                return Task.FromResult<Server>(null);
            return Task.FromResult(server);
        }

        public Task<Server> GetServerByCode(string code)
        {
            if (!_serversByCode.TryGetValue(code, out var server))
                return Task.FromResult<Server>(null);
            return Task.FromResult(server);
        }

        public Task<Server> GetAvailableQuickPlayServer()
        {
            return Task.FromResult(_quickPlayServers.Values.FirstOrDefault(server =>
                server.IsQuickPlay && server.CurrentPlayerCount < server.MaximumPlayerCount));
        }

        public Task<bool> AddServer(Server server)
        {
            if (!_servers.TryAdd(server.Secret, server))
                return Task.FromResult(false);
            _serversByCode[server.Code] = server;
            if (server.IsQuickPlay)
                _quickPlayServers.TryAdd(server.Secret, server);
            return Task.FromResult(true);
        }

        public Task<bool> RemoveServer(string secret)
        {
            if (!_servers.TryRemove(secret, out var server))
                return Task.FromResult(false);
            _serversByCode.TryRemove(server.Code, out _);
            if (server.IsQuickPlay)
                _quickPlayServers.Remove(server.Secret, out _);
            return Task.FromResult(true);
        }

        public Task<bool> IncrementCurrentPlayerCount(string secret)
        {
            if (!_servers.TryGetValue(secret, out var server))
                return Task.FromResult(false);
            server.CurrentPlayerCount++;
            return Task.FromResult(true);
        }

        public void UpdateCurrentPlayerCount(string secret, int currentPlayerCount)
        {
            if (!_servers.TryGetValue(secret, out var server))
                return;
            server.CurrentPlayerCount = currentPlayerCount;
        }
    }
}
