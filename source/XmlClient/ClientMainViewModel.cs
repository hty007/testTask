using Microsoft.Win32;
using Protocol;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using WPFStorage.Base;
using WPFStorage.Dialogs;

namespace XmlClient
{
    internal class ClientMainViewModel : ObservableObject, IDisposable
    {
        private int targetPort = 5011;
        private string targetServer = "127.0.0.1";
        private MyClient client;
        private bool isConnect;
        private int indexTab;
        private string title = "Клиент";

        public ClientMainViewModel()
        {
            ListServerCommand = new RelayCommand(ListServer);
            ConnectCommand = new RelayCommand(Connect);
            DisconnectCommand = new RelayCommand(Disconnect);
            ParseFileCommand = new RelayCommand(ParseFileAsync);
            RepeatedCommand = new RelayCommand<string>(RepeatedAsync);
            ClearHistoryCommand = new RelayCommand(ClearHistory);
            AboutCommand = new RelayCommand(About);
            Viewer = new ViewerViewModel();
            Setting = new SettingViewModel();
            Setting.Ip = TargetServer;
            Setting.Port = TargetPort;

            Setting.ClickAppleSetting += OnClickAppleSetting;
            client = new MyClient(TargetServer, TargetPort);
            client.IsConnectedChanged += Client_IsConnectedChanged;
        }

        public ObservableCollection<string> History { get; private set; } = new ObservableCollection<string>();
        public RelayCommand ListServerCommand { get; }
        public RelayCommand ConnectCommand { get; }
        public RelayCommand DisconnectCommand { get; }
        public RelayCommand ParseFileCommand { get; }
        public RelayCommand<string> RepeatedCommand { get; }
        public RelayCommand ClearHistoryCommand { get; }
        public object AboutCommand { get; }
        public ViewerViewModel Viewer { get; }
        public SettingViewModel Setting { get; }
        public int TargetPort { get => targetPort; set => SetProperty(ref targetPort, value); }
        public int IndexTab { get => indexTab; set => SetProperty(ref indexTab, value); }

        public string TargetServer { get => targetServer; set => SetProperty(ref targetServer, value); }
        public string Title { get => title; set => SetProperty(ref title, value); }
        public bool IsConnect { get => isConnect; set => SetProperty(ref isConnect, value); }

        private void About()
        {
            WinBox.ShowMessage(@"Тестовое задание. 
Клиент на TCP. Отправка XML файлов на сервер для парсинга 
и отображение результатов.");
        }
        

        private void Client_IsConnectedChanged(bool isConnect)
        {
            IsConnect = isConnect;
            Title = IsConnect ? $"Клиент {TargetServer}:{TargetPort}" : "Клиент";
        }

        private void Disconnect()
        {
            client.Disconnect();
        }

        private void Connect()
        {
            try
            {
                client.Connect();
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

        private void ClearHistory() => History.Clear();

        private async void RepeatedAsync(string fileName)
        {
            try
            {
                if (!IsConnect)
                {
                    WinBox.ShowMessage("Перед запросом необходимо подключится к серверу!");
                    return;
                }
                MailModel model = await client.RepeatedModel(fileName);


                Viewer.SetModel(model);
                Viewer.Time = DateTime.Now;
                IndexTab = 1;
            }
            catch (Exception ex)
            {
                IsConnect = false;
            }
        }

        private void OnClickAppleSetting()
        {
            if (!Setting.ValidIp)
            {
                WinBox.ShowMessage("Применение настроек отменено из-за ошибки ввода целевого Ip");
                return;
            }
            bool needConnect = false;
            if (isConnect)
            {
                Disconnect();
                needConnect = true;
            }
            client.Ip = TargetServer = Setting.Ip;
            client.Port = TargetPort = Setting.Port;

            if (needConnect)
                Connect();

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
                openFile.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";

                openFile.CheckFileExists = true;
                openFile.AddExtension = true;
                var res = openFile.ShowDialog();
                if (res == true)
                {
                    MailModel model = await client.ParseModel(openFile.FileName);


                    Viewer.SetModel(model);
                    Viewer.Time = DateTime.Now;
                    IndexTab = 1;

                    History.Add(Path.GetFileName(openFile.FileName));
                }
            }
            catch (Exception ex)
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