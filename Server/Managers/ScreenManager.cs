using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using DesktopDuplication;


namespace Server.Managers
{
    public class ScreenManager
    {
        private bool _isRunning;
        private DesktopDuplicator _desktopDuplicator = new DesktopDuplicator(0);



        public delegate void ScreenHandler(Image changedPart, Point startPoint);

        public event ScreenHandler? ScreenChanged;



        public ScreenManager() 
        {
            _isRunning = false;
        }

        public void Start()
        {
            _isRunning = true;
            //int frames = 0;
            //System.Timers.Timer timer = new System.Timers.Timer(1000);
            //timer.Elapsed += (Object source, ElapsedEventArgs e) => { Console.WriteLine(frames); frames = 0; };
            //timer.AutoReset = true;
            //timer.Enabled = true;
            //timer.Start();


           

               

            while (_isRunning)
            {
                DesktopFrame frame = null;
                try
                {
                    frame = _desktopDuplicator.GetLatestFrame();
                }
                catch
                {
                    _desktopDuplicator = new DesktopDuplicator(0);
                }

                if (frame != null)
                {
                    if (frame.DesktopImage != null)
                    {

                        ScreenChanged?.Invoke(frame.DesktopImage, new Point(0, 0));
                    }

                }
            }
            
 
        }
        
        public void Stop()
        {
            _isRunning = false;
        }
    }
}
