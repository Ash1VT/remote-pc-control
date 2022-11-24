﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ScreenManager
    {
        public delegate void ScreenHandler(Image screen);

        public static event ScreenHandler? ScreenChanged;

        public static void ChangeScreen(Image screen)
        {
            ScreenChanged?.Invoke(screen);
        }
    }
}
