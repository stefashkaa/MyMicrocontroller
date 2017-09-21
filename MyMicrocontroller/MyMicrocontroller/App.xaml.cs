using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace MyMicrocontroller
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static List<CultureInfo> languages = new List<CultureInfo>();
        private static CultureInfo currentLanguage = null;

        public static List<CultureInfo> Languages
        {
            get
            {
                return languages;
            }
        }

        public App()
        {
            languages.Clear();
            languages.Add(new CultureInfo("en-US")); //default
            languages.Add(new CultureInfo("ru-RU"));
        }

        public static event EventHandler LanguageChanged;

        public static CultureInfo Language
        {
            get => currentLanguage ?? CultureInfo.GetCultureInfo("en-US");//default
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                if (value == System.Threading.Thread.CurrentThread.CurrentUICulture) return;

                System.Threading.Thread.CurrentThread.CurrentUICulture = value;

                var dict = new ResourceDictionary();
                switch (value.Name)
                {
                    case "ru-RU":
                        dict.Source = new Uri(String.Format("Resources/lang.{0}.xaml", value.Name), UriKind.Relative);
                        break;
                    default:
                        dict.Source = new Uri("Resources/lang.xaml", UriKind.Relative);
                        break;
                }

                var oldDict = (from d in Application.Current.Resources.MergedDictionaries
                                              where d.Source != null && d.Source.OriginalString.StartsWith("Resources/lang.")
                                              select d).First();
                if (oldDict != null)
                {
                    int ind = Application.Current.Resources.MergedDictionaries.IndexOf(oldDict);
                    Application.Current.Resources.MergedDictionaries.Remove(oldDict);
                    Application.Current.Resources.MergedDictionaries.Insert(ind, dict);
                }
                else
                {
                    Application.Current.Resources.MergedDictionaries.Add(dict);
                }
                currentLanguage = value;
                LanguageChanged(Application.Current, new EventArgs());
            }
        }
    }
}
