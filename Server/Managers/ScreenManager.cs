using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using DesktopDuplication;
using Timer = System.Timers.Timer;


namespace Server.Managers
{
    public class ScreenManager
    {
        private bool _isRunning = false;
        private bool _autoRestart = false;


        private DesktopDuplicator _desktopDuplicator = new DesktopDuplicator(0);


        public delegate Task<bool> ScreenHandler(Image screen);

        public event ScreenHandler? ScreenChanged;


                                
        public ScreenManager() 
        {
        }

        public ScreenManager(bool autoRestart)
        {
            _autoRestart = autoRestart;
        }

        public void Start()
        {
            _isRunning = true;
            Debug.WriteLine("STARTED SCREEN MANAGER");
            

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
                                try
                                {
                                    if (!await ScreenChanged?.Invoke(frame.DesktopImage))
                                    {
                                        Stop();
                                    
                                        if (_autoRestart)
                                        {
                                            Start();
                                        }
                                    }
                                }
                                catch
                                {
                                    Stop();
                                    if (_autoRestart)
                                    {
                                        Start();
                                    }
                                }
                            }
                        
                        }
                    }
                });
            
 
        }
        
        public void Stop()
        {
            _isRunning = false;
            Debug.WriteLine("STOPPED SCREEN MANAGER");
        }
    }
}
