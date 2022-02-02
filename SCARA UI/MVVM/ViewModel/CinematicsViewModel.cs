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
using System.Windows.Threading;
using System.Threading;
using System.Windows.Media.Animation;
using Microsoft.Win32;
using System.IO;
using System.IO.Ports;

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
            if (main.sp.IsOpen)
            switch (o.ToString())
            {
                case "P":
                    setValues();
                    break;
                case "D":
                    setValues();
                    break;
                case "R":
                    List<Tuple<float, float>> points = linePoints(150, 150, 200, 100);
                    for(int i = 0; i < points.Count; i++)
                    {
                        string cmd = points[i].Item1.ToString("#.##") + " " + points[i].Item2.ToString("#.##") + " " + s.ToString("#.##");
                        var tet = IK(points[i].Item1 , points[i].Item2);
                        rotateL1(tet.Item1 * (Math.PI / 180));
                        rotateL2(tet.Item2 * (Math.PI / 180));
                        submitP(tet);
                    }
                    break;
                case "G":
                    string p = LoadG_code();
                    if (p != string.Empty)
                    {
                        path = p;
                    }
                    break;
                case "E":
                    string G_cmd = G_code_file.ReadLine();
                    do
                    {
                        if (G_cmd != null)
                        {
                            Tuple<double, double, double> g = readG_codeLine(G_cmd);
                            Tuple<double, double> cor = IK(g.Item1, g.Item2); // this is a dumb way to do it
                            string cmd = cor.Item1.ToString("#.##") + " " + cor.Item2.ToString("#.##") + " " + ((int)(g.Item3)).ToString("#");
                            main.writeIn("P " + cmd);
                            rotateL1(cor.Item1 * (Math.PI / 180));
                            rotateL2(cor.Item2 * (Math.PI / 180));
                            // should wait here
                        }
                        else
                        {
                            MessageBox.Show("G-code is finished");
                            break;
                        }
                        G_cmd = G_code_file.ReadLine();
                    } while (G_cmd != null);
                    
                    break;
            }
            else
            {
                MessageBox.Show("Can't be executed, please connect");
            }
        }


        private Tuple<double , double , double> readG_codeLine(string buffer)
        {
            double a1 = 0, a2 = 0, a3 = 0;
            if (buffer[0] == 'G')
            {
                string[] comands = buffer.Split(' ');
                foreach (string cm in comands)
                {
                    char c = cm[0];
                    switch (c)
                    {
                        case 'G':
                            break;
                        case 'X':
                            if (double.TryParse(cm, out double j1))
                            {
                                a1 = j1;
                            }
                            break;
                        case 'Y':
                            if (double.TryParse(cm, out double j2))
                            {
                                a2 = j2;
                            }
                            break;
                        case 'F':
                           if( double.TryParse(cm , out double j3))
                            {
                                a3 = j3;
                            }
                            break;
                    }
                }
            }
            return Tuple.Create(a1, a2, a3);
        }




        public StreamReader G_code_file;// gcode
        private string LoadG_code()
        {
            string Path = string.Empty;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                Path = openFileDialog.FileName;
                G_code_file = new StreamReader(Path);
            }
            return Path;
        }

        private string _path = "empty";
        public string path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
                OnPropertyChanged("path");
            }
        }

        public void setValues()
        {
            var tet = IK(x , y);
            coordonates = "x = " + x.ToString() + "  y = " + y.ToString(); 
            submitP(tet);
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

        //use the DDA Algorithm
        public List<Tuple<float , float>> linePoints(float x1 , float y1 , float x2 , float y2) 
        {
            List<Tuple<float, float>> points = new List<Tuple<float, float>>();
            float step;
            float dx = (x2 - x1);
            float dy = (y2 - y1);
            if (Math.Abs(dx) >= Math.Abs(dy))
                step = Math.Abs(dx);
            else
                step = Math.Abs(dy);
            dx = dx / step;
            dy = dy / step;
            float x_o = x1;
            float y_o = y1;
            int i = 1;
            while (i <= step)
            {
                //add x and y to list
                points.Add(Tuple.Create(x_o , y_o));
                x_o += dx;
                y_o += dy;
                i++;
            }
            return points;
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

        public Tuple<double, double> IK(double x_u, double  y_u)
        {
            double t1, t2;

            // Inverse kinematics
            double A = x_u * x_u + y_u * y_u - (L1 * L1 + L2 * L2);
            double B = 2 * L1 * L2;

            t2 = Math.Acos( A / B);

            double C = y_u * (L1 + L2 * Math.Cos(t2)) - x_u * L2 * Math.Sin(t2);
            double D = x_u * (L1 + L2 * Math.Cos(t2)) + y_u * L2 * Math.Sin(t2);

            t1 = Math.Atan2(C , D);
            //teta1 = t1;
            //teta2 = t2;
            t1 = 180 - (t1 * (180 / Math.PI));
            t2 = 180 - (t2 * (180 / Math.PI));

            if (t1 < 0)
                t1 += 360;
            else if (t2 < 0)
                t2 += 360;

            Tuple<double, double> tetas = new Tuple<double, double>(t1 , t2);

            return tetas;
        }

        


        public void submitP(Tuple<double, double> tetas)
        {
            string cmd = tetas.Item1.ToString("#.##") + " " + tetas.Item2.ToString("#.##") + " " + s.ToString("#.##");
            //main.writeIn("P " + cmd); 
            //sp.WriteLine("P " + cmd);
            //var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.02) };

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

            rotateL1(0);
        }

        private double oldt = 0.0f, oldt2 = 0.0f;
        public double scale;

        public void rotateL1(double t1)
        {
            scale = 500 / 6;
            c1.X = 250 + scale * Math.Sin(t1);
            c1.Y = 250 - scale * Math.Cos(t1);
            //await Task.Delay(2000);
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
            //await Task.Delay(2000);
            ss2();
        }


    }
}
