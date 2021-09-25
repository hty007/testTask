using Microsoft.Win32;
using Protocol;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using WPFStorage.Base;
using WPFStorage.Dialogs;

namespace XmlClient
{
    internal class ClientMainViewModel: ObservableObject, IDisposable
    {
        private int targetPort = 5011;
        private string targetServer = "127.0.0.1";
        private MyClient client;
        private bool isConnect;

        public ClientMainViewModel()
        {
            ListServerCommand = new RelayCommand(ListServer);
            ConnectCommand = new RelayCommand(Connect);
            DisconnectCommand = new RelayCommand(Disconnect);
            ParseFileCommand = new RelayCommand(ParseFileAsync);
        }

        public RelayCommand ListServerCommand { get; }
        public RelayCommand ConnectCommand { get; }
        public RelayCommand DisconnectCommand { get; }
        public RelayCommand ParseFileCommand { get; }
        public int TargetPort { get => targetPort; set =>SetProperty(ref targetPort, value); }
        public string TargetServer { get => targetServer; set =>SetProperty(ref targetServer, value); }
        public bool IsConnect { get => isConnect; set => SetProperty(ref isConnect, value); }

        private void Disconnect()
        {
            client.Dispose();
            IsConnect = false;
        }

        private void Connect()
        {
            try
            {
                client = new MyClient(TargetServer, TargetPort);
                client.Connect();
                IsConnect = true;
            }
            catch (SocketException socketEx)
            {
                WinBox.ShowMessage($"Сервер '{TargetServer}:{TargetPort}' не найден.");
            }
            catch (Exception ex)
            {
                WinBox.ShowMessage($"{ex.GetType().Name}: {ex.Message}");
            }
        }

        private async void ParseFileAsync()
        {
            try
            {
                if (!IsConnect)
                {
                    WinBox.ShowMessage("Перед запросом необходимо подключится к серверу!");
                    return;
                }
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.Filter = "XML files (*.xml)|*.txt|All files (*.*)|*.*";

                openFile.CheckFileExists = true;
                openFile.AddExtension = true;
                var res = openFile.ShowDialog();
                if (res == true)
                {
                    MailModel model = await client.ParseModel(openFile.FileName);


                    var viewer = new ViewerModel(model);
                    // Todo: Время полученного сообщения;
                    viewer.OpenDialog();
                }
            }
            catch
            {
                IsConnect = false;
            }
        }

        private async void ListServer()
        {
            try
            {
                if (!IsConnect)
                {
                    WinBox.ShowMessage("Перед запросом необходимо подключится к серверу!");
                    return;
                }
                List<string> files = await client.GetList();
                WinBox.ShowMessage(string.Join('\n', files));
            }
            catch 
            {
                IsConnect = false;
            }
        }

        public void Dispose()
        {
            client?.Dispose();
        }
    }
}