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
            Messenger Mail = new Messenger();
            TTS Speech = new TTS();
            Camera Cam = new Camera();
            PhonePing Pinger = new PhonePing();
            PIR MotionDetect = new PIR();

            Mail.MailQueue.Enqueue("System:AVA Started");    

            Speech.TTSQueue.Enqueue("Hello! My name is Awva, and I am your personal home assistant.");    
               
            Cam.TakePic();
            Speech.TTSQueue.Enqueue("A photo has been taken.");
            Mail.MailQueue.Enqueue("Camera:Test picture has been taken");            

            bool prevState = MotionDetect.status;

            while (true)
            {
                if (prevState != MotionDetect.status)
                {
                    //Console.WriteLine(MotionDetect.status);
                    prevState = MotionDetect.status;
                }
                
            }


        }

    }
}
