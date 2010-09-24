using System.Net;
using System.Net.Sockets;
using System.Threading;
using PersistentDlr.Dlr;

namespace PersistentDlr
{
    public class Agent {

        private int DefaultPort = 7894;
        private IPAddress IPAddress = IPAddress.Loopback;
        private TcpListener _server;
        private RequestHandler _handler;
        Thread _thread;

        public void Start() {
            _server = new TcpListener(IPAddress, DefaultPort);
            _server.Start();

            _handler = new RequestHandler(_server, new DlrHost());
            _thread = new Thread(_handler.ProcessConnections);
            _thread.Start();
        }

        public void Stop() {
            _handler.Stop();
            _server.Stop();
        }
    }
}