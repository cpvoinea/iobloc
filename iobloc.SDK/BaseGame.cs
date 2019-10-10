using System.Collections.Generic;
using System.Windows.Forms;

namespace iobloc.SDK
{
    // Base game handles common operations like loading settings and initialization
    public abstract class BaseGame : IGame
    {
        public int Width { get; protected set; }
        public int Height { get; protected set; }
        protected bool IsInitialized { get; private set; }
        public List<Pane> Panes { get; protected set; }
        public bool IsRunning { get; private set; }
        protected Pane Main { get; set; }

        protected BaseGame()
        {
            InitializeSettings();
            InitializeUI();
            Initialize();
            IsInitialized = true;
        }

        protected virtual void InitializeUI()
        {
            Main = new Pane(0, 0, Height, Width);
            Panes = new List<Pane> { Main };
        }

        protected virtual void Initialize() { }

        protected virtual void Change(bool set)
        {
            if (set)
                Main.Change();
        }

        public virtual void Start()
        {
            IsRunning = true;
            foreach (var p in Panes) // force refresh of panes
                p.Change();
        }

        public void Stop()
        {
            IsRunning = false;
        }

        protected abstract void InitializeSettings();
        public abstract void HandleInput(Keys key);
    }
}
