namespace iobloc
{
    public class Demo : BasicGame
    {
        int _cursor;
        bool _nailedIt;

        // this basic game has a main panel which is 14 blocks wide and 1 tall
        // is initialized as a color panel, the interval between frames is 100ms
        // and the only supported key is "I" which needs to be guessed as a game
        public Demo() : base(14, 1, null, 50, "I") { }

        public override void Start()
        {
            // set the pause screen help text without entering text mode
            MainPanel.SetText("Guess the key!");
            _nailedIt = false;
            if (MainPanel.IsTextMode)
                MainPanel.SwitchMode();
            base.Start();
        }

        // on each frame, if the key was not guessed then run an animation
        public override void NextFrame()
        {
            // if key was already guessed, skip the animation
            if (_nailedIt)
                return;

            // unshow current cursor position
            MainPanel[0, _cursor] = 0;
            // move cursor to next position
            _cursor++;
            // go back to start if end is reached
            if (_cursor >= MainPanel.Width)
                _cursor = 0;
            // set color for current position
            MainPanel[0, _cursor] = MainPanel.Width - _cursor;
            // mark main panel as changed to paint
            MainPanel.Change();
        }

        // when an allowed key is pressed, it is handled here
        public override void HandleInput(string key)
        {
            // if key was already guessed, exit on next key press
            if (_nailedIt)
            {
                Stop();
                return;
            }

            // key was correctly guessed
            _nailedIt = true;
            // replace pause help text with congratulation message
            MainPanel.SetText("Nailed it!");
        }

        // display help when an unsupported key is pressed
        public override void TogglePause()
        {
            // if key was already guessed, exit on next key press
            if (_nailedIt)
            {
                Stop();
                return;
            }

            base.TogglePause();
        }
    }
}
