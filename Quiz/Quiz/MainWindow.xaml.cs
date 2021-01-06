using System.Windows;

namespace Quiz
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            TabManager.InitMenuTabs(MainMenuTabControl, ref ErrorSnackbar);
        }
    }
}
