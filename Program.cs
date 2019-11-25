using System;
using System.IO;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Websocket.Client;

namespace sample_dotnet_websocket
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var exitEvent = new ManualResetEvent(false);
            var url = new Uri("wss://echo.websocket.org");
            var loop = 1;

            using (var client = new WebsocketClient(url))
            {
                client.ReconnectTimeoutMs = (int)TimeSpan.FromSeconds(30).TotalMilliseconds;
                client.ReconnectionHappened.Subscribe(type => Console.WriteLine($"Reconnection happened, type: {type}"));

                client.MessageReceived.Subscribe(async msg =>
                {
                    Console.WriteLine($"Message received: {msg}");
                    await SendMessage(client, loop);
                    loop++;
                });

                await client.Start();
                await Task.Run(() => client.Send($"sawasdee first time"));

                exitEvent.WaitOne();
            }
        }

        private static async Task SendMessage(WebsocketClient client, int loop)
        {
            await Task.Delay(2000);
            await Task.Run(() => client.Send($"sawasdee in subscribe: {loop}"));
        }
    }
}
