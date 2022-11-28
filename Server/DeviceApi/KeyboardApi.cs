using Server.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DeviceApi
{
    public class KeyboardApi
    {
        public static void SendKeyDown(VirtualKeyShort keyCode)
        {
            INPUT[] Inputs = new INPUT[1];
            INPUT Input = new INPUT();
            Input.type = 1;
            Input.U.ki.wVk = keyCode;
            Input.U.ki.dwFlags = KEYEVENTF.EXTENDEDKEY;
            Inputs[0] = Input;
            User32.SendInput(1, Inputs, INPUT.Size);
        }

        public static void SendKeyUp(VirtualKeyShort keyCode)
        {
            INPUT[] Inputs = new INPUT[1];
            INPUT Input = new INPUT();
            Input.type = 1;
            Input.U.ki.wVk = keyCode;
            Input.U.ki.dwFlags = KEYEVENTF.KEYUP | KEYEVENTF.EXTENDEDKEY;
            Inputs[0] = Input;
            User32.SendInput(1, Inputs, INPUT.Size);
        }
    }
}
