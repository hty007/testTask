using Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace XmlClient
{
    internal class MyClient : IDisposable
    {
        private int port;
        private DispatcherTimer timer;
        private int updateTime = 100;
        private bool isConnect;
        private string ip;

        public MyClient(string targetServer, int targetPort)
        {
            this.Ip = targetServer;
            this.Port = targetPort;

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, UpdateTime);
            timer.Tick += CheckConnect;
        }

        public event Action<bool> IsConnectedChanged;

        public int UpdateTime
        {
            get => updateTime;
            set
            {
                if (updateTime != value)
                {
                    UpdateInterval(value);
                    updateTime = value;
                }
            }
        }

        public bool IsConnect
        {
            get => isConnect;
            private set
            {
                isConnect = value;
                IsConnectedChanged?.Invoke(value);
            }
        }

        public string Ip { get => ip; internal set => ip = value; }
        public int Port { get => port; internal set => port = value; }

        /// <summary>
        /// Подключится
        /// </summary>
        public void Connect()
        {
            timer.Start();
        }

        public void Disconnect()
        {
            timer.Stop();
            IsConnect = false;
        }

        public void Dispose() => Disconnect();


        private void UpdateInterval(int milliseconds)
        {
            timer.Stop();
            timer.Interval = new TimeSpan(0, 0, 0, 0, milliseconds);
            timer.Start();
        }

        public async Task<List<string>> GetList()
        {
            using (TcpClient client = new TcpClient())
            {
                client.SendTimeout = UpdateTime;
                client.ReceiveTimeout = UpdateTime;
                try
                {
                    using (MyRequest request = new MyRequest())
                    {
                        request.WriteCommand(ServerCommand.getList);

                        var stream = client.GetStream();
                        await Task.Run(() => request.Stream.WriteTo(stream));
                    }

                    using (MyResponse response = new MyResponse())
                    {
                        var stream = client.GetStream();
                        await Task.Run(() => response.GetData(stream));

                        ClientCommand command = response.ReadCommand();
                        if (command == ClientCommand.list)
                        {
                            var reader = response.Reader;
                            int count = reader.ReadInt32();

                            var list = new List<string>();
                            for (int i = 0; i < count; i++)
                            {
                                list.Add(reader.ReadString());
                            }

                            return list;
                        }
                    }

                }
                catch (Exception ex)
                {
                    IsConnect = false;
                }
                return null;
            }
        }

        public async Task<MailModel> ParseModel(string fileName)
        {
            using (TcpClient client = new TcpClient())
            {
                client.SendTimeout = UpdateTime;
                client.ReceiveTimeout = UpdateTime;
                try
                {
                    using (MyRequest request = new MyRequest())
                    {
                        request.WriteCommand(ServerCommand.parse);
                        request.WriteString(Path.GetFileName(fileName));
                        request.WriteFile(fileName);

                        var stream = client.GetStream();
                        await Task.Run(() => request.Stream.WriteTo(stream));
                        stream.Flush();
                    }

                    using (MyResponse response = new MyResponse())
                    {
                        var stream = client.GetStream();
                        await Task.Run(() => response.GetData(stream));

                        ClientCommand command = response.ReadCommand();
                        if (command == ClientCommand.model)
                        {
                            var model = StreamHelper.StreamToModel(response.Stream, (int)response.Position);

                            return model;
                        }
                    }

                }
                catch (Exception ex)
                {
                    IsConnect = false;
                }
                return null;
            }
        }

        private async void CheckConnect(object sender, EventArgs e)
        {
            timer.Stop();
            using (TcpClient client = new TcpClient())
            {
                client.SendTimeout = UpdateTime;
                client.ReceiveTimeout = UpdateTime;
                try
                {
                    await client.ConnectAsync(Ip, Port);
                    using (MyRequest request = new MyRequest())
                    {
                        request.WriteCommand(ServerCommand.hello);

                        var stream = client.GetStream();
                        await Task.Run(() => request.Stream.WriteTo(stream));
                    }

                    using (MyResponse response = new MyResponse())
                    {
                        await Task.Run(() => response.GetData(client.GetStream()));

                        ClientCommand command = response.ReadCommand();
                        if (command == ClientCommand.hello)
                        {
                            IsConnect = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    IsConnect = false;
                }
            }
            timer.Start();
        }
    }
}