using System;
using System.Collections.Generic;

namespace iobloc
{
    class Menu
    {
        readonly Dictionary<int, MenuItem> _items = new Dictionary<int, MenuItem>();

        internal Menu(IEnumerable<MenuItem> menuItems)
        {
            foreach (var item in menuItems)
                _items.Add(item.Code, item);
        }

        internal void Show()
        {
            UI.Clear();
            int row = 0;
            foreach (int key in _items.Keys)
            {
                var item = _items[key];
                if(item.Visible)
                UI.TextAt($"{key}:{item.Name,-8}", row++, 0, _items[key].Color);
            }
            UI.TextReset();
        }

        internal Option? WaitOption()
        {
            string input = UI.InputWait();
            while (input != "Escape")
            {
                var option = GetSelection(input);
                if (option.HasValue)
                    return option;
                input = UI.InputWait();
            }

            return null;
        }

        Option? GetSelection(string input)
        {
            int code = (int)Enum.Parse(typeof(ConsoleKey), input);
            int? key = null;
            if (code >= (int)ConsoleKey.D0 && code <= (int)ConsoleKey.D9)
                key = code - (int)ConsoleKey.D0;
            else if (code >= (int)ConsoleKey.NumPad0 && code <= (int)ConsoleKey.NumPad9)
                key = code - (int)ConsoleKey.NumPad0;
            if (key.HasValue && _items.ContainsKey(key.Value))
                return (Option)_items[key.Value].Code;
            return null;
        }
    }
}
