using FluentFTP;
using System.Net;

namespace DzFTP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //FTP.DownloadFile();
            //FTP.DownloadFolder();
            //FTP.DownloadSeveralFiles();
            //FTP.UploadFile();
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

            public static void DownloadFolder()
            {
                string remoteFolderPath = "/2024";
                string localFolderPath = "C:\\Temp\\MyFTP";

                using (FtpClient client = new FtpClient(url))
                {
                    client.Credentials = new NetworkCredential(user, password);
                    client.Connect();

                    if (!Directory.Exists(localFolderPath))
                    {
                        Directory.CreateDirectory(localFolderPath);
                    }

                    client.DownloadDirectory(localFolderPath, remoteFolderPath, FtpFolderSyncMode.Update);
                    Console.WriteLine($"Downloaded folder: {remoteFolderPath} to {localFolderPath}");
                }
            }

            public static void DownloadSeveralFiles()
            {
                string[] remoteFilePaths = new string[]
                    {
                        "C:\\Temp\\MyFTP\\My123.xlsx",
                        "C:\\Temp\\MyFTP\\readme.txt"
                    };
                string localFolderPath = "C:\\Temp\\MyFTP";

                using (FtpClient client = new FtpClient(url))
                {
                    client.Credentials = new NetworkCredential(user, password);
                    client.Connect();

                    if (!Directory.Exists(localFolderPath))
                    {
                        Directory.CreateDirectory(localFolderPath);
                    }

                    foreach (var remoteFilePath in remoteFilePaths)
                    {
                        var fileName = Path.GetFileName(remoteFilePath);
                        var localFilePath = Path.Combine(localFolderPath, fileName);

                        client.DownloadFile(localFilePath, remoteFilePath);
                        Console.WriteLine($"Downloaded file: {remoteFilePath} to {localFilePath}");
                    }
                }
            }

            public static void UploadFolder(string localFolderPath, string remoteFolderPath)
            {
                using (FtpClient client = new FtpClient(url))
                {
                    client.Credentials = new NetworkCredential(user, password);
                    client.Connect();

                    client.UploadDirectory(localFolderPath, remoteFolderPath, FtpFolderSyncMode.Update);
                    Console.WriteLine($"Uploaded folder: {localFolderPath} to {remoteFolderPath}");
                }
            }

            public static void UploadSeveralFiles(string[] localFilePaths, string remoteFolderPath)
            {
                using (FtpClient client = new FtpClient(url))
                {
                    client.Credentials = new NetworkCredential(user, password);
                    client.Connect();

                    foreach (var localFilePath in localFilePaths)
                    {
                        var fileName = Path.GetFileName(localFilePath);
                        var remoteFilePath = Path.Combine(remoteFolderPath, fileName).Replace("\\", "/");

                        client.UploadFile(localFilePath, remoteFilePath);
                        Console.WriteLine($"Uploaded file: {localFilePath} to {remoteFilePath}");
                    }
                }
            }
        }
    }
}