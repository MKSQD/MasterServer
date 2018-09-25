using System;
using Unosquare.Labs.EmbedIO;
using Unosquare.Labs.EmbedIO.Modules;
using Unosquare.Labs.EmbedIO.Constants;

class Program {
    const int serverTimeoutMinutes = 2;

    static void Main(string[] args) {
        if (args.Length < 1) {
            Console.WriteLine("Wrong number of arguments. Example: MasterServer http://*:23888");
            return;
        }

        var address = args[0];

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
