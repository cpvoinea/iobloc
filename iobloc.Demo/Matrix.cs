using System.Text;

namespace iobloc
{
    public class Matrix : NativeGame
    {
        static readonly System.Random rand = new System.Random();
        public override void HandleInput(string key) { }

        public Matrix() : base() { }

        public override void NextFrame()
        {
            int range = 'z' - 'a' + 1;

            StringBuilder text = new StringBuilder();
            for (int i = 0; i < Main.Height * Main.Width; i++)
                text.Append((char)(rand.Next(range) + 'a'));

            var area = new Area(1, 1, Main.Width, Main.Height, (short)(NativeColor.ForegroundGreen | NativeColor.BackgroundGreen | NativeColor.ForegroundIntensity));
            area.SetText(text.ToString());

            Main.Area = area;
            Main.Change();
        }
    }
}
