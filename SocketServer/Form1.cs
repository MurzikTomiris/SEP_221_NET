using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace SocketServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StartServer();
        }

        async Task StartServer()
        {
            await Task.Run(() =>
            {
                var ip = Dns.GetHostEntry(tbHost.Text);
                var ad = ip.AddressList[0];
                var ep = new IPEndPoint(ad, int.Parse(tbPort.Text));
                Socket listener = new Socket(ad.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                listener.Bind(ep);
                listener.Listen(20);
                listBox1.Items.Add("Listen to " + ep);
                Socket handler;
                while (true)
                {
                    byte[] bytes = new byte[1024];
                    handler = listener.Accept();
                    string data = null;
                    int count = handler.Receive(bytes);
                    var st = Encoding.UTF8.GetString(bytes, 0, count);
                    listBox1.Items.Add(st);
                    data += Encoding.UTF8.GetString(bytes, 0, count);
                    string replay = "Получено от " + data;
                    byte[] response = Encoding.UTF8.GetBytes(replay);
                    handler.Send(response);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            });
        }

        async Task SendToServer2()
        {
            await Task.Run(() =>
            {
                TcpListener tcpListener = null;
                try
                {
                    IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                    tcpListener = new TcpListener(localAddr, int.Parse(tbPort.Text) + 1);
                    tcpListener.Start();
                    listBox1.Items.Add("Listen to ");
                    while (true)
                    {
                        TcpClient client = tcpListener.AcceptTcpClient();
                        NetworkStream stream = client.GetStream();

                        StreamReader reader = new StreamReader(stream);
                        string message = reader.ReadLine();
                        listBox1.Items.Add("Получено от " + message);

                        StreamWriter writer = new StreamWriter(stream);
                        writer.WriteLine("Ответка " + message);

                        writer.Close();
                        reader.Close();
                        stream.Close();
                        client.Close();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    if (tcpListener != null)
                        tcpListener.Stop();
                }
            });

        }

        private void button2_Click(object sender, EventArgs e)
        {
            SendToServer2();
        }
    }
}