using EllipseSpaceClient.Core.Configuration;
using EllipseSpaceClient.Core.EllipseSpaceAPI;
using EllipseSpaceClient.Models.ServerStatus;
using EllipseSpaceClient.Models.Sessions;
using EllipseSpaceClient.Pages;
using System;
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
                new MessageWindow(MaterialDesignThemes.Wpf.PackIconKind.FaceConfusedOutline, Application.Current.Resources["l_passwords_not_match"].ToString()).ShowDialog();
                return;
            }

            var session = new Session(sessionName, sessionPassword);

            var idResp = new API().SendRequest(API.MakeAddress("/session/create"), HttpMethod.Post, false, session.Marshal());
            string id = idResp.Content.ReadAsStringAsync().Result.Replace("\"", "");

            if(idResp.IsSuccessStatusCode) 
            {
                var configuration = Configuration.Create();
                configuration.Id = Convert.ToInt32(id);
                configuration.Save();

                new MessageWindow(MaterialDesignThemes.Wpf.PackIconKind.FaceExcitedOutline, Application.Current.Resources["l_account_registred"].ToString()).ShowDialog();
            }
            else
                new MessageWindow(MaterialDesignThemes.Wpf.PackIconKind.EmojiSadOutline, ServerStatus.Unmarshal(idResp.Content.ReadAsStringAsync().Result).Message);
        }
    }
}
