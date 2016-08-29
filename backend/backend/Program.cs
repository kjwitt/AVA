using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    class Program
    {

        static SoundPlayer player = new SoundPlayer();

        static void Main(string[] args)
        {
            //Messenger lol = new Messenger();
            //lol.WriteMessage("Test", "Brielle, you might get some messages because I'm testing my new framework. Sorry in advance.");
            TTS lol = new Backend.TTS();

            lol.Speak("Hello! My name is Awva, and I am your personal home assistant.");

            Camera test = new Camera();
            test.TakePic();

            PIR MotionDetect = new PIR();


        }

    }
}
