using System;
using System.Net.Sockets;
using System.Text;

namespace PersistentDlr
{
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
            byte[] data = new byte[1024];
            string stringData = null;

            if (ns.CanRead)
            {
                if (ns.DataAvailable) {
                    ns.ReadTimeout = 1;
                    var recv = ns.Read(data, 0, data.Length);
                    stringData = Encoding.UTF8.GetString(data, 0, recv);
                    stringData = stringData.Trim(new[] {'\r', '\n', '\0'});
                    Console.WriteLine("Got: " + stringData);
                }
            }

            return stringData;

        }
    }
}