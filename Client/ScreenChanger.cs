using System.Drawing;

namespace Client
{
    public class ScreenChanger
    {
        public delegate void ScreenHandler(Image screen);

        public static event ScreenHandler? ScreenChanged;

        public static void ChangeScreen(Image screen)
        {
            ScreenChanged?.Invoke(screen);
        }
    }
}
