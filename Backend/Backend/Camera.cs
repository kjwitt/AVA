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
        public Camera()
        {         
            
        }
        public void TakePic()
        {

            Capture capture = new Capture(); //create a camera captue
            capture.SetCaptureProperty(CapProp.FrameWidth, 2592);
            capture.SetCaptureProperty(CapProp.FrameHeight, 1944);
            capture.FlipVertical = true;
            double FrameRate = capture.GetCaptureProperty(CapProp.Fps);

            Mat frame = new Mat();
            capture.QueryFrame();


            frame.Save("/home/pi/AVA/Photos/" + string.Format("{0:yyyy-MM-dd hh_mm_ss_fftt}", DateTime.Now) + ".jpg");
            Thread.Sleep((int)(1000.0 / FrameRate));
            frame.Dispose();
            capture.Dispose();
        }


        
    }
}

//2592, 1944
//1920×1440

//cameras = Cameras.DeclareDevice()
//              .Named("Camera 1")
//            .WithDevicePath("/dev/video0")
//          .Memorize();
//cam

//eras.Get("Camera 1").SavePicture(new PictureSize(1920,1440), "/home/pi/AVA/Photos/test2.jpg",0);