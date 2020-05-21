using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace iobloc
{
    static class RenderMapping
    {
        public static readonly Dictionary<int, Color> FormColor = new Dictionary<int, Color> {
            {0, Color.Black},
            {1, Color.DarkBlue},
            {2, Color.DarkGreen},
            {3, Color.DarkCyan},
            {4, Color.DarkRed},
            {5, Color.DarkMagenta},
            {6, Color.DarkSlateGray},
            {7, Color.Gray},
            {8, Color.DarkGray},
            {9, Color.Blue},
            {10, Color.Green},
            {11, Color.Cyan},
            {12, Color.Red},
            {13, Color.Magenta},
            {14, Color.Yellow},
            {15, Color.White}
        };

        public static readonly Dictionary<int, Brush> FormBrush = new Dictionary<int, Brush> {
            {0, Brushes.Black},
            {1, Brushes.DarkBlue},
            {2, Brushes.DarkGreen},
            {3, Brushes.DarkCyan},
            {4, Brushes.DarkRed},
            {5, Brushes.DarkMagenta},
            {6, Brushes.DarkSlateGray},
            {7, Brushes.Gray},
            {8, Brushes.DarkGray},
            {9, Brushes.Blue},
            {10, Brushes.Green},
            {11, Brushes.Cyan},
            {12, Brushes.Red},
            {13, Brushes.Magenta},
            {14, Brushes.Yellow},
            {15, Brushes.White}
        };

        public static readonly Dictionary<Keys, string> FormKey = new Dictionary<Keys, string>{
            {Keys.Left, UIKey.LeftArrow},
            {Keys.Right, UIKey.RightArrow},
            {Keys.Up, UIKey.UpArrow},
            {Keys.Down, UIKey.DownArrow},
            {Keys.Escape, UIKey.Escape},
            {Keys.Enter, UIKey.Enter},
            {Keys.Space, UIKey.Space}
        };
    }
}