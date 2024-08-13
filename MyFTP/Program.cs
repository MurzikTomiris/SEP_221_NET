using FluentFTP;
using Renci.SshNet;
using System.Net;
using static System.Net.WebRequestMethods;

namespace MyFTP
{
    internal class Program
    {

        static void Main(string[] args)
        {
            // FTP считается не безопасным и сейчас рездко используется
            //порт 21
            //nuget пакет FluentFTP сайт dlptest.com
            //FTP.DownloadFile();
            //FTP.UploadFile();
            //FTP.DownloadFileMyFTP();
            //для sftp пакет SSH.NET от Renci  сайт https://test.rebex.net/
            SFTP.DownloadFile();
        }

        class FTP
        {
            static string url = "ftp://ftp.dlptest.com/";
            static string user = "dlpuser";
            static string password = "rNrKYTX9g7z3RgJRmxWuGHbeu";
            public static void DownloadFile()
            {
                FtpClient client = new FtpClient();
                client.Host = url;
                client.Credentials = new NetworkCredential(user, password);
                client.Connect();
                var status = client.DownloadFile("C:\\Temp\\MyFTP\\My123.xlsx", "/2024/123.xlsx");
                Console.WriteLine("DownloadFile");

            }

            public static void UploadFile()
            {
                FtpClient client = new FtpClient();
                client.Host = url;
                client.Credentials = new NetworkCredential(user, password);
                client.Connect();
                var status = client.UploadFile("C:\\Temp\\MyFTP\\My123.xlsx", "/2024/Tomiris.xlsx");
                Console.WriteLine("UploadFile");

            }

            public static void DownloadFileMyFTP()
            {
                //скачивает в bin
                FtpClient client = new FtpClient();
                client.Host = "localhost";
                client.Connect();
                var status = client.DownloadFile("My123.xlsx", "/My123.xlsx");
                Console.WriteLine("DownloadFileMyFTP");

            }
        }

        class SFTP
        {
            static string url = "test.rebex.net";
            static string user = "demo";
            static string password = "password";
            static int port = 22;

            public static void DownloadFile()
            {
                using (var sftp = new SftpClient(url, port, user, password))
                {
                    sftp.Connect();

                    string path = "readme.txt";
                    string localFilePath = @"C:\Temp\MyFTP\readme.txt";

                    using (var fileStream = new FileStream(localFilePath, FileMode.Create))
                    {
                        // Загрузка файла с сервера
                        sftp.DownloadFile(path, fileStream);
                        Console.WriteLine("DownloadFile");
                    }

                    sftp.Disconnect();
                }
            }
        }
    }
}