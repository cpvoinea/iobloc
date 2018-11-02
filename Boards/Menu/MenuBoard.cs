using System.Collections.Generic;

namespace iobloc
{
    class MenuBoard : BaseBoard
    {
        private Dictionary<int, string[]> _itemAllowedKeys;
        private string[] _itemNames;

        public MenuBoard() : base(BoardType.Menu) { }

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
            Main.Text = items.ToArray(); ;
            Main.IsText = true;
        }

        public override void Start()
        {
            Level = Settings.MasterLevel;
            base.Start();
        }

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
            Main.HasChanges = true;
        }

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
