using Protocol;
using System;
using System.Collections.ObjectModel;
using WPFStorage.Base;
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

            Server = new ServerController();
            Server.ClientRequest += OnRequest;
        }

        public ObservableCollection<PoolRequest> Requests { get; set; } = new ObservableCollection<PoolRequest>();

        public int Port { get => port; set => SetProperty(ref port, value); }
        public bool ServerIsWorking { get => serverIsWorking; set => SetProperty(ref serverIsWorking, value); }

        public RelayCommand ConfigureServerCommand { get; }
        public RelayCommand StartServerCommand { get; }
        public RelayCommand StopServerCommand { get; }
        public RelayCommand CreateCommand { get; }
        public ServerController Server { get; }

        public void Closed(object sender, EventArgs e)
        {
            if (ServerIsWorking)
            {
                Server.Stop();

            }
        }

        private void Create()
        {
            EditorModel editor = new EditorModel(new ProtocolModel());

            var res = editor.OpenDialog();


            ProtocolModel model = editor.GetModel();


        }

        private void OnRequest(MyContext context)
        {
            var request = new PoolRequest();
            request.Id = context.LocalEndPoint.ToString();
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