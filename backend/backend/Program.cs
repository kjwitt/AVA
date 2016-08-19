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

            lol.Speak("Ok here goes nothing...");
        }

        /*static public void PlaySound(Stream sound)
        {
            
            sound.Position = 0;     // Manually rewind stream 
            player.Stream = null;    // Then we have to set stream to null 
            player.Stream = sound;  // And set it again, to force it to be loaded again... 
            player.Play();          // Yes! We can play the sound! 
        }*/
    }
}
