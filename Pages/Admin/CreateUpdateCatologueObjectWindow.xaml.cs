using EllipseSpaceClient.Core.Configuration;
using EllipseSpaceClient.Core.EllipseSpaceAPI;
using EllipseSpaceClient.Models.CatalogueObject;
using EllipseSpaceClient.Models.ServerStatus;
using System.Net.Http;
using System.Windows;

namespace EllipseSpaceClient.Pages.Admin
{
    enum EditType
    {
        Create,
        Update
    }

    public partial class CreateUpdateCatalogueObjectWindow : Window
    {
        private readonly EditType Type;

        public CreateUpdateCatalogueObjectWindow(CatologueObject? objectToUpdate = null)
        {
            InitializeComponent();

            if (objectToUpdate != null)
            {
                catalogueObjectDG.ItemsSource = new CatologueObject[1] { objectToUpdate };
                Type = EditType.Update;
            }
            else
            {
                catalogueObjectDG.ItemsSource = new CatologueObject[1] { new CatologueObject() };
                Type = EditType.Create;
            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (Type == EditType.Create)
                CreateUpdateObject("/add-object-catologue", HttpMethod.Post, "Объект успешно добавлен.");
            else
                CreateUpdateObject("/update-object-catologue", HttpMethod.Put, "Даннные об объекте обновлены.");
        }

        private void CreateUpdateObject(string relative, HttpMethod method, string message)
        {
            var resp = new API(Configuration.Create().JwtToken).SendRequest(API.MakeAddress("/add-object-catologue"), method, true,
                    ((CatologueObject)(catalogueObjectDG.Items[0])).Marshal());

            if (resp.IsSuccessStatusCode)
                new MessageWindow(MaterialDesignThemes.Wpf.PackIconKind.SuccessCircleOutline, message);
            else
                new MessageWindow(MaterialDesignThemes.Wpf.PackIconKind.EmojiSadOutline, ServerStatus.Unmarshal(resp.Content.ReadAsStringAsync().Result).Message);
        }
    }
}
