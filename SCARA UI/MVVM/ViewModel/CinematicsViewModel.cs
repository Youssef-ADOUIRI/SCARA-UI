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
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SCARA_UI.MVVM.ViewModel
{

    class ShapeData : ObservableObject
    {
        private string type;
        public string Type 
        { 
            get 
            { 
                return type; 
            } 
            set 
            { 
                type = value;
                OnPropertyChanged("Type");
            } 
        }
        private Geometry geometry;
        public Geometry Geometry 
        { 
            get
            {
                return geometry;
            }
            set
            {
                geometry = value;
                OnPropertyChanged("Geometry");
            }
        }
        private Brush fill;
        public Brush Fill { get { return fill; } set { fill = value; OnPropertyChanged("Fill"); } }
        private Brush stroke;
        public Brush Stroke { get { return stroke; } set { stroke = value; OnPropertyChanged("Stroke"); } }
        public double StrokeThickness { get; set; }
        public float opacity { get; set; }
    }


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
            setCanvas();
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
            var tet = IK(Tuple.Create(x, y));
            double t1 = tet.Item1;
            double t2 = tet.Item2;
            coordonates = "x = " + x.ToString() + "  y = " + y.ToString(); /*+ "   " + t1.ToString() + " " + t2.ToString() + " s = " + s.ToString()*/
            submit(tet);
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
        

        public Canvas DrawCanvas
        {
            set;
            get;
        }

        private void setPath()
        {
            L1 = main.L1;
            L2 = main.L2;

            
        }


        private BindingList<ShapeData> _ShapeItems;
        public BindingList<ShapeData> ShapeItems { 
            get { 
                return _ShapeItems; 
            } 
            set {
                _ShapeItems = value;
                OnPropertyChanged("ShapeItems");
            } 
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

        public double _s;
        public double s
        {
            get
            {
                return _s;
            }
            set
            {

                _s = value;
                OnPropertyChanged("s");

            }
        }


        private double L1 = 175 , L2 = 175;

        MainWindow main = (MainWindow)System.Windows.Application.Current.MainWindow;

        public Tuple<double, double> IK(Tuple<double, double> point)
        {
            double t1, t2;
            double x = point.Item1;
            double y = point.Item2;

            // Inverse kinematics
            double A = x * x + y * y - (L1 * L1 + L2 * L2);
            double B = 2 * L1 * L2;

            t2 = Math.Acos(A / B);

            double C = y * (L1 + L2 * Math.Cos(t2)) - x * L2 * Math.Cos(t2);
            double D = x * (L1 + L2 * Math.Cos(t2)) - y * L2 * Math.Cos(t2);

            t1 = Math.Atan2(C , D);
            //teta1 = t1;
            //teta2 = t2;


            Tuple<double, double> tetas = new Tuple<double, double>(180 - (t1 * (180 / Math.PI)), 180 - (t2 * (180 / Math.PI)));

            return tetas;
        }

        


        public void submit(Tuple<double, double> tetas)
        {
            string cmd = tetas.Item1.ToString("#.##") + " " + tetas.Item2.ToString("#.##") + " " + s.ToString("#.##");
            main.writeIn("P " + cmd);
            rotateL1(tetas.Item1 * (Math.PI / 180));
            rotateL2(tetas.Item2 * (Math.PI / 180));
        }

        
        private Point c1;
        private Point c2;
        int radius = 5;
        public ShapeData s1;
        public ShapeData s2;

        private void ss1()
        {
            s1.Geometry = new LineGeometry(new Point(250, 250), c1);
            ShapeItems[3] = s1;
            ShapeItems[4].Geometry = new EllipseGeometry(c1, radius * 2 / 3, radius * 2 / 3);
        }
        private void ss2()
        {
            s2.Geometry = new LineGeometry(c1, c2);
            ShapeItems[5] = s2;
        }

        private void setCanvas()
        {
            //add canvas grid
            ShapeItems = new BindingList<ShapeData>();
            ShapeItems.Add(new ShapeData
            {
                Type = "Circle",
                Geometry = new EllipseGeometry(new Point(250, 250), radius, radius),
                Fill = Brushes.Transparent,
                Stroke = Brushes.LightGray,
                StrokeThickness = 1,
                opacity = 0.8f
            });

            ShapeItems.Add(new ShapeData
            {
                Type = "Line",
                Geometry = new LineGeometry(new Point(0, 250), new Point(500, 250)),
                Fill = Brushes.Transparent,
                Stroke = Brushes.LightGray,
                StrokeThickness = 1,
                opacity = 0.8f
            });
            ShapeItems.Add(new ShapeData
            {
                Type = "Line",
                Geometry = new LineGeometry(new Point(250, 0), new Point(250, 500)),
                Fill = Brushes.Transparent,
                Stroke = Brushes.LightGray,
                StrokeThickness = 1,
                opacity = 0.8f
            });


            //movable object
            c1 = new Point(250, 500 / 6);
            c2 = new Point(250, 500 * 2 / 6);
            
            //first arm 3
            s1 = new ShapeData
            {
                Type = "Line",
                Geometry = new LineGeometry(new Point(250, 250), c1),
                Fill = Brushes.Transparent,
                Stroke = Brushes.White,
                StrokeThickness = 2,
                opacity = 1
            };
            ShapeItems.Add(s1);
            
            


            //joint 4
            ShapeItems.Add(new ShapeData
            {
                Type = "Circle",
                Geometry = new EllipseGeometry(c1, radius * 2/3, radius*2/3),
                Fill = Brushes.White,
                Stroke = Brushes.LightGray,
                StrokeThickness = 1,
                opacity = 0.8f
            });

            //second arm 5
            s2 = new ShapeData
            {
                Type = "Line",
                Geometry = new LineGeometry(c1, c2),
                Fill = Brushes.Transparent,
                Stroke = Brushes.White,
                StrokeThickness = 2,
                opacity = 1
            };
            ShapeItems.Add(s2);

            rotateL1(Math.PI/4);
        }

        private double oldt = 0.0f, oldt2 = 0.0f;
        public double scale;

        public void rotateL1(double t1)
        {
            scale = 500 / 6;
            c1.X = 250 + scale * Math.Sin(t1);
            c1.Y = 250 - scale * Math.Cos(t1);
            ss1();
            oldt = t1;
            rotateL2(oldt2);
        }

        public void rotateL2(double t2)
        {
            scale = 500 / 6;
            c2.X = c1.X + scale * Math.Sin(oldt - t2);
            c2.Y = c1.Y - scale * Math.Cos(oldt - t2);
            oldt2 = t2;
            ss2();
        }


    }
}
