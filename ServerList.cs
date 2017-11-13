using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

/// <summary>
/// Manages all informations for registered servers.
/// </summary>
public class ServerList {

    int _serverTimeoutMinutes;

    Dictionary<int, ServerDetails> _server;
    public Dictionary<int, ServerDetails> server {
        get { return _server; }
    }

    /// <param name="serverTimeoutMinutes">Servers will be removed N minutes after last update.</param>
    /// <see cref="ServerController.Put(Unosquare.Labs.EmbedIO.WebServer, Unosquare.Net.HttpListenerContext)"/>
    public ServerList(int serverTimeoutMinutes) {
        _serverTimeoutMinutes = serverTimeoutMinutes;

        _server = new Dictionary<int, ServerDetails>();
    }

    /// <summary>
    /// Add a new server or update existing informations
    /// </summary>
    /// <param name="info">Details about server to add/update</param>
    public void Add(ServerDetails info) {
        _server[info.id] = info;
    }

    public ServerDetails Get(int serverId) {
        ServerDetails info;
        if (!_server.TryGetValue(serverId, out info))
            return null;
        return info;
    }

    /// <summary>
    /// Removes old servers.
    /// </summary>
    /// <remarks>Minutes since server</remarks>
    /// <see cref="ServerList(int)"/>
    public void RemoveOldServers() {
        var itemsToRemove = _server.Where(val => (val.Value.lastUpdated - DateTime.Now).TotalMinutes > _serverTimeoutMinutes).ToArray();
        foreach (var item in itemsToRemove) {
            _server.Remove(item.Key);
        }
    }
}
