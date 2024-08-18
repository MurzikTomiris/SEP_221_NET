using System.Net;
using System.Text;

namespace DzHTTP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8080/");
            listener.Start();
            Console.WriteLine("HTTP сервер запущен. Ожидание подключений...");

            while (true)
            {
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;

                if (request.HttpMethod == "POST")
                {
                    using (StreamReader reader = new StreamReader(request.InputStream, request.ContentEncoding))
                    {
                        string requestBody = reader.ReadToEnd();
                        var coefficients = ParseCoefficients(requestBody);

                        if (coefficients != null)
                        {
                            var (root1, root2, message) = SolveQuadraticEquation(coefficients.Value.a, coefficients.Value.b, coefficients.Value.c);
                            string responseString = $"<html><body><h1>{message}</h1><p>Root 1: {root1}</p><p>Root 2: {root2}</p></body></html>";
                            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                            response.ContentLength64 = buffer.Length;
                            response.OutputStream.Write(buffer, 0, buffer.Length);
                        }
                        else
                        {
                            string responseString = "<html><body><h1>Ошибка: Неверные входные данные</h1></body></html>";
                            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                            response.ContentLength64 = buffer.Length;
                            response.OutputStream.Write(buffer, 0, buffer.Length);
                        }
                    }
                }
                else
                {
                    response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                }

                response.OutputStream.Close();
            }
        }

        static (double a, double b, double c)? ParseCoefficients(string requestBody)
        {
            try
            {
                var parts = requestBody.Split('&');
                double a = double.Parse(parts[0].Split('=')[1]);
                double b = double.Parse(parts[1].Split('=')[1]);
                double c = double.Parse(parts[2].Split('=')[1]);
                return (a, b, c);
            }
            catch
            {
                return null;
            }
        }

        static (double? root1, double? root2, string message) SolveQuadraticEquation(double a, double b, double c)
        {
            double discriminant = b * b - 4 * a * c;

            if (discriminant > 0)
            {
                double root1 = (-b + Math.Sqrt(discriminant)) / (2 * a);
                double root2 = (-b - Math.Sqrt(discriminant)) / (2 * a);
                return (root1, root2, "Уравнение имеет два действительных корня:");
            }
            else if (discriminant == 0)
            {
                double root = -b / (2 * a);
                return (root, null, "Уравнение имеет один действительный корень:");
            }
            else
            {
                return (null, null, "Уравнение не имеет действительных корней.");
            }
        }
    }
}