using System;
using System.Collections.Generic;
using System.Text;

namespace iobloc
{
    static class UIPainter
    {
        public static void Initialize()
        {
            Console.OutputEncoding = Encoding.UTF8;
            //Console.CursorVisible = false;
        }

        public static void Exit()
        {
            Console.CursorVisible = true;
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
            for (int i = 0; i < border.Height; i++)
            {
                StringBuilder line = new StringBuilder();
                for (int j = 0; j < border.Width; j++)
                {
                    int c = border[i, j];
                    line.Append(c == 0 ? ' ' : (char)c);
                }
                Console.SetCursorPosition(0, i);
                Console.Write(line.ToString());
            }
        }

        public static void ClearPanel(UIPanel panel)
        {
            string row = new String(' ', panel.Width);
            for (int i = panel.FromRow; i <= panel.ToRow; i++)
            {
                Console.SetCursorPosition(panel.FromCol, i);
                Console.Write(row);
            }
        }

        public static void DrawPanelText(UIPanel panel, string[] lines)
        {
            ClearPanel(panel);
            if (lines == null)
                return;
            for (int i = 0; i < lines.Length; i++)
            {
                Console.SetCursorPosition(panel.FromCol, panel.FromRow + i);
                Console.Write(lines[i]);
            }
        }

        public static void DrawPanel(UIPanel panel)
        {
            if (panel.IsText)
                DrawPanelText(panel, panel.Text);
            else
            {
                for (int i = panel.FromRow, x = 0; i <= panel.ToRow && x < panel.Height; i++, x++)
                {
                    Console.SetCursorPosition(panel.FromCol, i);
                    for (int y = 0; y < panel.Width; y++)
                    {
                        int c = panel[x, y];
                        if (c == 0)
                            Console.Write(' ');
                        else
                        {
                            Console.ForegroundColor = (ConsoleColor)c;
                            Console.Write(panel.Symbol);
                        }
                    }
                }

                Console.ResetColor();
            }
        }

        public static void DrawPanels(IEnumerable<UIPanel> panels)
        {
            foreach (var p in panels)
                if (p.HasChanges)
                {
                    DrawPanel(p);
                    p.HasChanges = false;
                }
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
