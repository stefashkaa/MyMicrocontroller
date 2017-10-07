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
        private readonly COMPort port;

        public Client()
        {
            InitializeComponent();
            InitializeCOMSelector();
            worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            port = new COMPort();
        }

        public Client(Account account) : this()
        {
            loginName.Content = account.Name;
            InitialAdminTools(account);
            Trace.Log("Ready for work", traceView);
        }

        private void changeIndicator(bool condition)
        {
            if (condition)
            {
                indicatorOFF.Visibility = Visibility.Hidden;
                indicatorON.Visibility = Visibility.Visible;
            }
            else
            {
                indicatorON.Visibility = Visibility.Hidden;
                indicatorOFF.Visibility = Visibility.Visible;
            }
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
            Trace.Log("Starting...", traceView, MessageType.Info);
            Thread.Sleep(4000);
            Trace.Log("Procedure was complited", traceView);
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //update ui once worker complete his work
            progressBar.Visibility = Visibility.Hidden;
            changeIndicator(true);
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

        private void start_btn_Click(object sender, RoutedEventArgs e)
        {
            changeIndicator(false);
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

        private void openPort_btn_Click(object sender, RoutedEventArgs e)
        {
            var condition = port.Open(portsSelector.SelectedItem.ToString());
            switch (condition)
            {
                case EventMessage.PortOpened:
                    Trace.Log(condition.ToString(), traceView);
                    break;
                case EventMessage.PortOpeningError:
                    Trace.Log(condition.ToString(), traceView, MessageType.Error);
                    break;
                case EventMessage.PortAlreadyOpen:
                    Trace.Log(condition.ToString(), traceView, MessageType.Warning);
                    break;
                default:
                    break;
            }
        }

        private void closePort_btn_Click(object sender, RoutedEventArgs e)
        {
            var condition = port.Close();
            switch (condition)
            {
                case EventMessage.PortClosed:
                    Trace.Log(condition.ToString(), traceView);
                    break;
                case EventMessage.PortClosingError:
                    Trace.Log(condition.ToString(), traceView, MessageType.Error);
                    break;
                case EventMessage.PortAlreadyClose:
                    Trace.Log(condition.ToString(), traceView, MessageType.Warning);
                    break;
                default:
                    break;
            }
        }
    }
}
