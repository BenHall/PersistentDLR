using System;
using System.Net.Sockets;
using System.Text;
using NUnit.Framework;

namespace PersistentDlr.Tests
{
    public class AgentTests
    {
        private const int PORT = 7894;

        [TestFixture]
        public class AgentTests_HandlingCommands
        {
            private Agent _agent;

            [SetUp]
            public void Setup() {
                _agent = new Agent();
                _agent.Start();
            }

            [TearDown]
            public void Teardown() {
                _agent.Stop();                
            }

            [Test]
            public void Agent_takes_command_and_returns_response()
            {
                var response = IssueCommand("127.0.0.1", PORT, "i = 2");
                Assert.That(response, Is.EqualTo("2"));
            }

            [Test]
            public void Agent_takes_multiple_commands_and_maintains_state()
            {
                IssueCommand("127.0.0.1", PORT, "i = 2");
                var response = IssueCommand("127.0.0.1", PORT, "i += 1");

                Assert.That(response, Is.EqualTo("3"));
            }

            private string IssueCommand(string ip, int port, string command) {
                string readString = null;
                using (var c = new TcpClient())
                {
                    c.Connect(ip, port);
                    var networkStream = c.GetStream();
                    WriteToStream(networkStream, command);

                    readString = GetResponseFromStream(c, networkStream);
                    WriteToStream(networkStream, "quit");
                }
                return readString;
            }

            private string GetResponseFromStream(TcpClient c, NetworkStream networkStream) {
                string readString = string.Empty;
                if (networkStream.CanRead)
                {
                    byte[] bytes = new byte[c.ReceiveBufferSize];

                    networkStream.ReadTimeout = 20;
                    networkStream.Read(bytes, 0, c.ReceiveBufferSize);
                    
                    readString = Encoding.UTF8.GetString(bytes);
                }
                return readString.Trim('\0', '\r','\n');
            }

            private void WriteToStream(NetworkStream networkStream, string command) {
                Byte[] sendBytes = Encoding.UTF8.GetBytes(command);
                networkStream.Write(sendBytes, 0, sendBytes.Length);
            }
        }

        [TestFixture]
        public class AgentTests_StartStop
        {

            [Test]
            public void Agent_opens_tcp_connection_on_port_7894()
            {
                var agent = new Agent();
                agent.Start();

                Assert_Port_Open("127.0.0.1", PORT);
            }

            [Test]
            public void Agent_closes_tcp_connection()
            {
                var agent = new Agent();
                agent.Start();
                agent.Stop();

                Assert_Port_Closed("127.0.0.1", PORT);
            }


            private void Assert_Port_Closed(string ip, int port)
            {
                try
                {
                    using (var c = new TcpClient())
                    {
                        c.Connect(ip, port);
                    }

                    Assert.Fail("Exception should have been thrown when we are unable to connect");
                }
                catch (SocketException)
                { }
            }

            private void Assert_Port_Open(string ip, int port)
            {
                using (var c = new TcpClient())
                {
                    c.Connect(ip, port);
                    Assert.True(c.Connected);
                }
            }
        }
    }
}
