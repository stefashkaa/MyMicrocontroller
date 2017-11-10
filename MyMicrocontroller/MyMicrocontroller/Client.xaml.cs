using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        private readonly List<COMPort> ports;

        public Client()
        {
            InitializeComponent();
            InitializeCOMSelector();
            worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            ports = new List<COMPort>();
        }

        public Client(Account account) : this()
        {
            loginName.Content = account.Name;
            InitialAdminTools(account);
            Trace.Log("ready", traceView);
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
            var number = int.Parse(e.Argument.ToString());
            var name = "";
            var port = ports.FirstOrDefault(p => p.GetName().Equals(name));

            if (port == null)
            {
                Trace.Log(EventMessage.PortNotOpened.ToString(), traceView, MessageType.Error, name);
                return;
            }

            Trace.Log("startMainProc", traceView, MessageType.Info, name);
            var condition = port.Run();
            Thread.Sleep(4000);

            switch (condition)
            {
                case EventMessage.PortNotOpened:
                    Trace.Log(condition.ToString(), traceView, MessageType.Error, name);
                    break;
                case EventMessage.MainOperationNotComplited:
                    Trace.Log(condition.ToString(), traceView, MessageType.Error, name);
                    break;
                case EventMessage.MainOperationComplited:
                    Trace.Log(condition.ToString(), traceView, MessageType.Success, name);
                    break;
                default:
                    break;
            }
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
            //worker.RunWorkerAsync(number);
            AcyncRunProcedure(number).ContinueWith(new Action<Task>(delegate(Task t) 
            {
                //changeIndicator(true);
                //start_btn.IsEnabled = true;
                //progressBar.Visibility = Visibility.Hidden;
                Invoker(() => progressBar.Visibility = Visibility.Hidden);
                Invoker(() => changeIndicator(true));
                Invoker(() => start_btn.IsEnabled = true);

            }));
        }

        private void portsSelector_DropDownOpened(object sender, EventArgs e)
        {
            InitializeCOMSelector();
        }

        private void logoff_btn_Click(object sender, RoutedEventArgs e)
        {
            ports.Clear();
            Trace.Clear(traceView);
            new Logon().Show();
            this.Close();
        }

        private void openPort_btn_Click(object sender, RoutedEventArgs e)
        {
            var name = portsSelector.SelectedItem.ToString();
            var port = ports.FirstOrDefault(p => p.GetName().Equals(name));
            if (port == null)
            {
                ports.Add(new COMPort());
                port = ports.Last();
            }
            var condition = port.Open(name);
            switch (condition)
            {
                case EventMessage.PortOpened:
                    Trace.Log(condition.ToString(), traceView, MessageType.Success, name);
                    break;
                case EventMessage.PortOpeningError:
                    Trace.Log(condition.ToString(), traceView, MessageType.Error, name);
                    break;
                case EventMessage.PortAlreadyOpen:
                    Trace.Log(condition.ToString(), traceView, MessageType.Warning, name);
                    break;
                default:
                    break;
            }
        }

        private void closePort_btn_Click(object sender, RoutedEventArgs e)
        {
            var name = portsSelector.SelectedItem.ToString();
            var port = ports.FirstOrDefault(p => p.GetName().Equals(name));
            if (port == null)
            {
                Trace.Log(EventMessage.PortClosingError.ToString(), traceView, MessageType.Error, name);
                return;
            }
            var condition = port.Close();
            switch (condition)
            {
                case EventMessage.PortClosed:
                    Trace.Log(condition.ToString(), traceView, MessageType.Success, name);
                    break;
                case EventMessage.PortClosingError:
                    Trace.Log(condition.ToString(), traceView, MessageType.Error, name);
                    break;
                case EventMessage.PortAlreadyClose:
                    Trace.Log(condition.ToString(), traceView, MessageType.Warning, name);
                    break;
                default:
                    break;
            }
        }

        private void RunProcedure(int count)
        {
            var name = string.Empty;
            Invoker(() => name = portsSelector.SelectedItem.ToString());
            var port = ports.FirstOrDefault(p => p.GetName().Equals(name));
            if (port == null)
            {
                Trace.Log(EventMessage.PortNotOpened.ToString(), traceView, MessageType.Error, name);
                return;
            }

            Trace.Log("startMainProc", traceView, MessageType.Info, name);
            var condition = port.Run();
            Thread.Sleep(4000);

            switch (condition)
            {
                case EventMessage.PortNotOpened:
                    Trace.Log(condition.ToString(), traceView, MessageType.Error, name);
                    break;
                case EventMessage.MainOperationNotComplited:
                    Trace.Log(condition.ToString(), traceView, MessageType.Error, name);
                    break;
                case EventMessage.MainOperationComplited:
                    Trace.Log(condition.ToString(), traceView, MessageType.Success, name);
                    break;
                default:
                    break;
            }

            //Invoker(() => progressBar.Visibility = Visibility.Hidden);
            //Invoker(() => changeIndicator(true));
            //Invoker(() => start_btn.IsEnabled = true);
        }

        private async Task AcyncRunProcedure(int count)
        {
            await Task.Factory.StartNew(() => RunProcedure(count), TaskCreationOptions.LongRunning);
        }

        private void Invoker(Action callback)
        {
            Dispatcher.Invoke(callback);
        }
    }
}
