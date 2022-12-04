using Server.Input;
using System;

namespace Server.DeviceApi
{
    public static class MouseApi
    {
        public static void Send(int x, int y, MOUSEEVENTF mouseEvent)
        {
            INPUT[] Inputs = new INPUT[1];
            INPUT Input = new INPUT();
            Input.type = 0;
            Input.U.mi.dwFlags = mouseEvent;
            Input.U.mi.dx = x;
            Input.U.mi.dy = y;
            Input.U.mi.dwExtraInfo = UIntPtr.Zero;
            Inputs[0] = Input;
            User32.SendInput(1, Inputs, INPUT.Size);
        }
    }
}
