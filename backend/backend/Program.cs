using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Backend
{
    class Program
    {
        enum States {Idle = 0, Armed, Alarm };
        static States State;

        //static SoundPlayer player = new SoundPlayer();

        static private DebugMessenger Debug = new DebugMessenger("AVA");

        static void Main(string[] args)
        {
            Messenger Mail = new Messenger();
            TTS Speech = new TTS();
            Camera Cam = new Camera();
            PhonePing Pinger = new PhonePing();
            PIR MotionDetect = new PIR();

            Mail.MailQueue.Enqueue("System:AVA Started");

            Debug.DebugStatement("System Started", ConsoleColor.Green);

            Speech.TTSQueue.Enqueue("Hello! My name is Awva, and I am your personal home assistant.");        

            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 10000;
            aTimer.Enabled = true;
            Stopwatch StateTimer = new Stopwatch();
            StateTimer.Start();

            long TimeSinceLastEvent = 0;

            
            State = States.Idle;


            while (true)
            {
                switch(State)
                {
                    case (States.Idle):

                        //change state conditions
                        if (Pinger.PhonesOnNetwork.Count>0)
                        {
                            State = States.Idle;
                        }
                        else
                        {
                            State = States.Armed;
                            Mail.MailQueue.Enqueue("Security:No phones detected on network. System has been armed!");
                            StateTimer.Restart();
                            TimeSinceLastEvent = 0;
                        }
                        break;

                    case (States.Armed):

                        if( StateTimer.ElapsedMilliseconds - TimeSinceLastEvent > (60000*30) )
                        {
                            Cam.TakePic();
                            Mail.MailQueue.Enqueue("Security:Regular armed interval photo taken.");
                            TimeSinceLastEvent = StateTimer.ElapsedMilliseconds;
                        }

                        //change state conditions
                        if (Pinger.PhonesOnNetwork.Count > 0)
                        {
                            State = States.Idle;
                            Mail.MailQueue.Enqueue("Security:Phone detected. System has been disarmed!");
                            StateTimer.Restart();
                            TimeSinceLastEvent = 0;
                        }
                        else if ( MotionDetect.status == true)
                        {
                            State = States.Alarm;
                            Mail.MailQueue.Enqueue("Security:Motion Detected! Alarm in progress! Check Google Drive for Frequent Photo Updates.");
                            StateTimer.Restart();
                            TimeSinceLastEvent = 0;
                        }
                        else
                        {
                            State = States.Armed;
                        }
                        break;

                    case (States.Alarm):
                        Cam.TakePic();
                        if(StateTimer.ElapsedMilliseconds - TimeSinceLastEvent > 30000)
                        {
                            Speech.TTSQueue.Enqueue("Intruder Alert. Please leave the premises; the police are on their way.");
                            TimeSinceLastEvent = StateTimer.ElapsedMilliseconds;
                        }
                            //change state conditions
                        if (Pinger.PhonesOnNetwork.Count > 0)
                        {
                            State = States.Idle;
                            Mail.MailQueue.Enqueue("Security:Phone detected. Alarm ceased!");
                            StateTimer.Restart();
                            TimeSinceLastEvent = 0;
                        }
                        else
                        {
                            State = States.Alarm;
                        }
                        break;
                }              
            }


        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Debug.DebugStatement("Current State: " + State.ToString(), ConsoleColor.Green);
        }

    }
}
