using Microsoft.Win32;
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
        public HCommand ClearCommand { get; private set; }

        private void SaveFileMethod(object obj)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.CheckPathExists = true;
            fileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            fileDialog.Filter= "Текстовые файлы|*.txt";
            fileDialog.Title = "Сохранить данные";
            if (fileDialog.ShowDialog() == true)
            {
                DataProcessing dataProcessing = new DataProcessing();
                dataProcessing.SetSourses(DataPainter.GetSourses());
                dataProcessing.SetTrajectory(DataPainter.GetTrajectory());

                dataProcessing.CalculateTimes();

                DataFileHelper dataFileHelper = new DataFileHelper();
                dataFileHelper.SetSourses(dataProcessing.GetSourses());
                dataFileHelper.SetTimes(dataProcessing.GetTimes());

                if (dataFileHelper.FileOutputWrite(fileDialog.FileName))                
                    MessageBox.Show("Файл успешно сохранен!\n"+ fileDialog.FileName);
                else
                    MessageBox.Show("Ошибка во времемя записи файла!\n" + dataFileHelper.Message);

                //MessageBox.Show("Сохранение в файл ещё не реализовано!\n"+ fileDialog.FileName);
            }
        }
        public EmulatorViewModel()
        {
            SaveFileCommand = new HCommand(SaveFileMethod);
            ClearCommand = new HCommand(ClearMethod);
        }

        private void ClearMethod(object obj)
        {
            DataPainter.Clear();
        }

        internal void SetView(PathControl view)
        {
            View = view;
            View.pathCanvas.Background = new SolidColorBrush(Colors.White);
            DataPainter = new DataPainter(View.pathCanvas);
            DataPainter.InitializingObject();
            DataPainter.CanMove = true;
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