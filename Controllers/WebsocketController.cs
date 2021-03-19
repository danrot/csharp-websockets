using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace csharp_websockets.Controllers
{
    public class WebsocketController : Controller
    {
        private static Random random = new Random();

        private static UTF8Encoding utf8 = new UTF8Encoding();

        [Route("WebSocket")]
        public async Task Index()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest) {
                using (WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync())
                {
                    var sendRecursive = SendRecursive(webSocket);
                    var receiveRecursive = ReceiveRecursive(webSocket);

                    await Task.WhenAll(sendRecursive, receiveRecursive);
                }
            } else {
                Response.StatusCode = 400;
            }
        }

        private async Task ReceiveRecursive(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer),
                CancellationToken.None
            );

            await Send(webSocket);

            await ReceiveRecursive(webSocket);
        }

        private async Task Send(WebSocket webSocket)
        {
            var data = new {
                random = random.Next(0, 100),
                created = DateTime.Now,
            };

            await webSocket.SendAsync(
                new ArraySegment<byte>(utf8.GetBytes(JsonSerializer.Serialize(data))),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None
            );
        }

        private async Task SendRecursive(WebSocket webSocket)
        {
            await Send(webSocket);

            await Task.Delay(random.Next(0, 10000));
            await SendRecursive(webSocket);
        }
    }
}
