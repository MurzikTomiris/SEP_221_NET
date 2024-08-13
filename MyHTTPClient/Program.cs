using HtmlAgilityPack;
using System.Net;

namespace MyHTTPClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //await Method_1();
            //await Method_2();
            //Method_3();
            Method_4();
        }
        static async Task<string> Method_1()
        {
            using (HttpClient client = new HttpClient())
            {
                var result = await client.GetStringAsync("https://www.google.com/");
                return result;
            }
        }

        static async Task Method_2()
        {
            using (HttpClient client = new HttpClient())
            {
                var result = await client.GetAsync("https://www.google.com/");

                //    //var result = response.Content.ReadAsStringAsync().Result;
                using (StreamReader sr = new StreamReader(await result.Content.ReadAsStreamAsync()))
                {
                    string result2 = sr.ReadToEnd();
                    Console.WriteLine(result2);
                }

            }
        }


        static void Method_3()
        {
            using (var client = new WebClient())
            {
                //var result = client.DownloadData("https://www.nationalbank.kz/file/download/101938");
                //File.WriteAllBytes("c:\\temp\\123.pdf", result);

                //var result2 = client.DownloadData("https://www.nationalbank.kz/file/download/102469");
                //File.WriteAllBytes("c:\\temp\\123.xlsx", result2);

                var result = client.DownloadData("https://www.youtube.com/watch?v=DWGq_hp0XSI");
                File.WriteAllBytes("c:\\temp\\123.mp4", result);
            }
        }

        static void Method_4()
        {
            var url = "https://nationalbank.kz/ru";
            var web = new HtmlWeb();
            var doc = web.Load(url);
            HtmlNode node = doc.DocumentNode.SelectSingleNode("/html/body/div[1]/main/section[1]/div/div[1]/h1");
            Console.WriteLine(node.InnerText);
        }
    }
}