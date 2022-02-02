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

        MainWindow main = (MainWindow)System.Windows.Application.Current.MainWindow;

        public void test(object b)
        {
            if (main.sp.IsOpen == true )
            {
                if(b.ToString().ToUpper() == "A")
                {
                    main.writeIn("a");
                }
                else if (b.ToString().ToUpper() == "B")
                {
                    main.writeIn("b");
                }
            };
        }
    }
}
