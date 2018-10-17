using System;
using System.Collections.Generic;

namespace iobloc
{
    class Menu : IDisposable
    {
        readonly Dictionary<int, MenuItem> _items = new Dictionary<int, MenuItem>();
        internal event MenuItemSelected OnItemSelected;

        internal Menu(IEnumerable<MenuItem> menuItems)
        {
            foreach (var item in menuItems)
                _items.Add((int)item.Option, item);
        }

        internal void Show()
        {
            ShowOptions();
            HandleInput();
        }

        void ShowOptions()
        {
            foreach (int key in _items.Keys)
                UI.TextLine($"{key,Config.MENU_LEN_KEY}: {_items[key].Name,-Config.MENU_LEN_NAME} {_items[key].Info,Config.MENU_LEN_INFO}", _items[key].Color);
            UI.TextReset();
            UI.Text($"{Config.MENU_INPUT_TEXT}: ");
        }

        void HandleInput()
        {
            string input = UI.InputWait();
            Option? option = null;
            while (input != Config.INPUT_EXIT && !option.HasValue)
            {
                option = GetSelection(input);
                if (option.HasValue && OnItemSelected != null)
                    OnItemSelected(option.Value);
                else
                    input = UI.InputWait();
            }
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
                return _items[key.Value].Option;
            return null;
        }

        public void Dispose()
        {
            _items.Clear();
        }
    }
}
