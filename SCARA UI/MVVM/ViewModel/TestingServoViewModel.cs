using SCARA_UI.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SCARA_UI.MVVM.ViewModel
{
    class TestingServoViewModel
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

        public TestingServoViewModel()
        {
            ButtonCommand = new RelayCommand(new Action<object>(test));
        }

        public void test(object b)
        {
            if (((MainWindow)System.Windows.Application.Current.MainWindow).isConnected == true)
            {

            };
        }
    }
}
