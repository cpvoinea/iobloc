using System.Text;

namespace iobloc.Ascio
{
    class Matrix : Game
    {
        protected override Screen GetScreen()
        {
            System.Random rand = new System.Random();
            int range = 'z' - 'a' + 1;

            StringBuilder text = new StringBuilder();
            for (int i = 0; i < Height * Width; i++)
                text.Append((char)(rand.Next(range) + 'a'));

            var screen = new Screen(0, 0, Width, Height, CharAttr.FOREGROUND_GREEN);
            screen.SetText(text.ToString());

            return screen;
        }
    }
}
