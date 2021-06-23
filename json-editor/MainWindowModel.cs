using Microsoft.Win32;
using System;
using System.Windows;
using Teko.Test.Editor.Models;
using WPFStorage.Base;
using WPFStorage.Dialogs;

namespace Teko.Test.Editor
{
    internal class MainWindowModel : ObservableObject
    {
        private static readonly string DEFAULT_SAVE_FILE_NAME = "output";
        private static readonly string DEFAULT_OPEN_FILE_NAME = "input";
        private const string OPTION_PROPERTY = "property";
        private const string OPTION_OBJECT = "object";
        private const string OPTION_ARRAY = "array";
        private static readonly string [] COLLECTION_OPTIONS = new string[] { OPTION_PROPERTY, OPTION_OBJECT, OPTION_ARRAY };
        private FileModel file;
        private string message;
        private Record currentRecord;

        public MainWindowModel()
        {
            File = new FileModel();

            OpenDefailtCommand = new RelayCommand(() => File.Open(DEFAULT_OPEN_FILE_NAME));
            SaveDefailtCommand = new RelayCommand(() => File.Save(DEFAULT_SAVE_FILE_NAME));
            OpenFileCommand = new RelayCommand(Open);
            CreateFileCommand = new RelayCommand(Create);
            SaveFileCommand = new RelayCommand(Save);
            CloseFileCommand = new RelayCommand(Close);
            ExitCommand = new RelayCommand(Exit);

            AddRecordAfterCommand = new RelayCommand(AddRecordAfter);
            AddRecordInsideCommand = new RelayCommand(AddRecordInside);
            EditRecordCommand = new RelayCommand(EditRecord);
            RemoveRecordCommand = new RelayCommand(RemoveRecord);

            ShowTaskTextCommand = new RelayCommand(ShowTaskText);
        }


        public string Message { get => message; set => SetProperty(ref message, value); }
        public FileModel File { get => file; set => SetProperty(ref file, value); }

        public RelayCommand OpenFileCommand { get; }
        public RelayCommand CreateFileCommand { get; }
        public RelayCommand SaveFileCommand { get; }
        public RelayCommand CloseFileCommand { get; }
        public RelayCommand ExitCommand { get; }
        public RelayCommand AddRecordAfterCommand { get; }
        public RelayCommand AddRecordInsideCommand { get; }
        public RelayCommand EditRecordCommand { get; }
        public RelayCommand RemoveRecordCommand { get; }
        public RelayCommand ShowTaskTextCommand { get; }
        public RelayCommand OpenDefailtCommand { get; }
        public RelayCommand SaveDefailtCommand { get; }

        private void Exit() => Application.Current.Shutdown();

        private void AddRecordInside()
        {
            if (CheckCurrentRecordValidate())
            {
                var query = WinBox.SelectorBox(
                    collect: COLLECTION_OPTIONS,
                    question: "Выберите тип новой записи:");
                Record record = null;
                switch (query)
                {
                    case OPTION_PROPERTY: 
                        record = currentRecord.AddPropertyInside("property", "value");
                        break;
                    case OPTION_OBJECT:
                        record = currentRecord.AddObjectInside("obj");
                        break;
                    case OPTION_ARRAY:
                        record = currentRecord.AddArrayInside("array");
                        break;
                    default:
                        break;
                }


                if (record == null)
                {
                    WinBox.ShowMessage("Не удалось завершить операцию.", "Ошибка");
                }

            }
        }

        internal void RecordChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is Record newRec)
                currentRecord = newRec;
            if (e.OldValue is Record oldRec)
                oldRec.EditMode = false;
        }

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
            if (CheckCurrentRecordValidate())
            {
                if (currentRecord.IsRoot)
                {
                    File.Children.Remove(currentRecord);
                    return;
                }

                currentRecord.Detete();
            }
        }

        private bool CheckCurrentRecordValidate()
        {
            if (currentRecord == null)
            {
                WinBox.ShowMessage("Предел чем выполнять операцию необходимо выбрать запись.", "Нет выбранной записи!");
                return false;
            }
            return true;
        }

        private void EditRecord()
        {
            currentRecord.EditMode = true;
        }

        private void AddRecordAfter()
        {
            if (CheckCurrentRecordValidate())
            {
                Record record = currentRecord.AddPropertyAfter("rec", "value");
                
                if (record == null)
                {
                    WinBox.ShowMessage("Не удалось завершить операцию.", "Ошибка");
                }

            }

        }

        private void Close()
        {
            if (!File.IsOpen) 
                return;
            if (WinBox.ShowQuestion("Сохранить файл перед закрытием?"))
            {
                Save();
            }
            File.Close();
        }


        private void ShowTaskText()
        {
            WinBox.ShowMessage(
                title: "Задание",
                message: @"WPF приложение редактор данных на JSON

Есть 2 файла, 1 на вход, другой на выход.
Файлы имеют пакеты в формате JSON. (эмуляция работы по сети).
Необходимо написать простую программу, которая позволит отображать данные из входного 
файла (парсить JSON), управлять этими данными (добавление, редактирование, удаление), а в 
конце сохранять данные в выходной файл.

Требования:
  1. Приложение должно быть написано с использованием .NET 4.0.
  2. Для визуализации можно использовать только стандартные WPF компоненты.
  3. Для парсинга JSON необходимо использовать библиотеку Newtonsoft.Json.
  4. Код должен быть читабельным, и если будет необходимо с комментариями.
  5. После сохранения данных в выходной файл, программа также должна без ошибок загружать 
     этот файл, то есть он должен иметь точно такой же формат описания данных",
        okText: "Закрыть"
    );
        }

        private void Save()
        {
            if (!File.IsOpen)
            {
                WinBox.ShowMessage("Нет открытого файла.", "Сохранение не возможно!");
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
            if (File.IsOpen)
            {
                Close();
            }
            File.Create();
            
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