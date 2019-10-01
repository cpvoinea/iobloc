using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace iobloc
{
    public class AvaloniaWindow : Window
    {
        public AvaloniaWindow()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
