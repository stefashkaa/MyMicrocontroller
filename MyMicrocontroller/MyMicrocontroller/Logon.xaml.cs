using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyMicrocontroller
{
    /// <summary>
    /// Логика взаимодействия для Logon.xaml
    /// </summary>
    public partial class Logon : Window
    {
        public Logon()
        {
            InitializeComponent();
            InitializeLanguageMenu();
        }

        #region LanguageEvents

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

        #endregion

        private List<Account> InitializeTrustedAccounts()
        {
            return new List<Account>()
            {
                new Account("stefan", "pass"),
                new Account("tanya", "pass"),
                new Account("yaroslav", "pass")
            };
        }

        private void Logon_Click(object sender, RoutedEventArgs e)
        {
            var converter = new TypeConverter();
            var errorLogonTitle = converter.ConvertToString(Application.Current.Resources["errorLogonTitle"]);
            var errorLogonText = converter.ConvertToString(Application.Current.Resources["errorLogonText"]);

            var trustedAccounts = InitializeTrustedAccounts();
            var currentAccount = new Account(userName_txt.Text, password_pass.Password);

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

        private void UserName_txt_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(userName_txt.Text))
            {
                userName_txt.Text = (string)Application.Current.Resources["userName"];
            }
        }

        private void UserName_txt_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (userName_txt.Text.Equals((string)Application.Current.Resources["userName"]))
            {
                userName_txt.Text = "";
            }
        }

        private void Password_txt_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            password_txt.Visibility = Visibility.Hidden;
            password_pass.Visibility = Visibility.Visible;
            password_pass.Focus();
        }

        private void Password_pass_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(password_pass.Password))
            {
                password_pass.Visibility = Visibility.Hidden;
                password_pass.Password = "";
                password_txt.Visibility = Visibility.Visible;
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
