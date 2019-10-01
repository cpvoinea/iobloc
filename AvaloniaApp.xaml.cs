using Avalonia;
using Avalonia.Markup.Xaml;

namespace iobloc
{
    public class AvaloniaApp : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
