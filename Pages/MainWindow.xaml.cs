using EllipseSpaceClient.Core.EllipseSpaceAPI;
using EllipseSpaceClient.Models.Sessions;
using System.Net.Http;
using System.Windows;

namespace EllipseSpaceClient
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void authButton_Click(object sender, RoutedEventArgs e)
        {
            string sessionName = sessionNameTB.Text,
                sessionPassword = sessionPasswordTB.Password;

            var session = new Session(sessionName, sessionPassword);

            var jwt = new API().SendRequest(API.MakeAddress("/session/auth"), HttpMethod.Get, false, session.Marshal());

            if(jwt.Result == null)
                MessageBox.Show("Сервер отправил пустой ключ доступа. Возможно, вы ввели неправильные данные.");

            MessageBox.Show(jwt.Result);
        }

        private void regButton_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow rw = new RegistrationWindow();
            rw.Show();
        }
    }
}
