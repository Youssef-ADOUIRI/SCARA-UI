using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using SCARA_UI.MVVM.ViewModel;

namespace SCARA_UI.MVVM.View
{
    /// <summary>
    /// Interaction logic for Cinematics.xaml
    /// </summary>
    /// 
    
    public partial class Cinematics : UserControl
    {

        public Cinematics()
        {
            InitializeComponent();
            //CinematicsViewModel cvm = new CinematicsViewModel();
            //myCanvas = cvm.setCanvas();

        }

        /*


        public Path center;
        public Path center2 = new Path();
        int radius = 5;
        private EllipseGeometry geoC;
        private EllipseGeometry geoC2;
        public Line l1;
        public Line l2;
        private Line hl;
        private Line vl;
        public Point c;

        private void setCanvas()
        {
            Color b = (Color)ColorConverter.ConvertFromString("#FF282828");
            myCanvas.Background = new SolidColorBrush(b);
            myCanvas.Width = 500;
            myCanvas.Height = 500;

            //set grid
            hl = new Line();
            hl.Visibility = Visibility.Visible;
            hl.Stroke = Brushes.LightGray;
            hl.Opacity = 0.9;
            hl.StrokeThickness = 1;
            //hl.Height = myCanvas.Height;
            hl.X1 = myCanvas.Width / 2;
            hl.Y1 = 0;
            hl.X2 = hl.X1;
            hl.Y2 = myCanvas.Height;
            //Canvas.SetLeft(hl, myCanvas.Width / 2);
            //Canvas.SetBottom(hl, myCanvas.Height / 2);
            myCanvas.Children.Add(hl);


            vl = new Line();
            vl.Visibility = Visibility.Visible;
            vl.Stroke = Brushes.LightGray;
            vl.Opacity = 0.9;
            vl.StrokeThickness = 1;
            //hl.Height = myCanvas.Height;
            vl.X1 = 0;
            vl.Y1 = myCanvas.Height / 2;
            vl.X2 = myCanvas.Width;
            vl.Y2 = vl.Y1;
            myCanvas.Children.Add(vl);

            //first joint
            
            c = new Point(myCanvas.Width / 2, myCanvas.Height / 2);
            geoC = new EllipseGeometry(c, radius, radius);
            center = new Path
            {
                Stroke = Brushes.White,
                StrokeThickness = 2,
                Visibility = Visibility.Visible,
                Data = geoC
            };
            //center.RenderTransformOrigin = new Point(5, 5);
            //center.HorizontalAlignment = HorizontalAlignment.Center;
            //center.VerticalAlignment = VerticalAlignment.Center;
            //geoC.Center = c;
            //geoC.RadiusX = 10;
            //geoC.RadiusY = 10;
            //Canvas.SetLeft(center, (myCanvas.Width / 2 )- center.Width);
            //Canvas.SetBottom(center, (myCanvas.ActualHeight / 2) - center.ActualHeight);
            myCanvas.Children.Add(center);



            //bras 1
            l1 = new Line();
            l2 = new Line();
            l1.X1 = myCanvas.Width / 2;
            l1.Y1 = myCanvas.Height / 2;
            rotateL1(0);
            l1.Visibility = Visibility.Visible;
            l1.Stroke = Brushes.White;
            l1.StrokeThickness = 2;
            //Canvas.SetLeft(center, myCanvas.Width / 2);
            //Canvas.SetBottom(center, myCanvas.Height / 2);
            myCanvas.Children.Add(l1);

            //Center 2
            c2 = new Point(l1.X2, l1.Y2);
            geoC2 = new EllipseGeometry(c2, radius * 2 / 3, radius * 2 / 3);
            center2.Stroke = Brushes.White;
            center2.StrokeThickness = 2;
            center2.Visibility = Visibility.Visible;
            //geoC2.RadiusX = radius * 2 / 3;
            //geoC2.RadiusY = geoC2.RadiusX;
            geoC2.Center = new Point(l1.X2, l1.Y2);
            center2.Data = geoC2;
            myCanvas.Children.Add(center2);

            //bras 2
            l2.Visibility = Visibility.Visible;
            l2.Stroke = Brushes.White;
            l2.StrokeThickness = 2;
            myCanvas.Children.Add(l2);
        }

        MainWindow main = (MainWindow)Application.Current.MainWindow;

        public double scale;
        private Point c2;
        public void rotateL1(double t1)
        {
            scale = myCanvas.Height/6 ;
            l1.X2 = l1.X1 + scale * Math.Sin(t1);
            l1.Y2 = l1.Y1 - scale * Math.Cos(t1);
            center2.Data = new EllipseGeometry(new Point(l1.X2, l1.Y2), radius * 2 / 3, radius * 2 / 3);
            l2.X1 = l1.X2;
            l2.Y1 = l1.Y2;
            oldt = t1;
            rotateL2(oldt2);
        }

        private double oldt = 0.0f , oldt2 = 0.0f;

        private void Exec_button_Click(object sender, RoutedEventArgs e)
        {
            double teta1 = 0 , teta2 = 0;
            if (double.TryParse(t1.Text , out  double j1))
                teta1 = j1; rotateL1(teta1); MessageBox.Show(teta1.ToString("#.##"));
            if (double.TryParse(t2.Text, out double j2))
                teta2 = j2; rotateL2(teta2); 
        }

        public void rotateL2(double t2)
        {
            scale = myCanvas.Height / 6;
            l2.X2 = l2.X1 + scale * Math.Sin(oldt - t2);
            l2.Y2 = l2.Y1 - scale * Math.Cos(oldt - t2);
            oldt2 = t2;
        }
        */

    }
}
