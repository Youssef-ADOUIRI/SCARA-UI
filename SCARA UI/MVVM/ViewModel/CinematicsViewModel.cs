using SCARA_UI.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SCARA_UI.MVVM.View;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;

namespace SCARA_UI.MVVM.ViewModel
{
    class CinematicsViewModel : ObservableObject
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

        public CinematicsViewModel()
        {
            ButtonCommand = new RelayCommand(new Action<object>(Exec));
            setPath();
        }

        public void Exec(object o)
        {
            if(o.ToString() == "start")
            {
                //start motors:
                //MessageBox.Show(x.ToString() + " " + y.ToString() );
                setValues();
                
            }
        }


        public void setValues()
        {
            coordonates = "x = " + x.ToString() + "  y = " + y.ToString();
            //MessageBox.Show(coordonates);
        }

        public string _coordonates;
        public string coordonates
        {
            get
            {
                return _coordonates;
            }
            set
            {
                _coordonates = value ;
                OnPropertyChanged("coordonates");
            }
        }



        Path myPath = new Path(); // drawing Scara Path
        EllipseGeometry myEllipseGeometry = new EllipseGeometry();

        public EllipseGeometry geo
        {
            get { return myEllipseGeometry; }
            set
            {
                //myEllipseGeometry = value;
                OnPropertyChanged("geo");
            }
        }

        public Canvas DrawCanvas
        {
            set;
            get;
        }

        private void setPath()
        {
            myPath.Stroke = Brushes.Yellow;
            myPath.StrokeThickness = 4;
            myPath.HorizontalAlignment = HorizontalAlignment.Left;
            myPath.VerticalAlignment = VerticalAlignment.Center;
            myEllipseGeometry.Center = new Point(50, 50);
            myPath.Visibility = Visibility.Visible;
            myEllipseGeometry.RadiusX = 50;
            myEllipseGeometry.RadiusY = 25;
            myPath.Data = myEllipseGeometry;
            DrawCanvas = new Canvas();
            DrawCanvas.Background = Brushes.DarkGray;
            DrawCanvas.Children.Add(myPath);
        }





        


        public double _x;
        public double _y;
        public double x
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
                OnPropertyChanged("x");
            }
        }
        public double y
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
                OnPropertyChanged("y");
            }
        }

        private const double L1 = 175 , L2 = 175;

        MainWindow main = (MainWindow)System.Windows.Application.Current.MainWindow;

        public Tuple<double, double> IK(Tuple<double, double> point)
        {
            double t1 = 0 , t2 = 0;
            double x = point.Item1;
            double y = point.Item2;

            // Inverse kinematics
            double A = (x * x + y * y) - (L1 * L1 + L2 * L2);
            double B = 2 * L1 * L2;

            t2 = Math.Acos(A / B);

            double C = y * (L1 + L2 * Math.Cos(t2)) - x * L2 * Math.Cos(t2);
            double D = x * (L1 + L2 * Math.Cos(t2)) - y * L2 * Math.Cos(t2);

            t1 = Math.Atan(C / D);


            Tuple<double, double> tetas = new Tuple<double, double>(t1 , t2);

            return tetas;
        }

        public void submit(Tuple<double, double> tetas)
        {
            string cmd = tetas.Item1.ToString() + " " + tetas.Item2.ToString();
            main.writeIn("b " + cmd);
        }

    }
}
