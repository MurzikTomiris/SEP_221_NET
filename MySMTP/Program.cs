using MailKit.Search;
using MailKit;
using System.Net.Mail;
using MailKit.Net.Imap;

namespace MySMTP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // SMTP протакол на отправку писем
            //IMAP - на чтение письма, можно не  скачивать самои письмо
            // pop3 - на чтение письма, не дает инфу о письме пока не скачает его
            // nuget MailKit - мощный пакет для работы с почтой

            //SendMail.Send();
            //GetMail.Start();
            string mailSubject = "Test";
            GetMail.DelMail(mailSubject);
        }
    }

    public class SendMail
    {
        public static void Send()
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.IsBodyHtml = true;
                mail.From = new MailAddress("murzik@mail.ru");
                mail.To.Add("murzik@yandex.kz");
                //mail.CC.Add("5934516276@bk.ru"); //копия, это коллекция можно несколько адресов
                //mail.Bcc.Add("5931565414276@bk.ru"); //скрытая копия
                mail.Subject = "Test";
                mail.Body = "<h1>Скрытая копия</h1>";
                Attachment attachment = new Attachment(@"C:\Users\Томирис\Desktop\Мур.pdf");

                mail.Attachments.Add(attachment);
                using (System.Net.Mail.SmtpClient server = new System.Net.Mail.SmtpClient("smtp.mail.ru", 587))
                {
                    server.UseDefaultCredentials = false;
                    server.Credentials = new System.Net.NetworkCredential("murzik@mail.ru", "Daw9Q3442715488434Pji6JeMNaBA1");
                    server.EnableSsl = true;
                    server.DeliveryMethod = SmtpDeliveryMethod.Network;
                    server.Send(mail);
                    server.Dispose();
                    Console.WriteLine("Send");
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
        }
    }

    public class GetMail
    {
        //метод для скачивания ящика
        public static void Start()
        {
            using (var client = new ImapClient())
            {
                client.Connect("imap.mail.ru", 993, true);
                client.Authenticate("murzik@mail.ru", "Daw9Q6564334Pji65146514JeM514654NaBA1");
                client.Inbox.Open(FolderAccess.ReadWrite);
                var ns = client.GetFolder("INBOX");
                //IMailFolder inbox = client.GetFolder("Test");
                //inbox.Open(FolderAccess.ReadWrite);
                //var uids = ns.Search(SearchQuery.All);
                var uids = ns.Search(SearchQuery.New);
                foreach (var uid in uids)
                {
                    var message = ns.GetMessage(uid);

                    Console.WriteLine(message.Subject);
                }

            }
        }


        public static void DelMail(string mailSubject)
        {
            using (var client = new ImapClient())
            {
                client.Connect("imap.mail.ru", 993, true);
                client.Authenticate("murzik@mail.ru", "Dawkjnlk9Q6516334Pji6JeM5846NaBA1");

                // Открываем INBOX с правами на чтение и запись
                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadWrite);

                // Ищем письма по теме
                var uids = inbox.Search(SearchQuery.SubjectContains(mailSubject));
                Console.WriteLine(mailSubject);

                // Удаляем найденные письма
                foreach (var uid in uids)
                {
                    inbox.AddFlags(uid, MessageFlags.Deleted, true);
                }

                // Очищаем папку, удаляя письма с флагом "Deleted"
                inbox.Expunge();

                // Закрываем подключение
                client.Disconnect(true);
            }
        }
    }
}