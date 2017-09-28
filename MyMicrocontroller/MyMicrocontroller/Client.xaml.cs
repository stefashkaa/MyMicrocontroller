using System;
using System.Collections.Generic;
using System.IO.Ports;
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
using System.Windows.Shapes;

namespace MyMicrocontroller
{
    /// <summary>
    /// Логика взаимодействия для Client.xaml
    /// </summary>
    public partial class Client : Window
    {
        private string userName;
        public Client()
        {
            InitializeComponent();
            InitializeCOMSelector();
        }

        public Client(string name) : this()
        {
            this.userName = name;
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
    }
}
