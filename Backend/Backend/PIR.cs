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
        const ConnectorPin PIRPin = ConnectorPin.P1Pin07;
        public bool status = false;

        public PIR()
        {
            var PIRSensor = PIRPin.Input()
                .Name("Switch")
                .Revert()
                .Switch()
                .Enable()
                .OnStatusChanged(b =>
                {
                    status = b;
                    Console.WriteLine("PIR switched {0}", b ? "on" : "off");
                });
        }       
    }
}
