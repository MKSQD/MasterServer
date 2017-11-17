using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

/// <summary>
/// 
/// </summary>
public class ServerDetails {

    public int id {
        get { return GetHashCode(); }
    }

    [JsonIgnore]
    DateTime _lastUpdated;
    public DateTime lastUpdated {
        get { return _lastUpdated; }
    }

    public string address;

    [JsonRequired]
    public ushort port;

    [JsonRequired]
    public string version;

    [JsonRequired]
    public ushort players;

    [JsonRequired]
    public ushort maxPlayers;

    [JsonRequired]
    public string title;

    public ServerDetails() {
        _lastUpdated = DateTime.Now;
    }

    public override bool Equals(object obj) {
        ServerDetails other = (ServerDetails)obj;
        if (other == null)
            throw new InvalidCastException();

        return other.address == address && other.port == port;
    }

    public override int GetHashCode() {
        return (address + port.ToString()).GetHashCode();
    }
}
