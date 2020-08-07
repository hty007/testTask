using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GPSTask
{
    internal class DataPainter
    {
        private Canvas _canvas;
        private HPoint Zero;
        private List<Ellipse> Sourses;
        private PathFigure PathFigure;
        private PolyLineSegment PathTrajectory;
        private Point? _movePoint;

        public DataPainter(Canvas pathCanvas)
        {
            _canvas = pathCanvas;                

            double width = _canvas.Width = 500;
            double heigth = _canvas.Height = 400;
            Zero = new HPoint(width / 2, heigth / 2);

            CoordinateHelper.Zero = Zero;
            CoordinateHelper.Scale = 5;

            CoordinateSystem(width, heigth);
            InitializingSourses();
            InitializingPath();
        }

        #region Приемники сигнала
        private void InitializingSourses()// Обязательно переименовать на чтонибудь осмысленноле
        {
            Sourses = new List<Ellipse>();
            Ellipse p1 = Dot(10, 10, "Источник 1");

            p1.MouseDown += Ellipse_MouseDown;
            p1.MouseUp += Ellipse_MouseUp;
            p1.MouseMove += Ellipse_MouseMove;

            Ellipse p2 = Dot(20, 10);

            p2.MouseDown += Ellipse_MouseDown;
            p2.MouseUp += Ellipse_MouseUp;
            p2.MouseMove += Ellipse_MouseMove;

            Ellipse p3 = Dot(30, 10);

            p3.MouseDown += Ellipse_MouseDown;
            p3.MouseUp += Ellipse_MouseUp;
            p3.MouseMove += Ellipse_MouseMove;

            Sourses.Add(p1);
            Sourses.Add(p2);
            Sourses.Add(p3);
            p1.Fill = p2.Fill = p3.Fill = new SolidColorBrush(Colors.Red);
        }

        private void Ellipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Ellipse control && MainVeiwModel.CanMove)
            {
                _movePoint = e.GetPosition(control);
                control.CaptureMouse();
            }
        }

        private void Ellipse_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Ellipse control && MainVeiwModel.CanMove)
            {
                _movePoint = null;
                control.ReleaseMouseCapture();
            }
        }

        private void Ellipse_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is Ellipse control && MainVeiwModel.CanMove)
            {
                if (_movePoint == null) return;
                Point p = e.GetPosition(_canvas) - (Vector)_movePoint.Value/**/;
                Canvas.SetLeft(control, p.X);
                Canvas.SetTop(control, p.Y);
            }
        }

        private Ellipse Dot(double x, double y, string toolTip = null, double radius = 10)
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Height = ellipse.Width = radius;
            if (toolTip != null)
            {
                ellipse.ToolTip = toolTip;
            }
            Canvas.SetLeft(ellipse, x);   // выстовляем x
            Canvas.SetTop(ellipse, y);// выстовляем y
            _canvas.Children.Add(ellipse);
            return ellipse;
        }
        internal void SetSourses(List<HPoint> sourses)
        {
            for (int i = 0; i < sourses.Count; i++)
            {
                HPoint point = sourses[i];
                Point p = CoordinateHelper.Convert(point);
                if (i < Sourses.Count)
                {
                    Ellipse ellipse = Sourses[i];
                    Canvas.SetLeft(ellipse, p.X);   // выстовляем x
                    Canvas.SetTop(ellipse, p.Y);// выстовляем y
                }
                else
                {
                    Ellipse ellipse = Dot(p.X, p.Y);

                    ellipse.MouseDown += Ellipse_MouseDown;
                    ellipse.MouseUp += Ellipse_MouseUp;
                    ellipse.MouseMove += Ellipse_MouseMove;
                    Sourses.Add(ellipse);
                }
            }
        }
        #endregion
        #region Координатное поле

        private void CoordinateSystem(double width, double heigth)
        {
            // Вертикальная ось
            Line(Zero.X, heigth, Zero.X, 0);
            Line(Zero.X, 0, Zero.X - 10, 10);
            Line(Zero.X, 0, Zero.X + 10, 10);
            Text(Zero.X + 15, 0, "у,м");

            // Горизонтальная ось   
            Line(0, Zero.Y, width, Zero.Y);
            Line(width, Zero.Y, width - 10, Zero.Y + 10);
            Line(width, Zero.Y, width - 10, Zero.Y - 10);
            Text(width - 20, Zero.Y + 15, "x,м");
        }

        private void Text(double x, double y, string text, string toolTip = null)
        {
            TextBlock block = new TextBlock();
            block.Text = text;
            block.Height = 15;
            if (toolTip != null)
            {
                block.ToolTip = toolTip;
            }
            Canvas.SetLeft(block, x);   // выстовляем x
            Canvas.SetTop(block, y);// выстовляем y
            _canvas.Children.Add(block);
        }

        public void Line(double x1, double y1, double x2, double y2)
        {
            Line line = new Line();
            line.X1 = x1;
            line.Y1 = y1;
            line.X2 = x2;
            line.Y2 = y2;
            line.Stroke = Brushes.Black;
            //Canvas.SetLeft(line, SelectedUnit.Left);   // выстовляем x
            //Canvas.SetTop(line, SelectedUnit.Top);

            _canvas.Children.Add(line);
        }
        #endregion
        #region Траектория
        private void InitializingPath()
        {
            Path p = new Path();
            p.Stroke = new SolidColorBrush(Colors.Blue);
            p.StrokeThickness = 1;

            PathGeometry pathGeom = new PathGeometry();
            PathFigure = new PathFigure();

            PathFigure.StartPoint = CoordinateHelper.Convert(10.0, 10.0);

            PathTrajectory = new PolyLineSegment();
            PathTrajectory.Points = new PointCollection();
            PathTrajectory.Points.Add(CoordinateHelper.Convert(20.0, 20));
            PathTrajectory.Points.Add(CoordinateHelper.Convert(30.0, 5));
            PathTrajectory.Points.Add(CoordinateHelper.Convert(33.0, 50));
            PathTrajectory.Points.Add(CoordinateHelper.Convert(40.0, 60));
            PathTrajectory.Points.Add(CoordinateHelper.Convert(70.0, 70));


            PathFigure.Segments.Add(PathTrajectory);
            pathGeom.Figures.Add(PathFigure);
            p.Data = pathGeom;
            _canvas.Children.Add(p);
        }

        internal void SetPath(List<HPoint> trajectory)
        {
            if (trajectory == null) return;
            PathTrajectory.Points.Clear();
            bool isStart = true;

            foreach (HPoint p in trajectory)
            {
                if (isStart)
                {
                    PathFigure.StartPoint = CoordinateHelper.Convert(p);
                    isStart = false;
                }
                else
                {
                    PathTrajectory.Points.Add(CoordinateHelper.Convert(p));
                }
            }
        } 
        #endregion

    }
}