using Protocol;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows.Threading;
using WPFStorage.Base;
using WPFStorage.Dialogs;
using WPFStorage.Settings;

namespace XmlServer
{
    internal class ServerMainViewModel : ObservableObject
    {
        private bool serverIsWorking;
        private int port = 5011;
        private Dispatcher dispatcher;
        private string ipAdress = "127.0.0.1";
        private bool needFilter;
        private string title = "Сервер";


        public ServerMainViewModel()
        {
            StartServerCommand = new RelayCommand(StartServer);
            StopServerCommand = new RelayCommand(StopServer);
            CreateCommand = new RelayCommand(Create);
            EditFileCommand = new RelayCommand<string>(EditFile);
            ClearHistoryCommand = new RelayCommand(ClearHistory);
            AboutCommand = new RelayCommand(About);

            Server = new ServerController();
            Server.SetAdress(IPAdress, Port);
            Server.ClientRequest += OnRequest;
            Server.ExeptionCallback += OnExeption;

            var names = Server.GetFileNames();
            Files = new ObservableCollection<string>(names);
            dispatcher = Dispatcher.CurrentDispatcher;
            Settings = new SettingViewModel()
            {
                Ip = IPAdress,
                Port = Port,
            };
            Settings.ClickAppleSetting += Settings_ClickAppleSetting;
        }

        public ObservableCollection<PoolRequest> Requests { get; set; } = new ObservableCollection<PoolRequest>();
        public ObservableCollection<string> Files { get; set; }
        public SettingViewModel Settings { get; set; }

        public int Port { get => port; set => SetProperty(ref port, value); }
        public string IPAdress { get => ipAdress; set => SetProperty(ref ipAdress, value); }
        public string Title { get => title; set => SetProperty(ref title, value); }
        public bool ServerIsWorking { get => serverIsWorking; set => SetProperty(ref serverIsWorking, value); }

        public bool NeedFilter { get => needFilter; set => SetProperty(ref needFilter, value); }

        public RelayCommand StartServerCommand { get; }
        public RelayCommand StopServerCommand { get; }
        public RelayCommand CreateCommand { get; }
        public RelayCommand<string> EditFileCommand { get; }
        public RelayCommand ClearHistoryCommand { get; }
        public RelayCommand AboutCommand { get; }
        public ServerController Server { get; }

        public void Closed(object sender, EventArgs e)
        {
            if (ServerIsWorking)
            {
                Server.Stop();
            }
        }

        private void About()
        {
            WinBox.ShowMessage(@"Тестовое задание. 
Сервер на TCP. Парсинг XML файлов.");
        }

        private void ClearHistory()
        {
            Requests.Clear();
        }

        private void EditFile(string fileName)
        {
            var dir = ServerController.DATA_DIR;
            var fileInFolder = Path.Combine(dir, fileName);
            MailModel model = ModelFileHelper.Load(fileInFolder);
            EditorModel editor = new EditorModel(model);
            editor.FileName = fileName;

            var res = editor.OpenDialog();

            if (res == true)
            {
                // TODO: Если польхователь введёт имя которое уже есть в папке?
                ModelFileHelper.Save(editor.GetModel(), Path.Combine(dir, editor.FileName));
                if (fileName != editor.FileName)
                {
                    Files.Remove(fileName);
                    Files.Add(editor.FileName);
                }
            }
        }

        private void Settings_ClickAppleSetting()
        {
            if (!Settings.ValidIp)
            {
                WinBox.ShowMessage("Поле IP заполнено c ошибкой!");
                return;
            }
            bool canStart = false;
            if (Server.IsWork)
            {
                Server.Stop();
                canStart = true;
            }
            IPAdress = Settings.Ip;
            Port = Settings.Port;
            Server.SetAdress(IPAdress, Port);
            if (canStart)
                Server.Start();
        }


        private void Create()
        {
            // Todo: Вынести в отдельный контроллер
            EditorModel editor = new EditorModel(new MailModel()
            {
                Color = "#FFFFFF00",
                Image = Properties.Resources.baikal,
            });

            var dir = ServerController.DATA_DIR;
            string fileName = "Model.xml";
            int index = 0;
            while (File.Exists(Path.Combine(dir, fileName)))
            {
                fileName = $"Model{++index}.xml";
            }
            editor.FileName = fileName;

            var res = editor.OpenDialog();

            if (res == true)
            {
                // TODO: Если польхователь введёт имя которое уже есть в папке?
                ModelFileHelper.Save(editor.GetModel(), Path.Combine(dir, editor.FileName));
                Files.Add(editor.FileName);
            }
        }

        private void OnExeption(Exception obj)
        {
            var pool = new PoolRequest();
            pool.Message = $"{obj.GetType().Name}: {obj.Message}";

            dispatcher.Invoke(() => Requests.Add(pool));
        }

        private void OnRequest(MyContext context)
        {
            if (NeedFilter && context.Command == ServerCommand.hello)
            {
                return;
            }
            var request = new PoolRequest();
            request.Id = context.LocalEndPoint.ToString();
            request.Command = context.Command;
            dispatcher.Invoke(() => Requests.Add(request));
        }

        private void StopServer()
        {
            Server.Stop();
            Title = $"Сервер остановлен";
            ServerIsWorking = false;
        }

        private void StartServer()
        {
            Server.Start();
            Title = $"Сервер {IPAdress}:{Port} Запущен";
            ServerIsWorking = true;
        }
    }
}