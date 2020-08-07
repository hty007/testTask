using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace GPSTask
{
    public class EmulatorViewModel : BaseViewModel
    {
        private PathControl View;
        private DataPainter DataPainter;
        private string position;

        public string Position { get => position; set { position = value; OnPropertyChanged("Position"); } }
        public HCommand SaveFileCommand { get; private set; }

        private void SaveFileMethod(object obj)
        {
            MessageBox.Show("Сохранение в файл ещё не реализовано!");
        }
        public EmulatorViewModel()
        {
            SaveFileCommand = new HCommand(SaveFileMethod);
        }



        internal void SetView(PathControl view)
        {
            View = view;
            View.pathCanvas.Background = new SolidColorBrush(Colors.White);
            DataPainter = new DataPainter(View.pathCanvas);
            View.pathCanvas.MouseMove += PathCanvas_MouseMove;
        }

        private void PathCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            var p = e.GetPosition(View.pathCanvas);
            HPoint point = CoordinateHelper.BackConvert(p);
            Position = $"{point.X:f1},{point.Y:f1}";
        }
    }
}