using EllipseSpaceClient.Core.CatologueObjectRepository;
using System.Windows;

namespace EllipseSpaceClient.Pages.Admin
{
    public partial class AdminPanelWindow : Window
    {
        public AdminPanelWindow()
        {
            InitializeComponent();

            objectsLV.ItemsSource = CatologueObjectRepository.Repository;
        }
    }
}
