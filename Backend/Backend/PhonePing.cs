using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    class PhonePing
    {
        private string PhoneInfoFilePath = "sensative/PhoneIP.config";
        public List<string> Names = new List<string>();
        public List<string> IPAddresses = new List<string>();

        public PhonePing()
        {
            using(StreamReader sr = File.OpenText(PhoneInfoFilePath))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    string[] words = s.Split(',');
                    Names.Add(words[0]);
                    IPAddresses.Add(words[1]);
                }
            }
            foreach(string sddress in IPAddresses)
            {
                Console.WriteLine(sddress);
            }
            foreach (string name in Names)
            {
                Console.WriteLine(name);
            }
        }
    }
}
