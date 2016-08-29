using RaspberryCam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    class Camera
    {
        Cameras cameras;

        public Camera()
        {
            cameras = Cameras.DeclareDevice()
                .Named("Camera 1")
                .WithDevicePath("/dev/video0")
                .Memorize();
        }

        public void TakePic()
        {
            cameras.Get("Camera 1").SavePicture(new PictureSize(1920,1440), "/home/pi/AVA/Photos/test2.jpg",0);
        }
        
    }
}

//2592, 1944
//1920×1440