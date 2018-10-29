using System;
using System.Diagnostics;
using System.Text;

namespace iobloc
{
    static class UI
    {
        const int MIN_WIDTH = 10;
        const int MIN_HEIGHT = 10;

        internal static void Open()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
            if (Console.WindowWidth < MIN_WIDTH)
                Resize(MIN_WIDTH, Console.WindowHeight);
            if (Console.WindowHeight < MIN_HEIGHT)
                Resize(Console.WindowWidth, MIN_HEIGHT);
        }

        internal static void Close()
        {
            Console.CursorVisible = true;
        }

        internal static void Clear()
        {
            Console.Clear();
        }

        internal static void Resize(int width, int height)
        {
            Console.SetWindowSize(width, height);
            if (Console.BufferWidth > width || Console.BufferHeight > height)
                Console.SetBufferSize(width, height);
        }

        internal static void GetSize(out int width, out int height)
        {
            width = Console.WindowWidth;
            height = Console.WindowHeight;
        }

        internal static void TextReset()
        {
            Console.ResetColor();
        }

        internal static void Text(string text, int? color = null)
        {
            if (color.HasValue)
                Console.ForegroundColor = (ConsoleColor)color.Value;
            Console.Write(text);
        }

        internal static void TextAt(string text, int row, int col, int? color = null)
        {
            Console.SetCursorPosition(col, row);
            Text(text, color);
        }

        internal static void BorderDraw(Border border)
        {
            Resize(border.Width, border.Height);
            for (int i = 0; i < border.Height; i++)
            {
                StringBuilder line = new StringBuilder();
                for (int j = 0; j < border.Width; j++)
                {
                    int c = border.Grid[i, j];
                    line.Append(c == 0 ? ' ' : (char)c);
                }
                Console.SetCursorPosition(0, i);
                Text(line.ToString());
            }
        }

        internal static void PanelClear(Panel panel)
        {
            string row = new String(' ', panel.Width);
            for (int i = panel.FromRow; i <= panel.ToRow; i++)
            {
                Console.SetCursorPosition(panel.FromCol, i);
                Text(row);
            }
        }

        internal static void PanelDraw(Panel panel)
        {
            for (int i = panel.FromRow, x = 0; i <= panel.ToRow && x < panel.Height; i++, x++)
            {
                Console.SetCursorPosition(panel.FromCol, i);
                for (int y = 0; y < panel.Width; y++)
                {
                    int c = panel.Grid[x, y];
                    if (c == 0)
                        Console.Write(' ');
                    else if (panel.Symbol == 0)
                    {
                        Console.Write(c);
                    }
                    else
                    {
                        Console.ForegroundColor = (ConsoleColor)c;
                        Console.Write(panel.Symbol);
                    }
                }
            }
            TextReset();
        }

        internal static void PanelTextLines(Panel panel, string[] lines)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                Console.SetCursorPosition(panel.FromCol, panel.FromRow + i);
                Text(lines[i]);
            }
        }

        internal static string InputWait()
        {
            return Console.ReadKey(true).Key.ToString();
        }

        internal static string Input()
        {
            if (Console.KeyAvailable)
                return InputWait();
            return null;
        }
    }
}
