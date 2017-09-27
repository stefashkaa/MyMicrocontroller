using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyMicrocontroller
{
    /// <summary>
    /// Логика взаимодействия для Logon.xaml
    /// </summary>
    public partial class Logon : Window
    {
        private List<Account> trustedAccounts;
        public Logon()
        {
            InitializeComponent();
            InitializeLanguageMenu();
            InitializeTrustedAccounts();
        }

        private void InitializeLanguageMenu()
        {
            App.LanguageChanged += LanguageChanged;

            var currLang = App.Language;

            menuLanguage.Items.Clear();
            foreach (var lang in App.Languages)
            {
                var menuLang = new MenuItem();
                menuLang.Header = lang.DisplayName;
                menuLang.Tag = lang;
                menuLang.IsChecked = lang.Equals(currLang);
                menuLang.Click += ChangeLanguageClick;
                menuLanguage.Items.Add(menuLang);
            }
        }

        private void InitializeTrustedAccounts()
        {
            trustedAccounts = new List<Account>()
            {
                new Account("stefan", "pass"),
                new Account("tanya", "pass"),
                new Account("yaroslav", "pass")
            };
        }

        private void LanguageChanged(object sender, EventArgs e)
        {
            var currLang = App.Language;

            foreach (MenuItem i in menuLanguage.Items)
            {
                var ci = i.Tag as CultureInfo;
                i.IsChecked = ci != null && ci.Equals(currLang);
            }
        }

        private void ChangeLanguageClick(object sender, EventArgs e)
        {
            var mi = sender as MenuItem;
            if (mi != null)
            {
                var lang = mi.Tag as CultureInfo;
                if (lang != null)
                {
                    App.Language = lang;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var converter = new TypeConverter();
            var errorLogonTitle = converter.ConvertToString(Application.Current.Resources["errorLogonTitle"]);
            var errorLogonText = converter.ConvertToString(Application.Current.Resources["errorLogonText"]);

            var currentAccount = new Account(userName_txt.Text, password_txt.Text);
            if (!trustedAccounts.Any(a => a.Name.Equals(currentAccount.Name) && a.Password.Equals(currentAccount.Password)))
            {
                MessageBox.Show(errorLogonText, errorLogonTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                var client = new Client(currentAccount.Name);
                client.Show();
                this.Close();
            }
        }
    }

    public class Account
    {
        private string name;
        private string password;

        public string Name { get => name; }
        public string Password { get => password; }

        public Account(string name, string password)
        {
            this.name = name;
            this.password = password;
        }
    }
}
