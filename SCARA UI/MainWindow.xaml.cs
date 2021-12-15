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
        public SerialPort sp = new SerialPort();
        public string pn = "COM4";
        public bool isConnected;
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

        public void connect( string com  , int bR)
        {
            // MessageBox.Show(com + " " +bR.ToString());

            try
            {
                RateBand = bR;
                pn = com;
                String portName = pn;
                sp.PortName = portName;
                sp.BaudRate = RateBand;
                sp.Open();
                //MessageBox.Show("Connected");
                isConnected = true;
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
                isConnected = false;
            }
            catch (Exception)
            {
                MessageBox.Show("First Connect and then disconnect");
            }
        }


        public void writeIn(string cmd)
        {
            sp.Write(cmd);
        }
        

    }
}
