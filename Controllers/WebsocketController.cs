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

        [Route("WebSocket")]
        public async Task Index()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest) {
                using (WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync())
                {
                    var utf8 = new UTF8Encoding();
                    while (true)
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

                        await Task.Delay(random.Next(0, 10000));
                    }
                }
            } else {
                Response.StatusCode = 400;
            }
        }
    }
}
