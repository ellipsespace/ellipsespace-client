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
using System.Windows.Media.Imaging;
using EllipseSpaceClient.Core.CatologueObjectRepository;
using EllipseSpaceClient.Core.Configuration;
using EllipseSpaceClient.Core.EllipseSpaceAPI;
using EllipseSpaceClient.Models.CatalogueObject;
using EllipseSpaceClient.Pages.Admin;

namespace EllipseSpaceClient.Pages
{
    public partial class MainWindow : Window
    {
        private Button CatalogueObjectButtonSample;
        private API API;

        public MainWindow()
        {
            InitializeComponent();

            var configuration = Configuration.Create();
            string jwt = configuration.JwtToken;
            Configuration.UpdateInfo(jwt);
            configuration = Configuration.Create();

            sessionNameLabel.Text = configuration.SessionName;

            API = new API(configuration.JwtToken);

            if (configuration.AccessLevel != 1)
                ItemAdminPanel.IsEnabled = false;

            CatalogueObjectButtonSample = new Button();

            var resp = API.SendRequest(API.MakeAddress("/get-all-object-catalogue"), HttpMethod.Get, true);
            CatologueObjectRepository.Repository = CatologueObject.UnmarshalArray(resp.Content.ReadAsStringAsync().Result).ToList();

            if (CatologueObjectRepository.Repository != null)
                FillObjectCatalogueList(CatologueObjectRepository.Repository.ToArray());
            else
            {
                var dr = new MessageWindow(MaterialDesignThemes.Wpf.PackIconKind.WebOff, "При попытке получения объектов возникла ошибка.").ShowDialog();
                if (dr == true || dr == false)
                    Application.Current.Shutdown();
            }
        }

        private void searchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            string entred = ((TextBox)sender).Text;

            var finded = CatologueObjectRepository.Repository.Where(obj => obj.Name.Contains(entred));
            FillObjectCatalogueList(finded.ToArray());
        }

        private void ObjectButton_Click(object sender, RoutedEventArgs e)
        {
            string name = ((Button)sender).Content.ToString();

            var obj = CatologueObjectRepository.Repository.Where(obj => obj.Name == name).ToArray()[0];

            if(obj != null)
            {
                objectNameTB.Text = obj.Name;
                objectODateTimeTB.Text = obj.OpeningDateTime;
                objectDescriptionTB.Text = obj.Description;
                var temp = Path.GetTempFileName();
                string link = obj.Photos[0];

                using (var client = new WebClient())
                {
                    if (Uri.IsWellFormedUriString(link, UriKind.Absolute))
                    {
                        client.DownloadDataAsync(new Uri(link));
                        objectPhoto.Source = new BitmapImage(new Uri(temp));
                    }
                }

                objectSCoversionPeriodTB.Text = obj.SidericConversionPeriod.ToString();
                objectOrbitalVelTB.Text = obj.BodyOrbitalVelocity.ToString();
                objectInclinationTB.Text = obj.Inclination.ToString();
                objectSatelitesTB.Text = string.Join('\n', obj.Satelites);
                objectWhoseSateliteTB.Text = obj.WhoseSatelite;
                objectEqRadiusTB.Text = obj.EquatorialRadius.ToString();
                objectPolarRadiusTB.Text = obj.PolarRadius.ToString();
                objectAvgRadiusTB.Text = obj.AverageRadius.ToString();
                objectSTB.Text = string.Format("{0:e}", obj.Square);
                objectVTB.Text = string.Format("{0:e}", obj.Volume);
                objectMTB.Text = string.Format("{0:e}", obj.Weight);
                objectPTB.Text = obj.AverageDensity.ToString();
                objectGTB.Text = obj.GravityAcceleration.ToString();
                objectV1TB.Text = obj.FirstSpaceVelocity.ToString();
                objectV2TB.Text = obj.SecondSpaceVelocity.ToString();
            }

            objectInfoBorder.Visibility = Visibility.Visible;
        }

        private void ItemAdminPanel_Selected(object sender, RoutedEventArgs e)
        {
            var adminPanelWindow = new AdminPanelWindow();
            adminPanelWindow.Owner = this;
            adminPanelWindow.Show();
        }

        private void ItemWeb_Selected(object sender, RoutedEventArgs e)
        {
            if (new MessageWindow(MaterialDesignThemes.Wpf.PackIconKind.AsteriskCircleOutline, "Нажатие на кнопку ОК приведет к открытию внешнего браузера. Вы хотите продолжить?")
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

        private void FillObjectCatalogueList(CatologueObject[] dataSourse)
        {
            catalogueObjectsList.Children.Clear();

            foreach (var obj in dataSourse)
            {
                var button = CatalogueObjectButtonSample;
                button.Content = obj.Name;
                button.Click += ObjectButton_Click;
                catalogueObjectsList.Children.Add(button);
            }
        }
    }
}
