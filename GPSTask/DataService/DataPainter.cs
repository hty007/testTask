using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GPSTask
{
    internal class DataPainter
    {
        private Grid Grid;

        public DataPainter(Grid grid)
        {
            Grid = grid;

            double heigth = Grid.Height;
            double width = Grid.Width;

            Line(0.0, 0.0, 10.0, 10.0);

        }

        private void Line(double x1, double y1, double x2, double y2)
        {
            Line vertL = new Line();
            vertL.X1 = x1;
            vertL.Y1 = y1;
            vertL.X2 = x2;
            vertL.Y2 = y2;
            vertL.Stroke = Brushes.Black;
            Grid.Children.Add(vertL);            
        }

        internal void SizeChanged(object sender, SizeChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}