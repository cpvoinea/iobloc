using System.Runtime.InteropServices;
using IntPtr = System.IntPtr;

namespace iobloc.Ascio
{
    internal class Interop
    {
        internal static readonly IntPtr OUTPUT_HANDLE = GetStdHandle(-11);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GetStdHandle(int nStdHandle);
        [DllImport("kernel32.dll", EntryPoint = "WriteConsoleOutputW", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool WriteConsoleOutput(IntPtr hConsoleOutput, [MarshalAs(UnmanagedType.LPArray), In] CharInfo[,] lpBuffer, Coord dwBufferSize, Coord dwBufferCoord, ref Rect lpWriteRegion);
    }
}
