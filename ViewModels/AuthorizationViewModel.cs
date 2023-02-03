using EllipseSpaceClient.Core;
using EllipseSpaceClient.Models.Sessions;
using EllipseSpaceClient.Pages;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using EllipseSpaceClient.Core.Configuration;
using MaterialDesignThemes.Wpf;

namespace EllipseSpaceClient.ViewModels
{
    public class AuthorizationViewModel : INotifyPropertyChanged
    {
        private Session entredSession;
        private string sessionName;
        private string sessionPassword;
        private Command authCommand;
        private Command openRegistrationWindowCommand;
        public event PropertyChangedEventHandler PropertyChanged;

        public AuthorizationViewModel()
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

        public Command AuthCommand
        {
            get
            {
                return authCommand ??
                  (authCommand = new Command(obj =>
                  {
                      var resp = EntredSession.Authorize();
                      if (resp.Item2 == true)
                      {
                          Configuration.UpdateSessionInfo(resp.Item1);
                          WindowManager.OpenMainWindow();
                      }
                      else
                          WindowManager.ShowDialog(PackIconKind.FaceConfusedOutline, resp.Item1);
                  }));
            }
        }

        public Command OpenRegistrationWindowCommand
        {
            get
            {
                return openRegistrationWindowCommand ??
                    (openRegistrationWindowCommand = new Command(obj =>
                    {
                        WindowManager.OpenRegistrationWindow();
                    }));
            }
        }
    }
}
