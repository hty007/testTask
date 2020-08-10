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
        #region Поля и свойства
        private Canvas _canvas;
        private HPoint Zero;
        private List<Ellipse> Sourses;
        private PathFigure PathFigure;
        private PolyLineSegment PathTrajectory;
        private Point? _movePoint;
        private List<HPoint> Trajectory;
        public Ellipse ObjectDot;

        public bool CanMove { get; set; }


        internal List<HPoint> GetSourses()
        {
            List<HPoint> sourses = new List<HPoint>();

            foreach (var item in Sourses)
            {
                sourses.Add(CoordinateHelper.BackConvert(Canvas.GetLeft(item), Canvas.GetTop(item)));
            }
            return sourses;
        }

        internal List<HPoint> GetTrajectory()
        {// Посчитать и вернуть список времен
            return Trajectory;
        } 
        #endregion
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
            if (sender is Ellipse control && CanMove)
            {
                _movePoint = e.GetPosition(control);
                control.CaptureMouse();
            }
        }

        private void Ellipse_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Ellipse control && CanMove)
            {
                _movePoint = null;
                control.ReleaseMouseCapture();
            }
        }

        private void Ellipse_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is Ellipse control && CanMove)
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

            var trajectory = new List<HPoint>();
            trajectory.Add(new HPoint(5, 20));
            trajectory.Add(new HPoint(10, 10));
            trajectory.Add(new HPoint(15, 5));
            trajectory.Add(new HPoint(20.0, 25));
            trajectory.Add(new HPoint(25.0, 28));
            trajectory.Add(new HPoint(30.0, -20));

            PathTrajectory = new PolyLineSegment();
            PathTrajectory.Points = new PointCollection();
            PathFigure.Segments.Add(PathTrajectory);
            pathGeom.Figures.Add(PathFigure);
            p.Data = pathGeom;
            _canvas.Children.Add(p);
            SetPath(trajectory);
        }

        public void InitializingObject()
        {
            
            var p = CoordinateHelper.Convert(0,0);
            ObjectDot = Dot(p.X, p.Y, "Объект");
            ObjectDot.Fill = new SolidColorBrush(Colors.Green);

            ObjectDot.MouseDown += Object_MouseDown;
            ObjectDot.MouseUp += Object_MouseUp;
            ObjectDot.MouseMove += Object_MouseMove;
            if (Trajectory.Count != 0)
            {
                Point lastPoint = CoordinateHelper.Convert(Trajectory[Trajectory.Count - 1]);
                Canvas.SetLeft(ObjectDot, lastPoint.X);     // выстовляем x
                Canvas.SetTop(ObjectDot, lastPoint.Y);      // выстовляем y
            }
        }
        internal void Clear()
        {
            Trajectory.Clear();
            PathTrajectory.Points.Clear();
            Point startPoint = new Point(Canvas.GetLeft(ObjectDot), Canvas.GetTop(ObjectDot));
            PathFigure.StartPoint = startPoint;
            Trajectory.Add(CoordinateHelper.BackConvert(startPoint));
        }

        private void Object_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is Ellipse control && CanMove)
            {
                if (_movePoint == null) return;
                Point p = e.GetPosition(_canvas) - (Vector)_movePoint.Value/**/;
                Canvas.SetLeft(control, p.X);
                Canvas.SetTop(control, p.Y);
                Trajectory.Add(CoordinateHelper.BackConvert(p));
                PathTrajectory.Points.Add(p);
            } 
        }

        private void Object_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Ellipse control && CanMove)
            {
                _movePoint = null;
                control.ReleaseMouseCapture();
            }
        }

        private void Object_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Ellipse control && CanMove)
            {
                _movePoint = e.GetPosition(control);
                control.CaptureMouse();
            }
        }

        internal void SetPath(List<HPoint> trajectory)
        {
            if (trajectory == null) return;
            Trajectory = trajectory;
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
            if (ObjectDot != null)
            {
                Point lastPoint = CoordinateHelper.Convert(Trajectory[Trajectory.Count - 1]);
                Canvas.SetLeft(ObjectDot, lastPoint.X);     // выстовляем x
                Canvas.SetTop(ObjectDot, lastPoint.Y);      // выстовляем y
            }
        } 
        #endregion
        public DataPainter(Canvas pathCanvas)
        {
            _canvas = pathCanvas;                

            // По идее размер полотна надо вынести из класса дабы была возможность его менять.
            double width = _canvas.Width = 500;
            double heigth = _canvas.Height = 400;
            Zero = new HPoint(width / 2, heigth / 2);

            CoordinateHelper.Zero = Zero;
            CoordinateHelper.Scale = 5;

            CoordinateSystem(width, heigth);
            InitializingSourses();
            InitializingPath();
        }

    }
}