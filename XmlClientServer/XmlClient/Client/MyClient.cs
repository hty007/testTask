using Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using WPFStorage.Dialogs;

namespace XmlClient
{
    internal class MyClient : IDisposable
    {
        private string server;
        private int port;
        private TcpClient client;


        public MyClient(string targetServer, int targetPort)
        {
            this.server = targetServer;
            this.port = targetPort;
            client = new TcpClient();
            //client.
        }

        /// <summary>
        /// Подключится
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="SocketException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public void Connect()
        {
            client.Connect(server, port);
        }

        public void Dispose()
        {
            client.Dispose();
        }

        public async Task<List<string>> GetList()
        {
            NetworkStream stream = null;
            using (MyRequest request = new MyRequest())
            {
                request.WriteCommand(ServerCommand.getList);

                stream = client.GetStream();
                await Task.Run(() => request.Stream.WriteTo(stream));
            }

            using (MyResponse response = new MyResponse())
            { 
                await Task.Run(() =>response.GetData(stream));
                stream.Close();

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

            return null;
        }

        public async Task<MailModel> ParseModel(string fileName)
        {
            NetworkStream stream = null;
            using (MyRequest request = new MyRequest())
            {
                request.WriteCommand(ServerCommand.parse);
                request.WriteString(fileName);
                request.WriteFile(fileName);

                stream = client.GetStream();
                await Task.Run(() => request.Stream.WriteTo(stream));
            }

            using (MyResponse response = new MyResponse())
            {
                await Task.Run(() => response.GetData(stream));
                stream.Close();

                ClientCommand command = response.ReadCommand();
                if (command == ClientCommand.model)
                {
                    var model = StreamHelper.StreamToModel(response.Stream, (int)response.Position);

                    return model;
                }
            }

            return null;
        }
    }
}