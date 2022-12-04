using Server.Input;
using System.Runtime.InteropServices;

namespace Server
{
    public static class User32
    {
       

        //[DllImport("user32.dll")]
        //public static extern bool GetCursorInfo(out CURSORINFO pci);

        //[DllImport("user32.dll")]
        //public static extern IntPtr CopyIcon(IntPtr hIcon);

        //[DllImport("user32.dll")]
        //public static extern bool DrawIcon(IntPtr hdc, int x, int y, IntPtr hIcon);

        //[DllImport("user32.dll")]
        //public static extern bool GetIconInfo(IntPtr hIcon, out ICONINFO piconinfo);
        
        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        //[DllImport("user32.dll", SetLastError = true)]
        //public static extern void mouse_event(MouseEventFlags dwFlags, int dx, int dy, int dwData, UIntPtr dwExtraInfo);

        [DllImport("user32.dll")]
        public static extern uint SendInput(
            uint nInputs,
            [MarshalAs(UnmanagedType.LPArray), In] INPUT[] pInputs,
            int cbSize);

    }

}