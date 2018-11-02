using System;
using System.Text;

namespace iobloc
{
    static class UIPainter
    {
        private static int WinWidth = 15;
        private static int WinHeight = 4;

        public static void Initialize()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
            WinWidth = Console.WindowWidth;
            WinHeight = Console.WindowHeight;
        }

        public static void Exit()
        {
            Console.ResetColor();
            Console.CursorVisible = true;
            Resize(WinWidth, WinHeight);
        }

        public static void Resize(int width, int height)
        {
            width = width < 15 ? 15 : width;
            height = height + 1;

            Console.Clear();
            Console.SetWindowSize(width, height);
            if (Console.BufferWidth > width || Console.BufferHeight > height)
                Console.SetBufferSize(width, height);
        }

        public static void DrawBorder(UIBorder border)
        {
            Resize(border.Width, border.Height);
            for (int row = 0; row < border.Height; row++)
            {
                StringBuilder line = new StringBuilder(new string(' ', border.Width));
                for (int j = 0; j < border.Width; j++)
                {
                    if (border[row, j] > 0)
                        line[j] = (char)border[row, j];
                }
                Console.SetCursorPosition(0, row);
                Console.Write(line);
            }
            Console.SetCursorPosition(1, 1);
        }

        public static void DrawPanelText(UIPanel panel, string[] lines)
        {
            string empty = new String(' ', panel.Width);
            for (int row = 0; row < panel.Height; row++)
            {
                string text = row < lines.Length ? lines[row].PadRight(panel.Width) : empty;
                Console.SetCursorPosition(panel.FromCol, panel.FromRow + row);
                Console.Write(text);
            }
            Console.SetCursorPosition(1, 1);
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
            Console.SetCursorPosition(1, 1);
        }

        public static void DrawPanel(UIPanel panel)
        {
            if (panel.IsText)
                DrawPanelText(panel, panel.Text);
            else
                DrawPanelColor(panel);
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
