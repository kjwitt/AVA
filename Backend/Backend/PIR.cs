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

        public PIR()
        {
            Thread PIRThread = new Thread(SetStatus);
            PIRThread.Start();

            var now = DateTime.Now;

            Console.WriteLine(now + "." + now.Millisecond.ToString("000") + ": " + "(PIR Sensor) " + "Thread Started");
        }

        private void SetStatus()
        {
            ConnectorPin PIRPin = ConnectorPin.P1Pin07;

            var procPin = PIRPin.ToProcessor();

            var driver = GpioConnectionSettings.DefaultDriver;

            driver.Allocate(procPin, PinDirection.Input);

            var isHigh = driver.Read(procPin);

            var now = DateTime.Now;

            Console.WriteLine(now + "." + now.Millisecond.ToString("000") + ": " + "(PIR Sensor) " + "Sensor Setup Completed");

            while (true)
            {
                now = DateTime.Now;

                Console.WriteLine(now + "." + now.Millisecond.ToString("000") + ": " + "(PIR Sensor) " + (isHigh ? "Movement indicated" : "Return to idle state"));

                driver.Wait(procPin, !isHigh, TimeSpan.FromDays(7)); //TODO: infinite
                isHigh = !isHigh;
                status = isHigh;
            }
        }
    }
}
