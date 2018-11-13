using System.Collections.Generic;

namespace iobloc
{
    /// <summary>
    /// Display list of available boards to link through Next
    /// </summary>
    class MenuBoard : BaseBoard
    {
        // list of board IDs and shortcut keys for each
        private Dictionary<int, string[]> _itemAllowedKeys;
        // list of board display names
        private string[] _itemNames;

        /// <summary>
        /// A text board with each line item having its own shortcut key(s)
        /// </summary>
        /// <returns></returns>
        public MenuBoard() : base(BoardType.Menu) { }

        /// <summary>
        /// Get setting items and iterate to get all allowed keys and names
        /// </summary>
        protected override void Initialize()
        {
            _itemAllowedKeys = Serializer.MenuKeys;

            List<string> items = new List<string>();
            List<string> allowedKeys = new List<string>(AllowedKeys);
            foreach (int k in _itemAllowedKeys.Keys)
            {
                items.Add(string.Format($"{k}:{(BoardType)k}"));
                allowedKeys.AddRange(_itemAllowedKeys[k]);
            }

            AllowedKeys = allowedKeys.ToArray();
            Main.Text = items.ToArray();
            Main.IsText = true;
        }

        /// <summary>
        /// Overriden to display correct MasterLevel value
        /// </summary>
        public override void Start()
        {
            Level = Settings.MasterLevel;
            base.Start();
        }

        /// <summary>
        /// Overriden to switch between 2 text modes (as oposed to text->grid switch which is default)
        /// </summary>
        public override void TogglePause()
        {
            if (_itemNames == null)
            {
                _itemNames = Main.Text;
                Main.Text = Help;
            }
            else
            {
                Main.Text = _itemNames;
                _itemNames = null;
            }
            base.Change(true);
        }

        /// <summary>
        /// Link to correct menu item board based on key
        /// </summary>
        /// <param name="key"></param>
        public override void HandleInput(string key)
        {
            foreach (int k in _itemAllowedKeys.Keys)
                if (_itemAllowedKeys[k].Contains(key))
                {
                    Next = Serializer.GetBoard(k);
                    Stop();
                    return;
                }
        }
    }
}
