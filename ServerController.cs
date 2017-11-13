using System.Collections.Generic;
using Unosquare.Labs.EmbedIO;
using Unosquare.Labs.EmbedIO.Modules;
using Unosquare.Labs.EmbedIO.Constants;
using Unosquare.Net;
using Newtonsoft.Json;

//#TODO token

public class ServerController : WebApiController {

    ServerList _serverList;

    public ServerController(ServerList serverList) {
        _serverList = serverList;
    }

    /// <summary>
    /// Must be called from server to register or update details.
    /// </summary>
    /// <param name="server">Set by EmbedIO Framework</param>
    /// <param name="context">Set by EmbedIO Framework</param>
    [WebApiHandler(HttpVerbs.Put, "/api/v1/server/put")]
    public bool Put(WebServer server, HttpListenerContext context) {
        try {
           var details = JsonConvert.DeserializeObject<ServerDetails>(context.RequestBody());

            if (details.address == null || details.address.Length == 0)
                details.address = context.Request.RemoteEndPoint.Address.ToString();   //#TODO IPv6 ?

            _serverList.Add(details);

            return true;
        } catch (JsonReaderException e) {
            return HandleError(context, e.Message);
        }
    }

    /// <summary>
    /// Called from clients to query all registered servers.
    /// </summary>
    /// <param name="server">Set by EmbedIO Framework</param>
    /// <param name="context">Set by EmbedIO Framework</param>
    [WebApiHandler(HttpVerbs.Get, "/api/v1/server/query")]
    public bool List(WebServer server, HttpListenerContext context) {
        _serverList.RemoveOldServers();

        var result = new {
            count = _serverList.server.Count,
            server = new List<ServerDetails>()
        };

        foreach (var entry in _serverList.server)
            result.server.Add(entry.Value);

        try {
            return context.JsonResponse(JsonConvert.SerializeObject(result));
        } catch (JsonReaderException e) {
            return HandleError(context, e.Message);
        }
    }

    protected bool HandleError(HttpListenerContext context, string message, int statusCode = 500) {
        var errorResponse = new {
            Message = message,
            StatusCode = statusCode
        };

        context.Response.StatusCode = statusCode;
        return context.JsonResponse(errorResponse);
    }

}
