using System;
using System.Text;
using System.Collections.Generic;

namespace iobloc.Ascio
{
    abstract class Game
    {
        protected static readonly bool DEBUG = true;
        private DateTime _start = DateTime.Now;
        private int _frames = 0;
        private int _fps = 0;
        protected bool IsExiting { get; private set; }
        protected int Height { get; private set; }
        protected int Width { get; private set; }
        protected Queue<ConsoleKey> Keys { get; } = new Queue<ConsoleKey>();
        protected ConsoleKey? LastAction { get; private set; }

        public Game()
        {
            Console.CursorVisible = false;
            Console.OutputEncoding = Encoding.UTF8;
            Width = Console.WindowWidth;
            Height = Console.WindowHeight;
            Console.SetBufferSize(Width, Height);
        }

        public void Start()
        {
            Init();
            while (true)
            {
                DoAction();
                Screen screen = GetScreen();
                if (DEBUG)
                    screen.SetScreen(GetStatus());
                DrawScreen(screen);
                // LastAction = null;

                if (DEBUG)
                {
                    _frames++;
                    double dif = DateTime.Now.Subtract(_start).TotalSeconds;
                    if (dif >= 3)
                    {
                        _fps = (int)(_frames / dif);
                        _frames = 0;
                        _start = DateTime.Now;
                    }
                }

                if (IsExiting)
                    return;

                while (Console.KeyAvailable)
                    Keys.Enqueue(Console.ReadKey(true).Key);
            }
        }

        private void DrawScreen(Screen screen)
        {
            var rect = screen.Rect;
            Interop.WriteConsoleOutput(Interop.OUTPUT_HANDLE, screen.Text, screen.Size, screen.From, ref rect);
        }

        private Screen GetStatus()
        {
            StringBuilder status = new StringBuilder();

            if (LastAction.HasValue)
                status.AppendFormat(" LastAction={0}", LastAction.Value);
            if (_fps > 0)
                status.AppendFormat(" FPS={0}", _fps);

            var screen = new Screen(Width - status.Length, 0, status.Length, 1, CharAttr.FOREGROUND_WHITE | CharAttr.FOREGROUND_INTENSITY);
            screen.SetText(status.ToString());

            return screen;
        }

        protected virtual bool DoAction()
        {
            if (Keys.Count == 0)
                return false;

            LastAction = Keys.Dequeue();
            if (LastAction == ConsoleKey.Escape)
            {
                IsExiting = true;
                return false;
            }

            return true;
        }

        protected virtual void Init() { }
        protected abstract Screen GetScreen();
    }
}
