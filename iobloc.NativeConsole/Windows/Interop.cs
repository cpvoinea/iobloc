using System.Runtime.InteropServices;
using IntPtr = System.IntPtr;

namespace iobloc.NativeConsole.Windows
{
    internal class Interop
    {
        internal const short KEY_EVENT = 1;

        internal static class Libraries
        {
            internal const string Advapi32 = "advapi32.dll";
            internal const string BCrypt = "BCrypt.dll";
            internal const string CoreComm_L1_1_1 = "api-ms-win-core-comm-l1-1-1.dll";
            internal const string CoreComm_L1_1_2 = "api-ms-win-core-comm-l1-1-2.dll";
            internal const string Crypt32 = "crypt32.dll";
            internal const string CryptUI = "cryptui.dll";
            internal const string Error_L1 = "api-ms-win-core-winrt-error-l1-1-0.dll";
            internal const string Gdi32 = "gdi32.dll";
            internal const string HttpApi = "httpapi.dll";
            internal const string IpHlpApi = "iphlpapi.dll";
            internal const string Kernel32 = "kernel32.dll";
            internal const string Memory_L1_3 = "api-ms-win-core-memory-l1-1-3.dll";
            internal const string Mswsock = "mswsock.dll";
            internal const string NCrypt = "ncrypt.dll";
            internal const string NtDll = "ntdll.dll";
            internal const string Odbc32 = "odbc32.dll";
            internal const string Ole32 = "ole32.dll";
            internal const string OleAut32 = "oleaut32.dll";
            internal const string PerfCounter = "perfcounter.dll";
            internal const string RoBuffer = "api-ms-win-core-winrt-robuffer-l1-1-0.dll";
            internal const string Secur32 = "secur32.dll";
            internal const string Shell32 = "shell32.dll";
            internal const string SspiCli = "sspicli.dll";
            internal const string User32 = "user32.dll";
            internal const string Version = "version.dll";
            internal const string WebSocket = "websocket.dll";
            internal const string WinHttp = "winhttp.dll";
            internal const string WinMM = "winmm.dll";
            internal const string Wldap32 = "wldap32.dll";
            internal const string Ws2_32 = "ws2_32.dll";
            internal const string Wtsapi32 = "wtsapi32.dll";
            internal const string CompressionNative = "clrcompression.dll";
            internal const string CoreWinRT = "api-ms-win-core-winrt-l1-1-0.dll";
            internal const string MsQuic = "msquic.dll";
            internal const string HostPolicy = "hostpolicy.dll";
        }

        // As defined in winerror.h and https://docs.microsoft.com/en-us/windows/win32/debug/system-error-codes
        internal static class Errors
        {
            internal const int ERROR_SUCCESS = 0x0;
            internal const int ERROR_INVALID_FUNCTION = 0x1;
            internal const int ERROR_FILE_NOT_FOUND = 0x2;
            internal const int ERROR_PATH_NOT_FOUND = 0x3;
            internal const int ERROR_ACCESS_DENIED = 0x5;
            internal const int ERROR_INVALID_HANDLE = 0x6;
            internal const int ERROR_NOT_ENOUGH_MEMORY = 0x8;
            internal const int ERROR_INVALID_DATA = 0xD;
            internal const int ERROR_INVALID_DRIVE = 0xF;
            internal const int ERROR_NO_MORE_FILES = 0x12;
            internal const int ERROR_NOT_READY = 0x15;
            internal const int ERROR_BAD_COMMAND = 0x16;
            internal const int ERROR_BAD_LENGTH = 0x18;
            internal const int ERROR_SHARING_VIOLATION = 0x20;
            internal const int ERROR_LOCK_VIOLATION = 0x21;
            internal const int ERROR_HANDLE_EOF = 0x26;
            internal const int ERROR_NOT_SUPPORTED = 0x32;
            internal const int ERROR_BAD_NETPATH = 0x35;
            internal const int ERROR_NETWORK_ACCESS_DENIED = 0x41;
            internal const int ERROR_BAD_NET_NAME = 0x43;
            internal const int ERROR_FILE_EXISTS = 0x50;
            internal const int ERROR_INVALID_PARAMETER = 0x57;
            internal const int ERROR_BROKEN_PIPE = 0x6D;
            internal const int ERROR_SEM_TIMEOUT = 0x79;
            internal const int ERROR_CALL_NOT_IMPLEMENTED = 0x78;
            internal const int ERROR_INSUFFICIENT_BUFFER = 0x7A;
            internal const int ERROR_INVALID_NAME = 0x7B;
            internal const int ERROR_NEGATIVE_SEEK = 0x83;
            internal const int ERROR_DIR_NOT_EMPTY = 0x91;
            internal const int ERROR_BAD_PATHNAME = 0xA1;
            internal const int ERROR_LOCK_FAILED = 0xA7;
            internal const int ERROR_BUSY = 0xAA;
            internal const int ERROR_ALREADY_EXISTS = 0xB7;
            internal const int ERROR_BAD_EXE_FORMAT = 0xC1;
            internal const int ERROR_ENVVAR_NOT_FOUND = 0xCB;
            internal const int ERROR_FILENAME_EXCED_RANGE = 0xCE;
            internal const int ERROR_EXE_MACHINE_TYPE_MISMATCH = 0xD8;
            internal const int ERROR_PIPE_BUSY = 0xE7;
            internal const int ERROR_NO_DATA = 0xE8;
            internal const int ERROR_PIPE_NOT_CONNECTED = 0xE9;
            internal const int ERROR_MORE_DATA = 0xEA;
            internal const int ERROR_NO_MORE_ITEMS = 0x103;
            internal const int ERROR_DIRECTORY = 0x10B;
            internal const int ERROR_NOT_OWNER = 0x120;
            internal const int ERROR_TOO_MANY_POSTS = 0x12A;
            internal const int ERROR_PARTIAL_COPY = 0x12B;
            internal const int ERROR_ARITHMETIC_OVERFLOW = 0x216;
            internal const int ERROR_PIPE_CONNECTED = 0x217;
            internal const int ERROR_PIPE_LISTENING = 0x218;
            internal const int ERROR_MUTANT_LIMIT_EXCEEDED = 0x24B;
            internal const int ERROR_OPERATION_ABORTED = 0x3E3;
            internal const int ERROR_IO_INCOMPLETE = 0x3E4;
            internal const int ERROR_IO_PENDING = 0x3E5;
            internal const int ERROR_NO_TOKEN = 0x3f0;
            internal const int ERROR_SERVICE_DOES_NOT_EXIST = 0x424;
            internal const int ERROR_NO_UNICODE_TRANSLATION = 0x459;
            internal const int ERROR_DLL_INIT_FAILED = 0x45A;
            internal const int ERROR_COUNTER_TIMEOUT = 0x461;
            internal const int ERROR_NO_ASSOCIATION = 0x483;
            internal const int ERROR_DDE_FAIL = 0x484;
            internal const int ERROR_DLL_NOT_FOUND = 0x485;
            internal const int ERROR_NOT_FOUND = 0x490;
            internal const int ERROR_NETWORK_UNREACHABLE = 0x4CF;
            internal const int ERROR_NON_ACCOUNT_SID = 0x4E9;
            internal const int ERROR_NOT_ALL_ASSIGNED = 0x514;
            internal const int ERROR_UNKNOWN_REVISION = 0x519;
            internal const int ERROR_INVALID_OWNER = 0x51B;
            internal const int ERROR_INVALID_PRIMARY_GROUP = 0x51C;
            internal const int ERROR_NO_SUCH_PRIVILEGE = 0x521;
            internal const int ERROR_PRIVILEGE_NOT_HELD = 0x522;
            internal const int ERROR_INVALID_ACL = 0x538;
            internal const int ERROR_INVALID_SECURITY_DESCR = 0x53A;
            internal const int ERROR_INVALID_SID = 0x539;
            internal const int ERROR_BAD_IMPERSONATION_LEVEL = 0x542;
            internal const int ERROR_CANT_OPEN_ANONYMOUS = 0x543;
            internal const int ERROR_NO_SECURITY_ON_OBJECT = 0x546;
            internal const int ERROR_CANNOT_IMPERSONATE = 0x558;
            internal const int ERROR_CLASS_ALREADY_EXISTS = 0x582;
            internal const int ERROR_NO_SYSTEM_RESOURCES = 0x5AA;
            internal const int ERROR_TIMEOUT = 0x5B4;
            internal const int ERROR_EVENTLOG_FILE_CHANGED = 0x5DF;
            internal const int ERROR_TRUSTED_RELATIONSHIP_FAILURE = 0x6FD;
            internal const int ERROR_RESOURCE_LANG_NOT_FOUND = 0x717;
        }

        /// <summary>
        /// Blittable version of Windows BOOL type. It is convenient in situations where
        /// manual marshalling is required, or to avoid overhead of regular bool marshalling.
        /// </summary>
        /// <remarks>
        /// Some Windows APIs return arbitrary integer values although the return type is defined
        /// as BOOL. It is best to never compare BOOL to TRUE. Always use bResult != BOOL.FALSE
        /// or bResult == BOOL.FALSE .
        /// </remarks>
        internal enum BOOL : int
        {
            FALSE = 0,
            TRUE = 1,
        }

        // Windows's KEY_EVENT_RECORD
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct KeyEventRecord
        {
            internal BOOL keyDown;
            internal short repeatCount;
            internal short virtualKeyCode;
            internal short virtualScanCode;
            internal char uChar; // Union between WCHAR and ASCII char
            internal int controlKeyState;
        }

        // Really, this is a union of KeyEventRecords and other types.
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct InputRecord
        {
            internal short eventType;
            internal KeyEventRecord keyEvent;
            // This struct is a union!  Word alignment should take care of padding!
        }


        internal class Kernel32
        {
            internal const int ENABLE_PROCESSED_INPUT = 0x0001;
            internal const int CTRL_C_EVENT = 0;
            internal const int CTRL_BREAK_EVENT = 1;
            private const int MAX_PATH = 260;
            private const int FORMAT_MESSAGE_IGNORE_INSERTS = 0x00000200;
            private const int FORMAT_MESSAGE_FROM_HMODULE = 0x00000800;
            private const int FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;
            private const int FORMAT_MESSAGE_ARGUMENT_ARRAY = 0x00002000;
            private const int FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x00000100;
            private const int ERROR_INSUFFICIENT_BUFFER = 0x7A;

            internal delegate bool ConsoleCtrlHandlerRoutine(int controlType);

            internal class FileTypes
            {
                internal const int FILE_TYPE_UNKNOWN = 0x0000;
                internal const int FILE_TYPE_DISK = 0x0001;
                internal const int FILE_TYPE_CHAR = 0x0002;
                internal const int FILE_TYPE_PIPE = 0x0003;
            }

            internal class HandleTypes
            {
                internal const int STD_INPUT_HANDLE = -10;
                internal const int STD_OUTPUT_HANDLE = -11;
                internal const int STD_ERROR_HANDLE = -12;
            }

            internal enum Color : short
            {
                Black = 0,
                ForegroundBlue = 0x1,
                ForegroundGreen = 0x2,
                ForegroundRed = 0x4,
                ForegroundYellow = 0x6,
                ForegroundIntensity = 0x8,
                BackgroundBlue = 0x10,
                BackgroundGreen = 0x20,
                BackgroundRed = 0x40,
                BackgroundYellow = 0x60,
                BackgroundIntensity = 0x80,

                ForegroundMask = 0xf,
                BackgroundMask = 0xf0,
                ColorMask = 0xff
            }


            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            private unsafe struct CPINFOEXW
            {
                internal uint MaxCharSize;
                internal fixed byte DefaultChar[2];
                internal fixed byte LeadByte[12];
                internal char UnicodeDefaultChar;
                internal uint CodePage;
                internal fixed char CodePageName[MAX_PATH];
            }

            [StructLayout(LayoutKind.Sequential)]
            internal struct CHAR_INFO
            {
                private readonly ushort charData;
                private readonly short attributes;
            }

            [StructLayoutAttribute(LayoutKind.Sequential)]
            internal struct CONSOLE_CURSOR_INFO
            {
                internal int dwSize;
                internal bool bVisible;
            }

            [StructLayoutAttribute(LayoutKind.Sequential)]
            internal struct CONSOLE_SCREEN_BUFFER_INFO
            {
                internal COORD dwSize;
                internal COORD dwCursorPosition;
                internal short wAttributes;
                internal SMALL_RECT srWindow;
                internal COORD dwMaximumWindowSize;
            }

            [StructLayoutAttribute(LayoutKind.Sequential)]
            internal struct COORD
            {
                internal short X;
                internal short Y;
            }

            [StructLayoutAttribute(LayoutKind.Sequential)]
            internal struct SMALL_RECT
            {
                internal short Left;
                internal short Top;
                internal short Right;
                internal short Bottom;
            }


            [DllImport(Libraries.Kernel32, SetLastError = true)]
            internal static extern bool Beep(int frequency, int duration);

            [DllImport(Libraries.Kernel32, SetLastError = true)]
            internal static extern bool GetConsoleCursorInfo(IntPtr hConsoleOutput, out CONSOLE_CURSOR_INFO cci);

            [DllImport(Libraries.Kernel32, SetLastError = true)]
            internal static extern bool SetConsoleCursorInfo(IntPtr hConsoleOutput, ref CONSOLE_CURSOR_INFO cci);

            [DllImport(Libraries.Kernel32, SetLastError = true, CharSet = CharSet.Unicode)]
            internal static extern bool FillConsoleOutputAttribute(IntPtr hConsoleOutput, short wColorAttribute, int numCells, COORD startCoord, out int pNumBytesWritten);

            [DllImport(Libraries.Kernel32, SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "FillConsoleOutputCharacterW")]
            internal static extern bool FillConsoleOutputCharacter(IntPtr hConsoleOutput, char character, int nLength, COORD dwWriteCoord, out int pNumCharsWritten);

            [DllImport(Libraries.Kernel32, CharSet = CharSet.Unicode, EntryPoint = "FormatMessageW", SetLastError = true, BestFitMapping = true, ExactSpelling = true)]
            private static extern unsafe int FormatMessage(int dwFlags, IntPtr lpSource, uint dwMessageId, int dwLanguageId, void* lpBuffer, int nSize, IntPtr arguments);

            [DllImport(Libraries.Kernel32)]
            internal static extern uint GetConsoleCP();

            [DllImport(Libraries.Kernel32, SetLastError = true)]
            internal static extern bool GetConsoleMode(IntPtr handle, out int mode);

            [DllImport(Libraries.Kernel32, SetLastError = true)]
            internal static extern bool SetConsoleMode(IntPtr handle, int mode);

            [DllImport(Libraries.Kernel32)]
            internal static extern uint GetConsoleOutputCP();

            [DllImport(Libraries.Kernel32, SetLastError = true)]
            internal static extern bool GetConsoleScreenBufferInfo(IntPtr hConsoleOutput, out CONSOLE_SCREEN_BUFFER_INFO lpConsoleScreenBufferInfo);

            [DllImport(Libraries.Kernel32, CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
            internal static extern unsafe uint GetConsoleTitleW(char* title, uint nSize);

            [DllImport(Libraries.Kernel32, CharSet = CharSet.Unicode, EntryPoint = "GetCPInfoExW")]
            private static extern unsafe Interop.BOOL GetCPInfoExW(uint CodePage, uint dwFlags, CPINFOEXW* lpCPInfoEx);

            [DllImport(Libraries.Kernel32, SetLastError = true)]
            internal static extern uint GetFileType(IntPtr hFile);

            [DllImport(Libraries.Kernel32, SetLastError = true)]
            internal static extern Interop.Kernel32.COORD GetLargestConsoleWindowSize(IntPtr hConsoleOutput);

            [DllImport(Libraries.Kernel32)]
            internal static extern IntPtr GetStdHandle(int nStdHandle);  // param is NOT a handle, but it returns one!

            [DllImport(Libraries.Kernel32)]
            internal static extern unsafe int MultiByteToWideChar(uint CodePage, uint dwFlags, byte* lpMultiByteStr, int cbMultiByte, char* lpWideCharStr, int cchWideChar);

            [DllImport(Libraries.Kernel32, SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "PeekConsoleInputW")]
            internal static extern bool PeekConsoleInput(IntPtr hConsoleInput, out InputRecord buffer, int numInputRecords_UseOne, out int numEventsRead);

            [DllImport(Libraries.Kernel32, CharSet = CharSet.Unicode, SetLastError = true, EntryPoint = "ReadConsoleW")]
            internal static extern unsafe bool ReadConsole(IntPtr hConsoleInput, byte* lpBuffer, int nNumberOfCharsToRead, out int lpNumberOfCharsRead, IntPtr pInputControl);

            [DllImport(Libraries.Kernel32, CharSet = CharSet.Unicode, SetLastError = true, EntryPoint = "ReadConsoleInputW")]
            internal static extern bool ReadConsoleInput(IntPtr hConsoleInput, out InputRecord buffer, int numInputRecords_UseOne, out int numEventsRead);

            [DllImport(Libraries.Kernel32, SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "ReadConsoleOutputW")]
            internal static extern unsafe bool ReadConsoleOutput(IntPtr hConsoleOutput, CHAR_INFO* pBuffer, COORD bufferSize, COORD bufferCoord, ref SMALL_RECT readRegion);

            [DllImport(Libraries.Kernel32, SetLastError = true)]
            internal static extern unsafe int ReadFile(IntPtr handle, byte* bytes, int numBytesToRead, out int numBytesRead, IntPtr mustBeZero);

            [DllImport(Libraries.Kernel32, SetLastError = true)]
            internal static extern bool SetConsoleCP(int codePage);

            [DllImport(Libraries.Kernel32, SetLastError = true)]
            internal static extern bool SetConsoleCtrlHandler(ConsoleCtrlHandlerRoutine handler, bool addOrRemove);

            [DllImport(Libraries.Kernel32, SetLastError = true)]
            internal static extern bool SetConsoleCursorPosition(IntPtr hConsoleOutput, COORD cursorPosition);

            [DllImport(Libraries.Kernel32, SetLastError = true)]
            internal static extern bool SetConsoleOutputCP(int codePage);

            [DllImport(Libraries.Kernel32, SetLastError = true)]
            internal static extern bool SetConsoleScreenBufferSize(IntPtr hConsoleOutput, Interop.Kernel32.COORD size);

            [DllImport(Libraries.Kernel32, SetLastError = true)]
            internal static extern int SetConsoleTextAttribute(IntPtr hConsoleOutput, short wAttributes);

            [DllImport(Libraries.Kernel32, CharSet = CharSet.Unicode, SetLastError = true, EntryPoint = "SetConsoleTitleW")]
            internal static extern bool SetConsoleTitle(string title);

            [DllImport(Libraries.Kernel32, SetLastError = true)]
            internal static extern unsafe bool SetConsoleWindowInfo(IntPtr hConsoleOutput, bool absolute, SMALL_RECT* consoleWindow);

            [DllImport(Libraries.Kernel32)]
            internal static extern unsafe int WideCharToMultiByte(uint CodePage, uint dwFlags, char* lpWideCharStr, int cchWideChar, byte* lpMultiByteStr, int cbMultiByte, IntPtr lpDefaultChar, IntPtr lpUsedDefaultChar);

            [DllImport(Libraries.Kernel32, CharSet = CharSet.Unicode, SetLastError = true, EntryPoint = "WriteConsoleW")]
            internal static extern unsafe bool WriteConsole(IntPtr hConsoleOutput, byte* lpBuffer, int nNumberOfCharsToWrite, out int lpNumberOfCharsWritten, IntPtr lpReservedMustBeNull);

            [DllImport(Libraries.Kernel32, SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "WriteConsoleOutputW")]
            internal static extern unsafe bool WriteConsoleOutput(IntPtr hConsoleOutput, CHAR_INFO* buffer, COORD bufferSize, COORD bufferCoord, ref SMALL_RECT writeRegion);

            [DllImport(Libraries.Kernel32, SetLastError = true)]
            internal static extern unsafe int WriteFile(IntPtr handle, byte* bytes, int numBytesToWrite, out int numBytesWritten, IntPtr mustBeZero);


            internal static bool IsGetConsoleModeCallSuccessful(IntPtr handle)
            {
                return GetConsoleMode(handle, out int mode);
            }

            internal static unsafe int GetLeadByteRanges(int codePage, byte[] leadByteRanges)
            {
                int count = 0;
                CPINFOEXW cpInfo;
                if (GetCPInfoExW((uint)codePage, 0, &cpInfo) != BOOL.FALSE)
                {
                    // we don't care about the last 2 bytes as those are nulls
                    for (int i = 0; i < 10 && leadByteRanges[i] != 0; i += 2)
                    {
                        leadByteRanges[i] = cpInfo.LeadByte[i];
                        leadByteRanges[i + 1] = cpInfo.LeadByte[i + 1];
                        count++;
                    }
                }
                return count;
            }

            /// <summary>
            ///     Returns a string message for the specified Win32 error code.
            /// </summary>
            internal static string GetMessage(int errorCode) =>
                GetMessage(errorCode, IntPtr.Zero);

            internal static unsafe string GetMessage(int errorCode, IntPtr moduleHandle)
            {
                int flags = FORMAT_MESSAGE_IGNORE_INSERTS | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_ARGUMENT_ARRAY;
                if (moduleHandle != IntPtr.Zero)
                {
                    flags |= FORMAT_MESSAGE_FROM_HMODULE;
                }

                // First try to format the message into the stack based buffer.  Most error messages willl fit.
                System.Span<char> stackBuffer = stackalloc char[256]; // arbitrary stack limit
                fixed (char* bufferPtr = stackBuffer)
                {
                    int length = FormatMessage(flags, moduleHandle, unchecked((uint)errorCode), 0, bufferPtr, stackBuffer.Length, IntPtr.Zero);
                    if (length > 0)
                    {
                        return GetAndTrimString(stackBuffer.Slice(0, length));
                    }
                }

                // We got back an error.  If the error indicated that there wasn't enough room to store
                // the error message, then call FormatMessage again, but this time rather than passing in
                // a buffer, have the method allocate one, which we then need to free.
                if (Marshal.GetLastWin32Error() == ERROR_INSUFFICIENT_BUFFER)
                {
                    IntPtr nativeMsgPtr = default;
                    try
                    {
                        int length = FormatMessage(flags | FORMAT_MESSAGE_ALLOCATE_BUFFER, moduleHandle, unchecked((uint)errorCode), 0, &nativeMsgPtr, 0, IntPtr.Zero);
                        if (length > 0)
                        {
                            return GetAndTrimString(new System.Span<char>((char*)nativeMsgPtr, length));
                        }
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(nativeMsgPtr);
                    }
                }

                // Couldn't get a message, so manufacture one.
                return string.Format("Unknown error (0x{0:x})", errorCode);
            }

            private static string GetAndTrimString(System.Span<char> buffer)
            {
                int length = buffer.Length;
                while (length > 0 && buffer[length - 1] <= 32)
                {
                    length--; // trim off spaces and non-printable ASCII chars at the end of the resource
                }
                return buffer.Slice(0, length).ToString();
            }

        }

        internal class User32
        {
            [DllImport(Libraries.User32)]
            internal static extern short GetKeyState(int virtualKeyCode);
        }
    }
}
