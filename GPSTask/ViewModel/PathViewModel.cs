using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace GPSTask
{
    public class PathViewModel : BaseViewModel
    {
        internal PathControl View;
        
        string fileName;
        private DataFileHelper DataFileHelper;
        private DataProcessing DataProcessing;
        private DataPainter DataPainter;
        private string position;

        public string FileName { get => fileName; set { fileName = value; OnPropertyChanged("FileName"); } }

        public HCommand SelectFileCommand { get; private set; }
        public HCommand SaveFileOutCommand { get; private set; }
        public HCommand DebagCommand { get; private set; }
        public string Position { get=>position;  set { position = value; OnPropertyChanged("Position"); } }

        private void SelectFileMethod(object obj)
        {

            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.CheckPathExists = true;
            fileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            fileDialog.Title = "Открыть файл входных данных";
            if (fileDialog.ShowDialog() == true)
            {
                DataFileHelper = new DataFileHelper();
                if (!DataFileHelper.ReadInputFile(fileDialog.FileName))
                {
                    MessageBox.Show(DataFileHelper.Message);
                }
                DataProcessing = new DataProcessing(DataFileHelper);

                DataProcessing.Processing();

                DataPainter.SetPath(DataProcessing.GetTrajectory());
                DataPainter.SetSourses(DataProcessing.GetSourses());

                //FileName = fileDialog.SafeFileName;
                FileName = fileDialog.FileName;
                MessageBox.Show("Файл успешно открыт!");
            }
        }

        public PathViewModel()
        {            
            SelectFileCommand = new HCommand(SelectFileMethod);
            SaveFileOutCommand = new HCommand(SaveFileOutMethod);
            DebagCommand = new HCommand(DebagMethod);
        }

        private void SaveFileOutMethod(object obj)
        {
            if (DataProcessing == null) return;
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.CheckPathExists = true;
            fileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            fileDialog.Filter = "Текстовые файлы|*.txt";
            fileDialog.Title = "Сохранить данные";
            if (fileDialog.ShowDialog() == true)
            {                
                DataFileHelper = new DataFileHelper();
                DataFileHelper.SetTrajectory(DataProcessing.GetTrajectory());
                if (!DataFileHelper.FileOutputWrite(fileDialog.FileName))
                {
                    MessageBox.Show(DataFileHelper.Message);
                }
                MessageBox.Show("Файл успешно сохранен!\n" + fileDialog.FileName);
            }
        }

        private void DebagMethod(object obj)
        {
            //DataPainter.Line(10, 10, 20, 10);
        }

        internal void SetView(PathControl view)
        {
            View = view;
            View.pathCanvas.Background = new SolidColorBrush(Colors.White);
            DataPainter = new DataPainter(View.pathCanvas);
            DataPainter.CanMove = false;
            View.pathCanvas.MouseMove += PathCanvas_MouseMove;
            //View.mainGrid.SizeChanged += DataPainter.SizeChanged;
        }

        private void PathCanvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var p = e.GetPosition(View.pathCanvas);
            HPoint point = CoordinateHelper.BackConvert(p);
            Position = $"{point.X:f1},{point.Y:f1}";

        }
    }
}