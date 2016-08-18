using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    class Program
    {
        static void Main(string[] args)
        {
            Messenger lol = new Messenger();
            lol.WriteMessage("Test", "<insert name here>, you might get some messages because I'm testing my new framework. Sorry in advance.");
        }
    }
}
