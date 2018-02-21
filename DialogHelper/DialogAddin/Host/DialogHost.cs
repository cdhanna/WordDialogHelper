using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace DialogAddin.Host
{
    class DialogHost
    {

        private WebSocketServer _server;

        public void Start(string host="localhost", int port=9090, string protocol="ws")
        {
            _server = new WebSocketServer($"{protocol}://{host}:{port}");
            _server.AddWebSocketService<DialogClientBehavour>("/dialog");
            _server.ReuseAddress = true;

            

            _server.Start();
        }

        public void Stop()
        {
            _server.Stop();
        }
    }
}
