using System;
using System.Collections.Generic;

namespace iobloc
{
    class Menu : IDisposable
    {
        readonly Config _config;
        readonly Dictionary<Option, MenuItem> _items = new Dictionary<Option, MenuItem>();
        readonly UI _ui;
        internal event MenuItemSelected OnItemSelected;
        internal event MenuExit OnExit;

        internal Menu(Config config, UI ui)
        {
            _config = config;
            _ui = ui;

            foreach (var item in _config.MenuItems)
                _items.Add(item.Option, item);
        }

        internal void Show()
        {
            ShowOptions();
            HandleInput();
        }

        void ShowOptions()
        {
            _ui.Clear();
            foreach (var key in _items.Keys)
                _ui.TextLine($"{key,Config.MENU_LEN_KEY}: {_items[key].Name,-Config.MENU_LEN_NAME} {_items[key].Info,Config.MENU_LEN_INFO}", _items[key].Color);
            _ui.TextReset();
            _ui.Text($"{Config.MENU_INPUT_TEXT}: ");
        }

        void HandleInput()
        {
            int input = _ui.InputWait();
            while (input != Config.INPUT_EXIT)
            {
                MenuItem? item = GetSelection(input);
                if (OnItemSelected != null && item.HasValue)
                    OnItemSelected(item.Value);
                input = _ui.InputWait();
            }
            if (OnExit != null)
                OnExit();
        }

        MenuItem? GetSelection(int input)
        {
            var key = (Option)(input - '0');
            if (!_items.ContainsKey(key))
                return null;
            return _items[key];
        }

        public void Dispose()
        {
            _items.Clear();
        }
    }
}
