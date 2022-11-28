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



        public delegate Task ScreenHandler(Image screen);

        public event ScreenHandler? ScreenChanged;



        public ScreenManager() 
        {
            _isRunning = false;
        }

        public void Start()
        {
            _isRunning = true;

            Task.Run(async () =>
            {

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
                            Console.WriteLine(frame.UpdatedRegions[0].Location);
                            ScreenChanged?.Invoke(frame.DesktopImage);

                        }

                    }
                }
            });
            
 
        }
        
        public void Stop()
        {
            _isRunning = false;
        }
    }
}
