using System.Runtime.InteropServices;

namespace MiniFramework
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi,Pack = 2)]
    public class PackHead
    {
        public short MsgID;
        public short PackLength;
    }
}

