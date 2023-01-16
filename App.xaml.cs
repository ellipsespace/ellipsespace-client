using EllipseSpaceClient.Pages;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Threading;
using EllipseSpaceClient.Core.Configuration;
using System.Diagnostics.Contracts;

namespace EllipseSpaceClient
{
    public partial class App : Application
    {
        private static List<CultureInfo> languages = new List<CultureInfo>();
        private static Dictionary<string, string> languagesNames = new Dictionary<string, string> {
            { "en-US", "English (American)" },
            { "ru-RU", "Русский" },
            { "el", "Ελληνικά" },
            { "pl-PL", "Polska" },
            { "bg-BG", "Български" }
        };
        public static event EventHandler LanguageChanged;

        public static CultureInfo Language
        {
            get
            {
                return Thread.CurrentThread.CurrentUICulture;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("No value");
                if (value == Thread.CurrentThread.CurrentUICulture) return;

                Thread.CurrentThread.CurrentUICulture = value;

                ResourceDictionary dict = new ResourceDictionary();
                if (value.Name != "en-US")
                    dict.Source = new Uri($"Resources/lang.{value.Name}.xaml", UriKind.Relative);
                else
                    dict.Source = new Uri($"Resources/lang.xaml", UriKind.Relative);

                ResourceDictionary oldDict = (from d in Current.Resources.MergedDictionaries
                                              where d.Source != null && d.Source.OriginalString.StartsWith("Resources/lang.")
                                              select d).First();
                if (oldDict != null)
                {
                    int ind = Current.Resources.MergedDictionaries.IndexOf(oldDict);
                    Current.Resources.MergedDictionaries.Remove(oldDict);
                    Current.Resources.MergedDictionaries.Insert(ind, dict);
                }
                else
                    Current.Resources.MergedDictionaries.Add(dict);

                LanguageChanged(Current, new EventArgs());
            }
        }

        public static List<CultureInfo> Languages
        {
            get
            {
                return languages;
            }
        }

        public static Dictionary<string, string> LanguagesNames
        {
            get
            {
                return languagesNames;
            }
            set
            {
                return;
            }
        }

        public const string UPDATE_FILE_NAME = "upd.zip";
        public const string UPDATE_MODULE_NAME = "updmodule.exe";

        public App()
        {
            InitializeComponent();
            DispatcherUnhandledException += App_DispatcherUnhandledException;

            languages.Clear();
            languages.Add(new CultureInfo("en-US"));
            languages.Add(new CultureInfo("ru-RU"));
            languages.Add(new CultureInfo("el"));
            languages.Add(new CultureInfo("pl-PL"));
            languages.Add(new CultureInfo("bg-BG"));

            LanguageChanged += App_LanguageChanged;
            Language = CultureInfo.GetCultureInfo(Configuration.Create().DefaultLanguage);
        }

        private void App_LanguageChanged(object? sender, EventArgs e)
        {
            var conf = Configuration.Create();
            conf.DefaultLanguage = Language.Name;
            conf.Save();
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            new MessageWindow(MaterialDesignThemes.Wpf.PackIconKind.ErrorOutline, $"{Current.Resources["l_exception_1"]}\n{e.Exception.Message}." +
                $"\n{Current.Resources["l_exception_2"]}").ShowDialog();
            e.Handled = true;
            Current.Shutdown();
        }
    }
}
