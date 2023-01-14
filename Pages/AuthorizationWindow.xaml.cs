using EllipseSpaceClient.Core.Configuration;
using EllipseSpaceClient.Core.EllipseSpaceAPI;
using EllipseSpaceClient.Models.ServerStatus;
using EllipseSpaceClient.Models.Sessions;
using EllipseSpaceClient.Pages;
using System.Net.Http;
using System.Windows;

namespace EllipseSpaceClient
{
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow()
        {
            InitializeComponent();

            var configuration = Configuration.Create();

            if (configuration.JwtToken != string.Empty && configuration.JwtToken != null)
                OpenMainWindow();

        }

        private void authButton_Click(object sender, RoutedEventArgs e)
        {
            string sessionName = sessionNameTB.Text,
                sessionPassword = sessionPasswordTB.Password;

            var session = new Session(sessionName, sessionPassword);
            var API = new API();

            var authResp = API.SendRequest(API.MakeAddress("/session/auth"), HttpMethod.Get, false, session.Marshal());
            string jwt =  authResp.Content.ReadAsStringAsync().Result.Replace("\"", "");
            API.ApiKey = jwt;

            if (!jwt.Contains(" "))
            {
                Configuration.UpdateSessionInfo(jwt);
                OpenMainWindow();
            }
            else
                new MessageWindow(MaterialDesignThemes.Wpf.PackIconKind.FaceConfusedOutline, ServerStatus.Unmarshal(authResp.Content.ReadAsStringAsync().Result).Message).ShowDialog();
        }

        private void regButton_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow rw = new RegistrationWindow();
            rw.Show();
        }

        private void OpenMainWindow()
        {
            var mWindow = new MainWindow();
            mWindow.Show();

            Close();
        }
    }
}
