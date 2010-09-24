using System;
using System.Net.Sockets;
using System.Threading;
using PersistentDlr.Dlr;

namespace PersistentDlr
{
    public class RequestHandler
    {
        private readonly TcpListener _server;
        private readonly DlrHost _dlrHost;
        private bool _stopped;

        public RequestHandler(TcpListener server, DlrHost dlrHost) {
            _server = server;
            _dlrHost = dlrHost;
        }

        public void Stop() {
            _stopped = true;
        }

        public void ProcessConnections()
        {
            while (!_stopped) {
                if (_server.Pending()) {
                    TcpClient client = _server.AcceptTcpClient();
                    NetworkStream ns = client.GetStream();

                    try
                    {
                        string request = NetworkStreamHandler.ReadStreamIntoString(ns);

                        ExecuteRequest(ns, request);
                        request = NetworkStreamHandler.ReadStreamIntoString(ns);
                        //ns.Close(1000);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("It's all gone wrong - " + ex.Message);
                    }

                    ns.Close();
                } else {
                    Thread.Sleep(100);
                }
            }
        }

        private void ExecuteRequest(NetworkStream ns, string request) {
            if (!string.IsNullOrEmpty(request)) {
                var response = _dlrHost.Execute(request);
                Console.WriteLine("Result: " + response);
                WriteResponse(ns, response);
            }
        }

        private void WriteResponse(NetworkStream ns, object response) {
            if (response == null) {
                NetworkStreamHandler.WriteStringToStream(ns, "No Response");
            } else {
                NetworkStreamHandler.WriteStringToStream(ns, response.ToString());
            }
        }
    }
}