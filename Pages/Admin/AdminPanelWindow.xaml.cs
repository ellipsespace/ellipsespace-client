using EllipseSpaceClient.Core.CatologueObjectRepository;
using EllipseSpaceClient.Core.Configuration;
using EllipseSpaceClient.Core.EllipseSpaceAPI;
using EllipseSpaceClient.Models.CatalogueObject;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;

namespace EllipseSpaceClient.Pages.Admin
{
    public partial class AdminPanelWindow : Window
    {
        public AdminPanelWindow()
        {
            InitializeComponent();

            objectsLV.ItemsSource = CatalogueObjectRepository.Repository;
        }

        private void objectsLV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (objectsLV.SelectedIndex != -1) ChangeButtonsEnable(true);
            else ChangeButtonsEnable(false);
        }

        private void createObjectButton_Click(object sender, RoutedEventArgs e)
        {
            new CreateUpdateCatalogueObjectWindow().ShowDialog();
        }

        private void updateObjectButton_Click(object sender, RoutedEventArgs e)
        {
            new CreateUpdateCatalogueObjectWindow(GetByName()).ShowDialog();
        }

        private void deleteObjectButton_Click(object sender, RoutedEventArgs e)
        {
            if(new MessageWindow(MaterialDesignThemes.Wpf.PackIconKind.AsteriskCircleOutline, "Это действие будет невозможно отменить. Нажмите ОК, чтобы продолжить.")
                .ShowDialog() == true)
            {
                var obj = GetByName();
                var resp = new API(Configuration.Create().JwtToken).SendRequest(API.MakeAddress("/catalogue/delete"), HttpMethod.Delete, true, obj.Marshal());
            }

        }

        private CatalogueObject? GetByName()
        {
            return CatalogueObjectRepository.Repository.Find(obj => obj.Name == ((CatalogueObject)objectsLV.SelectedItem).Name);
        }

        private void ChangeButtonsEnable(bool value)
        {
            updateObjectButton.IsEnabled = value;
            deleteObjectButton.IsEnabled = value;
        }
    }
}
