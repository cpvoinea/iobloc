using System;
using System.Text;

namespace iobloc
{
    static class UIPainter
    {
        private static int WinWidth = 15;
        private static int WinHeight = 4;
        private static int BuffWidth = 15;
        private static int BuffHeight = 15;

        private static void Resize(int width, int height)
        {
            bool success = false;
            do try
                {
                    width++;
                    Console.SetWindowSize(width, height);
                    Console.SetBufferSize(width, height);
                    Console.SetWindowSize(width, height);
                    success = true;
                }
                catch { }
            while (!success && width < 16);
        }

        public static void Initialize()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
            WinWidth = Console.WindowWidth;
            WinHeight = Console.WindowHeight;
            BuffWidth = Console.BufferWidth;
            BuffHeight = Console.BufferHeight;
        }

        public static void Exit()
        {
            Console.ResetColor();
            Console.CursorVisible = true;
            try
            {
                Console.SetWindowSize(WinWidth, WinHeight);
                Console.SetBufferSize(BuffWidth, BuffHeight);
            }
            catch { }
        }

        public static void DrawBorder(UIBorder border)
        {
            Console.Clear();
            int w = border.Width;
            int h = border.Height;
            Resize(w, h);

            for (int row = 0; row < h; row++)
            {
                StringBuilder line = new StringBuilder(new string(' ', w));
                for (int j = 0; j < w; j++)
                {
                    if (border[row, j] > 0)
                        line[j] = (char)border[row, j];
                }
                Console.SetCursorPosition(0, row);
                Console.Write(line);
            }
            Console.SetCursorPosition(1, 1);
        }

        public static void DrawPanel(UIPanel panel)
        {
            if (panel.IsText)
                DrawPanelText(panel, panel.Text);
            else
                DrawPanelColor(panel);
        }

        private static void DrawPanelText(UIPanel panel, string[] lines)
        {
            string empty = new String(' ', panel.Width);
            for (int row = 0; row < panel.Height; row++)
            {
                string text = row < lines.Length ? lines[row].PadRight(panel.Width) : empty;
                Console.SetCursorPosition(panel.FromCol, panel.FromRow + row);
                Console.Write(text);
            }
        }

        private static void DrawPanelColor(UIPanel panel)
        {
            for (int row = 0; row < panel.Height; row++)
            {
                Console.SetCursorPosition(panel.FromCol, panel.FromRow + row);
                int col = 0;
                while (col < panel.Width)
                {
                    int from = col;
                    int last = panel[row, col];
                    do col++; while (col < panel.Width && panel[row, col] == last);
                    if (last == 0)
                        Console.Write(new string(' ', col - from));
                    else
                    {
                        Console.ForegroundColor = (ConsoleColor)last;
                        Console.Write(new string(panel.Symbol, col - from));
                    }
                }
            }
            Console.ResetColor();
        }

        public static string InputWait()
        {
            return Console.ReadKey(true).Key.ToString();
        }

        public static string Input()
        {
            if (Console.KeyAvailable)
                return InputWait();
            return null;
        }
    }
}
