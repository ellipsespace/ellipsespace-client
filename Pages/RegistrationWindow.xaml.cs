using EllipseSpaceClient.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace EllipseSpaceClient
{
    public partial class RegistrationWindow : Window
    {
        private RegistrationViewModel registrationVM;

        public RegistrationWindow()
        {
            InitializeComponent();

            registrationVM = new RegistrationViewModel();
            DataContext = registrationVM;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ((PasswordBox)sender).Tag = ((PasswordBox)sender).Password;
        }
    }
}
