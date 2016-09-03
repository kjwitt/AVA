using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    class DebugMessenger
    {
        string type;

        public DebugMessenger(string _type)
        {
            type = _type;
        }

        public void DebugStatement(string message, ConsoleColor color)
        {
            var now = DateTime.Now;
            Console.ForegroundColor = color;
            Console.WriteLine(now + "." + now.Millisecond.ToString("000") + ": " + "("+ type +") " + message);
            Console.ResetColor();
        }
    }
}
