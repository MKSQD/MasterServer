using System;
using Unosquare.Labs.EmbedIO;
using Unosquare.Labs.EmbedIO.Modules;
using Unosquare.Labs.EmbedIO.Constants;

class Program {

    const int serverTimeoutMinutes = 2;
    const string address = "http://*:23888";

    static void Main(string[] args) {

        using (var server = new WebServer(address, RoutingStrategy.Wildcard)) {
            server.RegisterModule(new WebApiModule());

            var serverList = new ServerList(serverTimeoutMinutes);
            server.Module<WebApiModule>().RegisterController<ServerController>(() => {
                return new ServerController(serverList);
            });

            server.RunAsync();
            Console.ReadKey(true);
        }

    }
}
