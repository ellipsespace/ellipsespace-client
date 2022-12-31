using EllipseSpaceClient.Core.EllipseSpaceAPI;
using EllipseSpaceClient.Models.Sessions;
using System.Net.Http;
using System.Windows;

namespace EllipseSpaceClient
{
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private void regButton_Click(object sender, RoutedEventArgs e)
        {
            string sessionName = sessionNameTB.Text,
                sessionPassword = sessionPasswordTB.Password,
                rSP = rSessionPasswordTB.Password;

            if(rSP != sessionPassword)
            {
                MessageBox.Show("Пароли не совпадают.");
                return;
            }

            var session = new Session(sessionName, sessionPassword);

            var id = new API().SendRequest(API.MakeAddress("/session/create"), HttpMethod.Post, false, session.Marshal());

            var configuration = new Configuration.Configuration();
            configuration.Id = id.Result;
            configuration.Save();
        }
    }
}
