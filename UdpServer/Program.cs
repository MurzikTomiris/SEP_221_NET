using System.Net.Sockets;
using System.Net;
using System.Text;

namespace UdpServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            UDP.Listen();
        }
    }

    class UDP
    {
        public static void Listen()
        {
            int PORT = 8001;
            UdpClient udpClient = new UdpClient();
            udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, PORT));
            Console.WriteLine("Listening...");
            var from = new IPEndPoint(0, 0);
            var task = Task.Run(() =>
            {
                while (true)
                {
                    var recvBuffer = udpClient.Receive(ref from);
                    Console.WriteLine(Encoding.UTF8.GetString(recvBuffer));
                }
            });
            task.Wait();
        }
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return null;
        }
    }
}