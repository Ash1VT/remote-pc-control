using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Server.Managers
{
    public class MouseCoordinatesManager
    {
        private bool _isRunning;


        public delegate void MouseCoordinatesHandler(Point destPoint);
        public event MouseCoordinatesHandler? MouseCoordinatesChanged;


        public MouseCoordinatesManager() {
        
            _isRunning = false;
        }


        public void Start()
        { 
            _isRunning = true;
            var oldMouseCoordinates = GetMouseCoordinates();

            Task.Run(() => { 
                while(_isRunning)
                {

                    var newMouseCoordinates = GetMouseCoordinates();
                    if(!oldMouseCoordinates.Equals(newMouseCoordinates))
                    {
                        MouseCoordinatesChanged?.Invoke(newMouseCoordinates);
                        oldMouseCoordinates = newMouseCoordinates;
                    }

                }
            });

            
        }

        private Point GetMouseCoordinates()
        {
            User32.CURSORINFO cursorInfo;
            cursorInfo.cbSize = Marshal.SizeOf(typeof(User32.CURSORINFO));
            Point mouseCoordinates = new Point();
            if (User32.GetCursorInfo(out cursorInfo))
            {
                mouseCoordinates.X = cursorInfo.ptScreenPos.x;
                mouseCoordinates.Y = cursorInfo.ptScreenPos.y;
            }
            return mouseCoordinates;
        }

        public void Stop()
        {
            _isRunning = false;
        }


    }
}
