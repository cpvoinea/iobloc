using System;
using System.Text;

namespace iobloc
{
    class UI : IDisposable
    {
        readonly Config _config;

        internal UI(Config config)
        {
            _config = config;
        }

        internal void Clear()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
            TextReset();
            Console.Clear();
        }

        internal void TextReset()
        {
            Console.ResetColor();
        }

        internal void Text(string text, int? color = null)
        {
            if (color.HasValue)
                Console.ForegroundColor = (ConsoleColor)color.Value;
            Console.Write(text);
        }

        internal void TextLine(string text, int? color = null)
        {
            Text(text, color);
            Console.WriteLine();
        }

        internal void BorderDraw(Border border)
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

        internal void PanelClear(Panel panel)
        {
            string row = new String(' ', panel.Width);
            for (int i = panel.FromRow; i <= panel.ToRow; i++)
            {
                Console.SetCursorPosition(panel.FromCol, i);
                Text(row);
            }
        }

        internal void PanelDraw(Panel panel)
        {
            for (int i = panel.FromRow; i <= panel.ToRow; i++)
            {
                Console.SetCursorPosition(panel.FromCol, i);
                for(int j = panel.FromCol; j <= panel.ToCol; j++)
                {
                    int c = panel.Grid[i,j];
                    if(c == 0)
                        Console.Write(' ');
                    else
                    {
                        Console.ForegroundColor = (ConsoleColor)c;
                        Console.Write(Config.BLOCK);
                    }
                }
            }
        }

        internal void PanelTextLines(Panel panel, string[] lines)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                Console.SetCursorPosition(panel.FromCol, panel.FromRow + i);
                Text(lines[i]);
            }
        }

        internal int InputWait()
        {
            return (int)Console.ReadKey(true).Key;
        }

        internal int? Input()
        {
            if (Console.KeyAvailable)
                return InputWait();
            return null;
        }

        public void Dispose()
        {
            TextReset();
            Console.CursorVisible = true;
        }
    }
}
