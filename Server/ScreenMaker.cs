using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Server
{
    public class ScreenMaker
    {
        public static Bitmap Capture(int x, int y, int width, int height)
        {
            var bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(x, y, 0, 0, bitmap.Size, CopyPixelOperation.SourceCopy);

                User32.CURSORINFO cursorInfo;
                cursorInfo.cbSize = Marshal.SizeOf(typeof(User32.CURSORINFO));

                if (User32.GetCursorInfo(out cursorInfo))
                {
                    // if the cursor is showing draw it on the screen shot
                    if (cursorInfo.flags == User32.CURSOR_SHOWING)
                    {
                        var iconPointer = User32.CopyIcon(cursorInfo.hCursor);
                        User32.ICONINFO iconInfo;
                        int iconX, iconY;

                        if (User32.GetIconInfo(iconPointer, out iconInfo))
                        {
                            iconX = (int)((cursorInfo.ptScreenPos.x - ((int)iconInfo.xHotspot)) * 1.25);
                            iconY = (int)((cursorInfo.ptScreenPos.y - ((int)iconInfo.yHotspot)) * 1.25);
                            
                            User32.DrawIcon(g.GetHdc(), iconX, iconY, cursorInfo.hCursor);

                            g.ReleaseHdc();
                        }
                    }
                }
            }
            return bitmap;
        }
    }
}