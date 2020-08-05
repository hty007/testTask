using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Media;

namespace GPSTask
{
    public class PathViewModel : BaseViewModel
    {
        internal PathControl View;
        
        string fileName;
        private DataReader DataReader;
        private DataProcessing DataProcessing;

        public string FileName { get => fileName; set => fileName = value; }

        public HCommand SelectFileCommand { get; private set; }
        private void SelectFileMethod(object obj)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.CheckPathExists = true;
            fileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            fileDialog.Title = "Открыть файл входных данных";
            if (fileDialog.ShowDialog() == true)
            {
                DataReader = new DataReader();
                DataReader.Open(fileDialog.FileName);
                DataProcessing = new DataProcessing(DataReader);


                MessageBox.Show("Файл открыт!");
            }
        }

        public PathViewModel()
        {
            SelectFileCommand = new HCommand(SelectFileMethod);
        }

    }
}