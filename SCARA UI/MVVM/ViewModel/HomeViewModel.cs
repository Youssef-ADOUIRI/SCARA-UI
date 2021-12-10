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

        public string conStat = "Disconnected";

        MainWindow main = (MainWindow)System.Windows.Application.Current.MainWindow;


        public void choose(object b)
        {
            if (b.ToString() != "decon")
            {
                //main.connect(b.ToString());
                conStat = "Connected";
                main.connect(_comm);
                MessageBox.Show(_comm);
            }
            else{
                main.disconnect();
                conStat = "Disconnected";
            }
        }

        public string Connection {
            get { return conStat; }
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
    }
}
