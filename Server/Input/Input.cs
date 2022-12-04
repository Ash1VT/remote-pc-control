using System.Runtime.InteropServices;

namespace Server.Input
{

    [StructLayout(LayoutKind.Sequential)]
    public struct INPUT
    {
        public uint type;
        public InputUnion U;
        public static int Size
        {
            get { return Marshal.SizeOf(typeof(INPUT)); }
        }
    }
    
}
