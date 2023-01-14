using EllipseSpaceClient.Core.Configuration;
using EllipseSpaceClient.Core.Version;
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
                languagesCB.Items.Add(item);

                if (item.Content == App.Language.Name)
                    languagesCB.SelectedItem = item.Content;
            }

            var conf = new Configuration();
            foreach(var item in updateTickCB.Items)
            {
                if((item as ComboBoxItem).Content == Convert.ToString(conf.Tick))
                {
                    updateTickCB.SelectedItem = item as ComboBoxItem;
                    break;
                }
            }

            versionText.Text += $" {Version.Local()}";
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
