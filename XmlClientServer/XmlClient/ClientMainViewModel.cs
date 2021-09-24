using System;
using System.Collections.Generic;
using WPFStorage.Base;
using WPFStorage.Dialogs;

namespace XmlClient
{
    internal class ClientMainViewModel: ObservableObject
    {
        private int targetPort = 5011;
        private string targetServer = "127.0.0.1";
        private MyClient client;

        public ClientMainViewModel()
        {
            ListServerCommand = new RelayCommand(ListServer);
            client = new MyClient(TargetServer, targetPort);
        }

        public RelayCommand ListServerCommand { get; }


        public int TargetPort { get => targetPort; set =>SetProperty(ref targetPort, value); }
        public string TargetServer { get => targetServer; set =>SetProperty(ref targetServer, value); }

        private async void ListServer()
        {
            client.Connect();
            List<string> files = await client.GetList();

            WinBox.ShowMessage(string.Join('\n', files));
        }
    }
}