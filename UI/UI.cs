using System;
using System.Diagnostics;
using System.Text;

namespace iobloc
{
    static class UI
    {
        internal static void Open()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
        }

        internal static void Clear()
        {
            Console.Clear();
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

        internal static void TextLine(string text, int? color = null)
        {
            Text(text, color);
            Console.WriteLine();
        }

        internal static void TextAt(string text, int row, int col)
        {
            Console.SetCursorPosition(col, row);
            Text(text);
        }

        internal static void BorderDraw(Border border)
        {
            for (int i = 0; i < border.Height; i++)
            {
                StringBuilder line = new StringBuilder();
                for (int j = 0; j < border.Width; j++)
                {
                    int c = border.Grid[i, j];
                    line.Append(c == 0 ? ' ' : (char)c);
                }
                TextLine(line.ToString());
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
                    else
                    {
                        Console.ForegroundColor = (ConsoleColor)c;
                        Console.Write(Config.BLOCK);
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

        internal static void Close()
        {
            TextReset();
            Console.CursorVisible = true;
        }
    }
}
