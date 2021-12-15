using SCARA_UI.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using SCARA_UI.MVVM.View;

namespace SCARA_UI.MVVM.ViewModel
{
    class HomeViewModel : ObservableObject
    {
        private ICommand m_ButtonCommand;
        public ICommand ButtonCommand
        {
            get
            {
                return m_ButtonCommand;
            }
            set
            {
                m_ButtonCommand = value;
            }
        }

        public HomeViewModel()
        {
            ButtonCommand = new RelayCommand(new Action<object>(choose));
            Connection = "Disconnected";
        }

        /*
        public object _currentView;

        public object CurentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        */

       

        MainWindow main = (MainWindow)System.Windows.Application.Current.MainWindow;


        public void choose(object b)
        {
            if (b.ToString() != "decon")
            {
                //main.connect(b.ToString());
                
                try
                {
                        bool isCorrect = Int32.TryParse(_BandRate ,out int j);
                        if (isCorrect) {
                            main.connect(_comm, bR);
                            Connection = "Connected";
                        }
                }
                catch(FormatException)
                {
                    MessageBox.Show("Format incorrect de Bande Rate!");
                }
                
            }
            else{
                main.disconnect();
                Connection = "Disconnected";
            }
        }
        public string conStat;
        public string Connection {
            get { 
                return conStat; 
            }
            set { 
                conStat = value;
                OnPropertyChanged("Connection");
            }
        }

        public string _comm;

        public string comm
        {
            get {
                return _comm;
            }

            set
            {
                _comm = value;
                OnPropertyChanged("comm");
            }
            
        }


        private int bR = 9600;

        public string _BandRate;

        public string BandRate
        {
            get
            {
                return _BandRate;
            }

            set
            {
                _BandRate = value;
                OnPropertyChanged("BandRate");
            }

        }
    }
}
