using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace MyMicrocontroller
{
    /// <summary>
    /// Логика взаимодействия для Client.xaml
    /// </summary>
    public partial class Client : Window
    {
        private readonly BackgroundWorker worker;

        public Client()
        {
            InitializeComponent();
            InitializeCOMSelector();
            worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        }

        public Client(Account account) : this()
        {
            loginName.Content = account.Name;
            InitialAdminTools(account);
            Trace.Log(MessageType.Success, "Ready for work", traceView);
        }

        private void InitialAdminTools(Account account)
        {
            if (!account.IsAdmin)
            {
                adminTools.Visibility = Visibility.Hidden;
                userTools.Margin = adminTools.Margin;
            }
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            // run all background tasks here
            var number = int.Parse(e.Argument.ToString());
            Trace.Log(MessageType.Info, "Starting...", traceView);
            Thread.Sleep(4000);
            Trace.Log(MessageType.Success, "Procedure was complited", traceView);
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //update ui once worker complete his work
            progressBar.Visibility = Visibility.Hidden;
            start_btn.IsEnabled = true;
        }

        private void InitializeCOMSelector()
        {
            var ports = SerialPort.GetPortNames();
            portsSelector.Items.Clear();
            foreach (var port in ports)
            {
                portsSelector.Items.Add(port);
            }
            portsSelector.SelectedIndex = 0;
        }

        private COMPort InitializeSerial()
        {
            var name = (string)portsSelector.SelectedItem;
            var serial = new COMPort(name);
            return serial;
        }

        private void Start_btn_Click(object sender, RoutedEventArgs e)
        {
            start_btn.IsEnabled = false;
            progressBar.Visibility = Visibility.Visible;
            var number = int.Parse((new List<RadioButton>() { rb1, rb2, rb3, rb4 }).First(rb => rb.IsChecked == true).Content.ToString());
            worker.RunWorkerAsync(number);
        }

        private void portsSelector_DropDownOpened(object sender, EventArgs e)
        {
            InitializeCOMSelector();
        }

        private void logoff_btn_Click(object sender, RoutedEventArgs e)
        {
            Trace.Clear(traceView);
            new Logon().Show();
            this.Close();
        }
    }
}
