using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raspberry.IO.GeneralPurpose;
using System.Threading;

namespace Backend
{
    class PIR
    {
        public bool status = false;

        IGpioConnectionDriver driver = GpioConnectionSettings.DefaultDriver;
        ProcessorPin procPin;

        private DebugMessenger Debug = new DebugMessenger("PIR Sensor");

        public PIR()
        {
            ConnectorPin PIRPin = ConnectorPin.P1Pin07;

            procPin = PIRPin.ToProcessor();

            //driver = GpioConnectionSettings.DefaultDriver;

            driver.Allocate(procPin, PinDirection.Input);

            Debug.DebugStatement("Sensor Setup Completed",ConsoleColor.White);


            Thread PIRThread = new Thread(SetStatus);
            PIRThread.Start();

            Debug.DebugStatement("Thread Started",ConsoleColor.White);
        }

        private void SetStatus()
        {
            

            var now = DateTime.Now;

            var isHigh = driver.Read(procPin);

            var PrevState = isHigh;

            while (true)
            {
                isHigh = driver.Read(procPin);
                //driver.Wait(procPin, !isHigh, TimeSpan.FromDays(7)); //TODO: infinite

                if(isHigh!=PrevState)
                {
                    Debug.DebugStatement((isHigh ? "Movement indicated" : "Return to idle state"),ConsoleColor.White);
                    PrevState = isHigh;
                }
                status = isHigh;
                Thread.Sleep(500);
            }
        }
    }
}
