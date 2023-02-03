namespace EllipseSpaceClient.Pages
{
    public static class WindowManager
    {
        public static void OpenMainWindow()
        {
            var mWindow = new MainWindow();
            mWindow.Show();

            App.Current.MainWindow.Close();
        }

        public static void OpenRegistrationWindow()
        {
            RegistrationWindow rw = new RegistrationWindow();
            rw.Show();
        }

        public static bool ShowDialog(MaterialDesignThemes.Wpf.PackIconKind icon, string text)
        {
            return (bool)new MessageWindow(icon, text).ShowDialog();
        }
    }
}
