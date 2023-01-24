using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using EllipseSpaceClient.Core.CatologueObjectRepository;
using EllipseSpaceClient.Core.Configuration;
using EllipseSpaceClient.Core.EllipseSpaceAPI;
using EllipseSpaceClient.Core.Version;
using EllipseSpaceClient.Models.CatalogueObject;
using EllipseSpaceClient.Pages.Admin;
using Version = EllipseSpaceClient.Core.Version.Version;

namespace EllipseSpaceClient.Pages
{
    public partial class MainWindow : Window
    {
        private Button CatalogueObjectButtonSample;
        private API API;
        private DispatcherTimer Timer;

        public MainWindow()
        {
            InitializeComponent();

            var tag = GitAPI.GetLatestTag();
            var serverVersion = new Version(tag);
            var localVersion = Version.Local();

            if (serverVersion > localVersion && new MessageWindow(MaterialDesignThemes.Wpf.PackIconKind.Update,
                $"{Application.Current.Resources["l_new_version_1"]} {serverVersion} {Application.Current.Resources["l_new_version_2"]}").ShowDialog() == true)
            {
                if (File.Exists(App.UPDATE_FILE_NAME))
                    File.Delete(App.UPDATE_FILE_NAME);

                new WebClient().DownloadFileAsync(new Uri(GitAPI.MakeAddress("ellipsespace/ellipsespace-client/releases/latest/download/upd.zip")), App.UPDATE_FILE_NAME);

                new MessageWindow(MaterialDesignThemes.Wpf.PackIconKind.Update, Application.Current.Resources["l_update_downloaded"].ToString()).ShowDialog();

                if (File.Exists(App.UPDATE_MODULE_NAME))
                    Process.Start(App.UPDATE_MODULE_NAME, App.UPDATE_FILE_NAME.Split(' '));

                Application.Current.Shutdown();
            }

            var configuration = Configuration.Create();
            string jwt = configuration.JwtToken;
            Configuration.UpdateSessionInfo(jwt);
            configuration = Configuration.Create();

            sessionNameLabel.Text = configuration.SessionName;

            API = new API(configuration.JwtToken);

            if (configuration.AccessLevel != 1)
                ItemAdminPanel.IsEnabled = false;

            CatalogueObjectButtonSample = new Button();

            UpdateLocalRepository();

            if(CatalogueObjectRepository.Repository != null)
                FillObjectCatalogueList(CatalogueObjectRepository.Repository.ToArray());
            else
            {
                new MessageWindow(MaterialDesignThemes.Wpf.PackIconKind.WebOff, Application.Current.Resources["l_api_failed"].ToString()).ShowDialog();
                Configuration.Logout();
                Application.Current.Shutdown();
            }

            Timer = new DispatcherTimer();
            Timer.Tick += Timer_Tick;
            SetTimer(new TimeSpan(0, configuration.Tick, 0));
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if(Timer.Interval != TimeSpan.Zero)
            UpdateLocalRepository();
        }

        private void searchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            string entred = ((TextBox)sender).Text;

            var finded = CatalogueObjectRepository.Repository.Where(obj => obj.Name.ToLower().Contains(entred.ToLower()));
            FillObjectCatalogueList(finded.ToArray());
        }

        private void ObjectButton_Click(object sender, RoutedEventArgs e)
        {
            string name = ((Button)sender).Content.ToString();

            var obj = CatalogueObjectRepository.Repository.Where(obj => obj.Name == name).ToArray()[0];

            if(obj != null)
            {
                objectNameTB.Text = obj.Name;
                objectODateTimeTB.Text = obj.OpeningDateTime;
                objectDescriptionTB.Text = obj.Description;
                string link = obj.Photos[0];

                if (Uri.IsWellFormedUriString(link, UriKind.Absolute))
                {
                    objectPhoto.Source = (ImageSource)new ImageSourceConverter().ConvertFromString(link);
                }

                objectSCoversionPeriodTB.Text =  $"{obj.SidericConversionPeriod} {Application.Current.Resources["l_day"]}";
                objectOrbitalVelTB.Text = $"{obj.BodyOrbitalVelocity} {Application.Current.Resources["l_kilometers_per_second"]}";
                objectInclinationTB.Text = $"{obj.Inclination}{Application.Current.Resources["l_angle"]}"; ;
                objectSatelitesTB.Text = string.Join('\n', obj.Satelites);
                objectWhoseSateliteTB.Text = obj.WhoseSatelite;
                objectEqRadiusTB.Text = $"{obj.EquatorialRadius} {Application.Current.Resources["l_kilometer"]}";
                objectPolarRadiusTB.Text = $"{obj.PolarRadius} {Application.Current.Resources["l_kilometer"]}";
                objectAvgRadiusTB.Text = $"{obj.AverageRadius} {Application.Current.Resources["l_kilometer"]}";
                objectSTB.Text = $"{string.Format("{0:e}", obj.Square)} {Application.Current.Resources["l_kilometer_squared"]}";
                objectVTB.Text = $"{string.Format("{0:e}", obj.Volume)} {Application.Current.Resources["l_kilometer_per_qube"]}";
                objectMTB.Text = $"{string.Format("{0:e}", obj.Volume)} {Application.Current.Resources["l_kilo"]}";
                objectPTB.Text = $"{obj.AverageDensity} {Application.Current.Resources["l_gramms_per_centimetr_per_qube"]}";
                objectGTB.Text = $"{obj.GravityAcceleration} {Application.Current.Resources["l_meter_per_second_squared"]}";
                objectV1TB.Text = $"{obj.FirstSpaceVelocity} {Application.Current.Resources["l_kilometer_per_second"]}";
                objectV2TB.Text = $"{obj.SecondSpaceVelocity} {Application.Current.Resources["l_kilometer_per_second"]}";
            }

            objectInfoBorder.Visibility = Visibility.Visible;
        }

        private void ItemSettings_Selected(object sender, RoutedEventArgs e)
        {
            var window = new SettingsWindow();
            bool success = (bool)window.ShowDialog();

            if(success)
            {
                var pref = window.GetPreferences();
                var conf = Configuration.Create();

                App.Language = pref.Item1;
                SetTimer(new TimeSpan(0, pref.Item2, 0));

                conf.DefaultLanguage = pref.Item1.Name;
                conf.Tick = pref.Item2;
                conf.Save();
            }
        }

        private void ItemAdminPanel_Selected(object sender, RoutedEventArgs e)
        {
            var adminPanelWindow = new AdminPanelWindow();
            adminPanelWindow.Owner = this;
            adminPanelWindow.Show();
        }

        private void ItemSupportProject_Selected(object sender, RoutedEventArgs e)
        {
            if (new MessageWindow(MaterialDesignThemes.Wpf.PackIconKind.AsteriskCircleOutline, Application.Current.Resources["l_open_web"].ToString())
                .ShowDialog() == true)
            OpenUrl("https://boosty.to/ellipsespace");
        }

        private void ItemWeb_Selected(object sender, RoutedEventArgs e)
        {
            if (new MessageWindow(MaterialDesignThemes.Wpf.PackIconKind.AsteriskCircleOutline, Application.Current.Resources["l_open_web"].ToString())
                .ShowDialog() == true)
            OpenUrl("https://ellipsespace.onrender.com/");
        }

        private void OpenUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }

        private void FillObjectCatalogueList(CatalogueObject[] dataSourse)
        {
            catalogueObjectsList.Children.Clear();

            foreach (var obj in dataSourse)
            {
                var button = CatalogueObjectButtonSample;
                button.Name = $"{obj.Name}Button";
                button.Content = obj.Name;
                button.Click += ObjectButton_Click;
                catalogueObjectsList.Children.Add(button);
            }
        }

        private void SetTimer(TimeSpan tick)
        {
            Timer.Stop();
            Timer.Interval = tick;
            Timer.Start();
        }

        private void UpdateLocalRepository()
        {
            var resp = API.SendRequest(API.MakeAddress("/catalogue/all"), HttpMethod.Get, true);
            CatalogueObjectRepository.Repository = CatalogueObject.UnmarshalArray(resp.Content.ReadAsStringAsync().Result).ToList();
        }
    }
}
