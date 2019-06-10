using System.Runtime.InteropServices;

namespace MiniFramework
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi,Pack = 4)]
    public class PackHead
    {
        public short MsgID;
        public short PackLength;
        public int UserId;
        public long TimeStamp;
    }
}

