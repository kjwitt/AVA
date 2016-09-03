using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Backend
{
    class PhonePing
    {
        private string PhoneInfoFilePath = "sensative/PhoneIP.config";
        private Dictionary<string, string> PhoneDirectory =
            new Dictionary<string, string>();

        public Dictionary<string, string> PhonesOnNetwork =
            new Dictionary<string, string>();

        private DebugMessenger Debug = new DebugMessenger("Phone Ping");

        public PhonePing()
        {
            using(StreamReader sr = File.OpenText(PhoneInfoFilePath))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    string[] words = s.Split(',');
                    PhoneDirectory.Add(words[1], words[0]);
                    //Names.Add(words[0]);
                    //IPAddresses.Add(words[1]);
                }
            }
            foreach (var pair in PhoneDirectory)
            {
                Debug.DebugStatement("Registered Phone: " + pair.Key +", " + pair.Value,ConsoleColor.White);
            }
            Thread PhonePingThread = new Thread(OnNetwork);
            PhonePingThread.Start();
            Debug.DebugStatement("Thread started", ConsoleColor.White);
        }

        private void OnNetwork()
        {
            while (true)
            {
                foreach (var pair in PhoneDirectory)
                {
                    int i = 0;
                    bool on = false;

                    //TcpClient tcpclnt = new TcpClient();
                    //tcpclnt.Connect(pair.Key, 62078);
                    //tcpclnt.Close();

                    //lol.Connect(pair.Key, 62078);


                    //ph_sendsocketdata1 ( "192.168.1.92", 62078, 6, "\x16" )

                    while (i < 3 && on == false)
                    {
                        on = PingHost(pair.Key);
                        //Console.WriteLine(on);
                        i++;
                    }

                    

                    var now = DateTime.Now;

                    if (on)
                    {
                        try
                        {
                            PhonesOnNetwork.Add(pair.Key, pair.Value);
                            Debug.DebugStatement(pair.Value + "'s phone has joined the network", ConsoleColor.White);
                        }
                        catch
                        {
                            Debug.DebugStatement(pair.Value + "'s phone has already been detected - ignoring...", ConsoleColor.White);
                        }

                    }
                    else
                    {
                        if(PhonesOnNetwork.Remove(pair.Key))
                        {
                            Debug.DebugStatement(pair.Value + "'s phone has left the network", ConsoleColor.White);
                        }
                        else
                        {
                            Debug.DebugStatement(pair.Value + "'s phone is not on the network", ConsoleColor.White);
                        }

                    }
                }
                if (PhonesOnNetwork.Count > 0)
                {
                    Thread.Sleep(60000);
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        }

        private static bool PingHost(string nameOrAddress)
        {
            bool pingable = false;
            Ping pinger = new Ping();
            try
            {
                PingReply reply = pinger.Send(nameOrAddress, 1000);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                // Discard PingExceptions and return false;
            }
            return pingable;
        }
    }
}
