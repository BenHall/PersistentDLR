using System.Net.Sockets;

namespace PersistentDlr
{
    public class RequestHandler
    {
        private readonly TcpListener _server;

        public RequestHandler(TcpListener server)
        {
            _server = server;
        }

        public void ProcessConnections()
        {
            TcpClient client = _server.AcceptTcpClient();
            NetworkStream ns = client.GetStream();

            string request = NetworkStreamHandler.ReadStreamIntoString(ns);

            HandleRequest(ns, "2");

            ns.Close();
        }

        private void HandleRequest(NetworkStream ns, string response) {
            NetworkStreamHandler.WriteStringToStream(ns, response);
        }
    }
}