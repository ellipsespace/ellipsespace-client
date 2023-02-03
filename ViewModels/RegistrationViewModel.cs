using EllipseSpaceClient.Core;
using EllipseSpaceClient.Core.Configuration;
using EllipseSpaceClient.Models.Sessions;
using EllipseSpaceClient.Pages;
using MaterialDesignThemes.Wpf;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace EllipseSpaceClient.ViewModels
{
    public class RegistrationViewModel : INotifyPropertyChanged
    {
        private Session entredSession;
        private string sessionName;
        private string sessionPassword;
        private string sessionPasswordRepeat;

        private Command registrationCommand;

        public event PropertyChangedEventHandler PropertyChanged;

        public RegistrationViewModel()
        {
            entredSession = new Session();
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public Session EntredSession
        {
            get => entredSession;
            set
            {
                entredSession = value;
                OnPropertyChanged(nameof(entredSession));
            }
        }

        public string SessionName
        {
            get => sessionName;
            set
            {
                sessionName = value;
                EntredSession.Name = sessionName;
                OnPropertyChanged(nameof(sessionName));
            }
        }

        public string SessionPassword
        {
            get => sessionPassword;
            set
            {
                sessionPassword = value;
                EntredSession.Password = sessionPassword;
                OnPropertyChanged(nameof(sessionPassword));
            }
        }

        public string SessionPasswordRepeat
        {
            get => sessionPasswordRepeat;
            set
            {
                sessionPasswordRepeat = value;
                OnPropertyChanged(nameof(sessionPasswordRepeat));
            }
        }

        public Command RegistrationCommand
        {
            get
            {
                return registrationCommand ??
                  (registrationCommand = new Command(obj =>
                  {
                      if (sessionPassword != sessionPasswordRepeat)
                      {
                          WindowManager.ShowDialog(MaterialDesignThemes.Wpf.PackIconKind.FaceConfusedOutline, Application.Current.Resources["l_passwords_not_match"].ToString());
                          return;
                      }

                      var session = new Session(sessionName, sessionPassword);

                      var resp = session.Register();

                      if (resp.Item2 == true)
                      {
                          var configuration = Configuration.Create();
                          configuration.Id = Convert.ToInt32(resp.Item1);
                          configuration.Save();

                          WindowManager.ShowDialog(PackIconKind.FaceExcitedOutline, Application.Current.Resources["l_account_registred"].ToString());
                      }
                      else
                          WindowManager.ShowDialog(PackIconKind.EmojiSadOutline, resp.Item1);
                  }));
            }
        }
    }
}
