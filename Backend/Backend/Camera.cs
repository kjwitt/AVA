using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Backend
{
    class Camera
    {
        double FrameRate;
        Capture capture;
        public Camera()
        {
            capture = new Capture(); //create a camera captue
            capture.SetCaptureProperty(CapProp.FrameWidth, 2592);
            capture.SetCaptureProperty(CapProp.FrameHeight, 1944);
            capture.FlipVertical = true;
            FrameRate = capture.GetCaptureProperty(CapProp.Fps);
        }
        public void TakePic()
        {           

            //Console.WriteLine(FrameRate);

            Mat frame = new Mat();
            //frame = capture.QueryFrame();

            capture.Grab();
            capture.Retrieve(frame, 0);

            frame.Save("/home/pi/AVA/Photos/" + string.Format("{0:yyyy-MM-dd hh_mm_ss_fftt}", DateTime.Now) + ".jpg");
            //Thread.Sleep((int)(1000.0 / FrameRate));
            frame.Dispose();
            //capture.Dispose();
        }


        
    }
}

//2592, 1944
//1920×1440