using iobloc.NativeConsole;

namespace iobloc
{
    public abstract class NativeGame : BasicGame
    {
        public NativeGame(string allowedKeys = "") : base(Console.WindowWidth - 3, Console.WindowHeight - 3, frameInterval: 1, allowedKeys: allowedKeys) { }
    }
}
