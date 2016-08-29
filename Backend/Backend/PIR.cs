using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raspberry.IO.GeneralPurpose;

namespace Backend
{
    class PIR
    {
        public bool status = false;

        public PIR()
        {
            ConnectorPin PIRPin = ConnectorPin.P1Pin07;

            var procPin = PIRPin.ToProcessor();

            var driver = GpioConnectionSettings.DefaultDriver;

            driver.Allocate(procPin, PinDirection.Input);

            var isHigh = driver.Read(procPin);

            while (true)
            {
                var now = DateTime.Now;

                Console.WriteLine(now + "." + now.Millisecond.ToString("000") + ": " + (isHigh ? "HIGH" : "LOW"));

                driver.Wait(procPin, !isHigh, TimeSpan.FromDays(7)); //TODO: infinite
                isHigh = !isHigh;
            }
        }
    }
}
