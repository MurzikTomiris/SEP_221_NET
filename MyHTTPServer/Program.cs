using System.Net;
using System.Text;

namespace MyHTTPServer
{
    internal class Program
    {
        static Thread threadListener;
        static void Main(string[] args)
        {
            threadListener = new Thread(new ParameterizedThreadStart(Start2));
            threadListener.Start("http://localhost:12345/");
        }
        public static void Start(object prefix)
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(prefix.ToString());
            listener.Start();
            while (true)
            {
                Console.WriteLine("Прослушка работает..");
                HttpListenerContext context = listener.GetContext();

                HttpListenerRequest request = context.Request;
                var qry = request.RawUrl.Split('/');
                string Segment = null;
                Segment = qry[1];
                Console.WriteLine("Получено.." + Segment);
                HttpListenerResponse response = context.Response;

                string responseString = "<HTML><BODY> HELLO " + Segment + " </BODY></HTML>";
                byte[] buffer = Encoding.UTF8.GetBytes(responseString);

                response.ContentLength64 = buffer.Length;
                Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);

                output.Close();
            }
            //listener.Stop();
        }

        public static void Start2(object prefix)
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(prefix.ToString());
            listener.Start();
            while (true)
            {
                Console.WriteLine("Прослушка работает..");
                HttpListenerContext context = listener.GetContext();

                HttpListenerRequest request = context.Request;
                string text;
                using (var reader = new StreamReader(request.InputStream,
                                                     request.ContentEncoding))
                {
                    text = reader.ReadToEnd();
                }
                HttpListenerResponse response = context.Response;
                string responseString = "<HTML><BODY> HELLO " + text + " </BODY></HTML>";
                byte[] buffer = Encoding.UTF8.GetBytes(responseString);

                response.ContentLength64 = buffer.Length;
                Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);

                output.Close();
            }
            //listener.Stop();
        }
    }
}