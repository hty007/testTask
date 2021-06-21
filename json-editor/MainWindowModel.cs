using Microsoft.Win32;
using System;
using System.Windows;
using Teko.Test.Editor.Models;
using WPFStorage.Base;
using WPFStorage.Dialogs;

namespace Teko.Test.Editor
{
    internal class MainWindowModel: ObservableObject
    {
        private static readonly string DEFAULT_SAVE_FILE_NAME = "output";
        private static readonly string DEFAULT_OPEN_FILE_NAME = "input";
        private FileModel file;

        public MainWindowModel()
        {
            File = new FileModel();

            OpenDefailtCommand = new RelayCommand(()=> File.Open(DEFAULT_OPEN_FILE_NAME));
            SaveDefailtCommand = new RelayCommand(() => File.Save(DEFAULT_SAVE_FILE_NAME));
            OpenFileCommand = new RelayCommand(Open);
            CreateFileCommand = new RelayCommand(Create);
            SaveFileCommand = new RelayCommand(Save);
            CloseFileCommand = new RelayCommand(Close);
            ExitCommand = new RelayCommand(Exit);

            CreateRecordCommand = new RelayCommand(CreateRecord);
            EditRecordCommand = new RelayCommand(EditRecord);
            RemoveRecordCommand = new RelayCommand(RemoveRecord);
            MoveUpRecordCommand = new RelayCommand(MoveUpRecord);
            MoveDownRecordCommand = new RelayCommand(MoveDownRecord);
        }


        public FileModel File { get => file; set => SetProperty(ref file, value); }

        public RelayCommand OpenFileCommand { get; }
        public RelayCommand CreateFileCommand { get; }
        public RelayCommand SaveFileCommand { get; }
        public RelayCommand CloseFileCommand { get; }
        public RelayCommand ExitCommand { get; }
        public RelayCommand CreateRecordCommand { get; }
        public RelayCommand EditRecordCommand { get; }
        public RelayCommand RemoveRecordCommand { get; }
        public RelayCommand MoveUpRecordCommand { get; }
        public RelayCommand MoveDownRecordCommand { get; }
        public RelayCommand OpenDefailtCommand { get; }
        public RelayCommand SaveDefailtCommand { get; }

        private void Exit() => Application.Current.Shutdown();

        private void MoveDownRecord()
        {
            WinBox.ShowMessage("Это программисты ещё не предусмотрели!");
        }

        private void MoveUpRecord()
        {
            WinBox.ShowMessage("Это программисты ещё не предусмотрели!");
        }

        private void RemoveRecord()
        {
            WinBox.ShowMessage("Это программисты ещё не предусмотрели!");
        }

        private void EditRecord()
        {
            WinBox.ShowMessage("Это программисты ещё не предусмотрели!");
        }

        private void CreateRecord()
        {
            WinBox.ShowMessage("Это программисты ещё не предусмотрели!");
        }

        private void Close()
        {
            WinBox.ShowMessage("Это программисты ещё не предусмотрели!");
        }

        private void Save()
        {
            if (!File.IsOpen)
            {
                WinBox.ShowMessage("Нет открытого файла." ,"Сохранение не возможно!");
                return;
            }

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            dialog.Title = "Выберите как сохранить файл";
            dialog.FileName = DEFAULT_SAVE_FILE_NAME;
            var result = dialog.ShowDialog();
            if (result == true)
            {
                File.Save(dialog.FileName);
            }
        }

        private void Create()
        {
            WinBox.ShowMessage("Это программисты ещё не предусмотрели!");
        }

        private void Open()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            dialog.Title = "Выберите файл для открытия";
            dialog.FileName = DEFAULT_OPEN_FILE_NAME;

            var result = dialog.ShowDialog();
            if (result == true)
            {
                File.Open(dialog.FileName);
            }
        }

    }
}