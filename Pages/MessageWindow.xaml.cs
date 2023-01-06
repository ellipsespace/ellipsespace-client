using MaterialDesignThemes.Wpf;
using System.Windows;

namespace EllipseSpaceClient.Pages
{
    public partial class MessageWindow : Window
    {
        public MessageWindow(PackIconKind icon, string msg)
        {
            InitializeComponent();

            this.icon.Kind = icon;
            messageTB.Text = msg;
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
