using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace PersistentDlr
{
    public class Agent {

        private int DefaultPort = 7894;
        private IPAddress IPAddress = IPAddress.Loopback;
        private TcpListener _server;
        private RequestHandler _handler;

        public void Start() {
            _server = new TcpListener(IPAddress, DefaultPort);
            _server.Start();

            _handler = new RequestHandler(_server);
            Thread thread = new Thread(_handler.ProcessConnections);
            thread.Start();

            //return true;
        }

        public void Stop() {
            _server.Stop();
        }
    }

    public class NetworkStreamHandler
    {
        public static void WriteStringToStream(NetworkStream ns, string data)
        {
            string dataToSend = data;
            ns.Write(Encoding.UTF8.GetBytes(dataToSend), 0, dataToSend.Length);
            ns.Flush();
        }

        public static string ReadStreamIntoString(NetworkStream ns)
        {
            byte[] buffer = new byte[1024];
            ns.Read(buffer, 0, buffer.Length);
            while (ns.DataAvailable)
            {
                ns.Read(buffer, 0, buffer.Length);
            }
            var enc = new UTF8Encoding();
            return enc.GetString(buffer);
        }
    }
}