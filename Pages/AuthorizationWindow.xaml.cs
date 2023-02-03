using EllipseSpaceClient.Core.Configuration;
using EllipseSpaceClient.Pages;
using EllipseSpaceClient.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace EllipseSpaceClient
{
    public partial class AuthorizationWindow : Window
    {
        private AuthorizationViewModel authVM;

        public AuthorizationWindow()
        {
            InitializeComponent();

            authVM = new AuthorizationViewModel();
            DataContext = authVM;

            var configuration = Configuration.Create();

            if (configuration.JwtToken != string.Empty && configuration.JwtToken != null)
                WindowManager.OpenMainWindow();

        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ((PasswordBox)sender).Tag = ((PasswordBox)sender).Password;
        }
    }
}
