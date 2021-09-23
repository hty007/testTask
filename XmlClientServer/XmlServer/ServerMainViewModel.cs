using System;
using WPFStorage.Base;

namespace XmlServer
{
    internal class ServerMainViewModel : ObservableObject
    {
        private bool serverIsWorking;

        public ServerMainViewModel()
        {
            ConfigureServerCommand = new RelayCommand(ConfigureServer);
            StartServerCommand = new RelayCommand(StartServer);
            StopServerCommand = new RelayCommand(StopServer);

            Server = new ServerController();

            string str = "\n";
        }

        public bool ServerIsWorking { get => serverIsWorking; set => SetProperty(ref serverIsWorking, value); }

        public RelayCommand ConfigureServerCommand { get; }
        public RelayCommand StartServerCommand { get; }
        public RelayCommand StopServerCommand { get; }
        public ServerController Server { get; }

        public void Closed(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void StopServer()
        {
            Server.Stop();
            ServerIsWorking = false;
        }

        private void StartServer()
        {
            Server.Start();
            ServerIsWorking = true;
        }

        private void ConfigureServer()
        {
            throw new NotImplementedException();
        }
    }
}