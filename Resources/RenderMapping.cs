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