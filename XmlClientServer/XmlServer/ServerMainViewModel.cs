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

        public ServerMainViewModel()
        {
            ConfigureServerCommand = new RelayCommand(ConfigureServer);
            StartServerCommand = new RelayCommand(StartServer);
            StopServerCommand = new RelayCommand(StopServer);
            CreateCommand = new RelayCommand(Create);
            EditFileCommand = new RelayCommand<string>(EditFile);

            Server = new ServerController();
            Server.ClientRequest += OnRequest;

            var names = Server.GetFileNames();
            Files = new ObservableCollection<string>(names);
        }

        public ObservableCollection<PoolRequest> Requests { get; set; } = new ObservableCollection<PoolRequest>();
        public ObservableCollection<string> Files { get; set; }

        public int Port { get => port; set => SetProperty(ref port, value); }
        public bool ServerIsWorking { get => serverIsWorking; set => SetProperty(ref serverIsWorking, value); }

        public RelayCommand ConfigureServerCommand { get; }
        public RelayCommand StartServerCommand { get; }
        public RelayCommand StopServerCommand { get; }
        public RelayCommand CreateCommand { get; }
        public RelayCommand<string> EditFileCommand { get; }
        public ServerController Server { get; }

        public void Closed(object sender, EventArgs e)
        {
            if (ServerIsWorking)
            {
                Server.Stop();
            }
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


        private void Create()
        {
            // Todo: Вынести в отдельный контроллер
            EditorModel editor = new EditorModel(new MailModel()
            {
                Color = "#FFFFFF00",
                Image = Properties.Resources.baikal,
            }) ;

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

        private void OnRequest(MyContext context)
        {
            var request = new PoolRequest();
            request.Id = context.LocalEndPoint.ToString();
            //StringBuilder message = new StringBuilder();
            //message.AppendLine($"LocalEndPoint: {context.LocalEndPoint}");
            //message.AppendLine($"RemoteEndPoint: {context.RemoteEndPoint}");
            //message.AppendLine($"AddressFamily: {context.AddressFamily}");
            //Dispatcher.CurrentDispatcher.Invoke(() => WinBox.ShowMessage(message.ToString()));
;
            Requests.Add(request);
        }

        private void StopServer()
        {
            Server.Stop();
            ServerIsWorking = false;
        }

        private void StartServer()
        {
            Server.SetPort(Port);
            Server.Start();
            ServerIsWorking = true;
        }

        private void ConfigureServer()
        {
            var settings = new Settings();
            settings.Title = "Настройка сервера";

            settings.AddUpDownIterator("port", "Post")
                .SetInkermenter(1)
                .SetIsInteger(true)
                .SetMinValue(1)
                .SetMaxValue(10000);
        }
    }
}