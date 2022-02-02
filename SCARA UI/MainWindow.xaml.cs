using System;
using System.Collections.Generic;
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
using System.IO.Ports;
using SCARA_UI.MVVM.ViewModel;

namespace SCARA_UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    
    public partial class MainWindow : Window
    {

        // Globale variables
        public double L1 = 175, L2 = 175;


        public SerialPort sp = new SerialPort();
        public string pn = "COM5";
        public int RateBand = 9600;

        private void GridOfWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var move = sender as System.Windows.Controls.Grid;
            var win = Window.GetWindow(move);
            win.DragMove();
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        public void connect( string com, int bR)
        {
            // MessageBox.Show(com + " " +bR.ToString());

            try
            {
                RateBand = bR;
                pn = com;
                String portName = pn;
                sp.PortName = portName;
                sp.ReadTimeout = 1000;
                sp.WriteTimeout = 1000;
                sp.BaudRate = RateBand;
                sp.Parity = Parity.None;
                sp.Open();
                //sp.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
                buffer = string.Empty;
                MessageBox.Show("Connected");
                sp.WriteLine("I 1 1 1");
                
            }
            catch (Exception)
            {
                MessageBox.Show("Please give a valid port number or check your connection");
            }
        }


        public void disconnect()
        {
            try
            {
                sp.Close();
                MessageBox.Show("Disconnected");
            }
            catch (Exception)
            {
                MessageBox.Show("First Connect and then disconnect");
            }
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        public void writeIn(string cmd)
        {
            sp.WriteLine(cmd);
        }

        public string buffer { get; set; }
        public List<string> logs = new List<string>();

        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            buffer += sp.ReadExisting();
            MessageBox.Show(buffer);
            if (buffer.Contains("\r\n"))
            {
                //run code on data received from serial port
                logs.Add(buffer);
                MessageBox.Show(buffer);
                buffer = string.Empty;
            }
        }

    }
}
