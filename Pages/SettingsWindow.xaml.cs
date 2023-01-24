using EllipseSpaceClient.Core.Configuration;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using Version = EllipseSpaceClient.Core.Version.Version;

namespace EllipseSpaceClient.Pages
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();

            languagesCB.Items.Clear();
            foreach (var lang in App.Languages)
            {
                var item = new ComboBoxItem();
                item.Tag = lang.Name;
                item.Content = App.LanguagesNames[lang.Name];
                int id =languagesCB.Items.Add(item);

                if(item.Tag.ToString() == App.Language.Name)
                    languagesCB.SelectedIndex = id;
            }

            var conf = Configuration.Create();
            foreach(ComboBoxItem item in updateTickCB.Items)
            {
                if (item.Content.ToString() == Convert.ToString(conf.Tick))
                    updateTickCB.SelectedIndex = updateTickCB.Items.IndexOf(item);
            }

            versionText.Text += $" {Version.Local()}";
        }

        private void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (new MessageWindow(MaterialDesignThemes.Wpf.PackIconKind.WarningBoxOutline, Application.Current.Resources["l_logout_message"].ToString()).ShowDialog() == true)
            {
                Configuration.Logout();
                Application.Current.Shutdown();
            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (languagesCB.SelectedItem != null && updateTickCB.SelectedItem != null)
                DialogResult = true;
            else
                new MessageWindow(MaterialDesignThemes.Wpf.PackIconKind.EmojiSadOutline, Application.Current.Resources["l_invalid_settings"].ToString()).ShowDialog();
            Close();
        }

        public Tuple<CultureInfo, int> GetPreferences()
        {
            return new Tuple<CultureInfo, int>(CultureInfo.GetCultureInfo((languagesCB.SelectedItem as ComboBoxItem).Tag.ToString()),
                 Convert.ToInt32((updateTickCB.SelectedItem as ComboBoxItem).Content.ToString()));
        }
    }
}
