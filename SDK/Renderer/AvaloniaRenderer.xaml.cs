using Avalonia;
using Avalonia.Markup.Xaml;

namespace iobloc
{
    public class AvaloniaRenderer : Application, IRenderer<PaneCell>
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void DrawPane(Pane<PaneCell> pane)
        {
        }

        public void Run(IGame<PaneCell> game)
        {
        }

        public void Dispose()
        {
        }
    }
}
