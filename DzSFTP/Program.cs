using Renci.SshNet;
using static System.Net.WebRequestMethods;

namespace DzSFTP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string path = "readme.txt";
            string localFilePath = @"C:\Temp\MySftp\readme.txt";
            //SFTP.DownloadFile(path, localFilePath);

            string remoteFolderPath = @"pub/example";
            string localFolderPath = @"C:\Temp\MySftp\pub";
            //SFTP.DownloadFolder(remoteFolderPath, localFolderPath);

            string[] remoteFilePaths = new string[]
            {
                "pub/example/KeyGenerator.png",
                "pub/example/mail-editor.png"
            };
            SFTP.DownloadSeveralFiles(remoteFilePaths, localFolderPath);
        }

        class SFTP
        {
            static string url = "test.rebex.net";
            static string user = "demo";
            static string password = "password";
            static int port = 22;

            public static void DownloadFile(string path, string localFilePath)
            {
                using (var sftp = new SftpClient(url, port, user, password))
                {
                    sftp.Connect();

                    using (var fileStream = new FileStream(localFilePath, FileMode.Create))
                    {
                        // Загрузка файла с сервера
                        sftp.DownloadFile(path, fileStream);
                        Console.WriteLine("DownloadFile");
                    }

                    sftp.Disconnect();
                }
            }

            public static void DownloadFolder(string remoteFolderPath, string localFolderPath)
            {
                using (var sftp = new SftpClient(url, port, user, password))
                {
                    sftp.Connect();

                    // Создаем локальную папку, если она не существует
                    if (!Directory.Exists(localFolderPath))
                    {
                        Directory.CreateDirectory(localFolderPath);
                    }

                    var files = sftp.ListDirectory(remoteFolderPath);
                    foreach (var file in files)
                    {
                        if (!file.IsDirectory)
                        {
                            var remoteFilePath = file.FullName;
                            var localFilePath = Path.Combine(localFolderPath, file.Name);
                            using (var fileStream = new FileStream(localFilePath, FileMode.Create))
                            {
                                sftp.DownloadFile(remoteFilePath, fileStream);  
                            }
                            Console.WriteLine("Download folder");
                        }
                    }

                    sftp.Disconnect();
                }
            }

            public static void DownloadSeveralFiles(string[] remoteFilePaths, string localFolderPath)
            {
                using (var sftp = new SftpClient(url, port, user, password))
                {
                    sftp.Connect();

                    // Создаем локальную папку, если она не существует
                    if (!Directory.Exists(localFolderPath))
                    {
                        Directory.CreateDirectory(localFolderPath);
                    }

                    foreach (var remoteFilePath in remoteFilePaths)
                    {
                        var fileName = Path.GetFileName(remoteFilePath);
                        var localFilePath = Path.Combine(localFolderPath, fileName);
                        using (var fileStream = new FileStream(localFilePath, FileMode.Create))
                        {
                            sftp.DownloadFile(remoteFilePath, fileStream);
                            Console.WriteLine("Downloaded file: " + remoteFilePath);
                        }
                    }

                    sftp.Disconnect();
                }
            }
        }
    }
}