using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace Backend
{
    class Messenger
    {
        private List<string> ReceiverEmails = new List<string>();
        private string ReceiverFilePath = "sensative/ReceiverEmails.config";
        private string SenderEmailAddress;
        private string SenderEmailPassword;
        private string SenderFilePath = "sensative/SenderEmail.config";
        private SmtpClient SenderClient = new SmtpClient();
        public ConcurrentQueue<string> MailQueue;


        public Messenger()
        {
            using (StreamReader sr = File.OpenText(ReceiverFilePath))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    ReceiverEmails.Add(s);
                }
            }
            ReceiverEmails.TrimExcess();

            using (StreamReader sr = File.OpenText(SenderFilePath))
            {
                string s = "";
                s = sr.ReadLine();
                string[] words = s.Split(',');
                SenderEmailAddress = words[0];
                SenderEmailPassword = words[1];
            }

            SenderClient.Port = 587;
            SenderClient.Host = "smtp.gmail.com";
            SenderClient.EnableSsl = true;
            SenderClient.Timeout = 10000;
            SenderClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            SenderClient.UseDefaultCredentials = false;
            SenderClient.Credentials = new System.Net.NetworkCredential(SenderEmailAddress, SenderEmailPassword);
            SenderClient.EnableSsl = true;
            ServicePointManager.ServerCertificateValidationCallback =
                delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                { return true; };

            var now = DateTime.Now;
            Console.WriteLine(now + "." + now.Millisecond.ToString("000") + ": " + "(Mail) " + "Using " + SenderEmailAddress);

            MailQueue = new ConcurrentQueue<string>();

            Thread MailThread = new Thread(ProcessQueue);
            MailThread.Start();

            now = DateTime.Now;

            Console.WriteLine(now + "." + now.Millisecond.ToString("000") + ": " + "(Mail) " + "Thread started");
        }

        private void ProcessQueue()
        {
            while (true)
            {
                if (MailQueue.Count > 0)
                {
                    string s = "";
                    if (MailQueue.TryDequeue(out s))
                    {
                        string[] words = s.Split(':');
                        WriteMessage(words[0], words[1]);
                    }
                }

            }
        }

        public void WriteMessage(string subject, string body)
        {
            foreach (string email in ReceiverEmails)
            {
                MailMessage mm = new MailMessage(SenderEmailAddress, email, subject, body);
                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                SenderClient.Send(mm);
                mm.Dispose();

                string DebugMessage = "'(" + subject + ") " + body + "' sent to " + email;
                var now = DateTime.Now;
                Console.WriteLine(now + "." + now.Millisecond.ToString("000") + ": " + "(Mail) " + DebugMessage );
            }
        }
    }
}
