using Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace XmlServer
{
    internal class ServerController
    {
        private static readonly string DATA_DIR = "data";
        private MyListener listener;
        private bool canListen;
        private List<string> files = new List<string>() { "renat", "lila"};

        public ServerController()
        {
            Directory.CreateDirectory(DATA_DIR);
        }

        public int Port { get; private set; }

        public event Action<MyContext> ClientRequest;

        public void Start()
        {
            canListen = true;
            listener = new MyListener(Port);
            listener.Start();
            Listing();
        }

        public bool SetPort(int port)
        {
            if (listener == null)
            {
                Port = port;
                return true;
            }
            else if (!listener.IsWork)
            {
                listener.Dispose();
                listener = new MyListener(Port);
                return true;
            }

            return false;
        }

        public void Stop()
        {
            canListen = false;
            listener.Dispose();
        }

        private async void Listing()
        {
            while (canListen)
            {
                try
                {
                    MyContext context = await listener.GetContextAsync();
                    _ = Task.Run(() => RequestHandle(context));
                }
                catch (Exception ex)
                { 
                    // TODO: Реализовать логгирование
                }
            }
        }

        /// <summary>
        /// Формируем ответ и отвечаем
        /// </summary>
        /// <param name="context"></param>
        private void RequestHandle(MyContext context)
        {
            using (BinaryReader br = new BinaryReader(context.Data))
            {
                ServerCommand command = (ServerCommand)br.ReadInt32();
                switch (command)
                {
                    case ServerCommand.generate:
                        GenerateHandle(br, context);
                        break;
                    case ServerCommand.parse:
                        ParseHandle(br, context);
                        break;
                    case ServerCommand.repeat:
                        RepeatHandle(br);
                        break;
                    default:
                        ListHandle(context);
                        break;
                }
            }
            context.Dispose();
        }

        private void RepeatHandle(BinaryReader br)
        {
            throw new NotImplementedException();
        }

        private void ListHandle(MyContext context)
        {
            using (MemoryStream response = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(response))
                {
                    bw.Write((int)ClientCommand.list);
                    bw.Write(files.Count);
                    foreach (var fileName in files)
                    {
                        bw.Write(fileName);
                    }

                    context.Send(response);
                }
            }
        }

        private void GenerateHandle(BinaryReader br, MyContext context)
        {
            // Генерируем файл
            ProtocolModel model = StreamHelper.StreamToModel(context.Data);
            var xml = XMLHelper.GetXML(model);

            using (MemoryStream response = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(response))
                {
                    bw.Write((int)ClientCommand.xml);
                    bw.Write(xml);
                }

                context.Send(response);
            }
        }

        private void ParseHandle(BinaryReader br, MyContext context)
        {// Принимаем файл
            var fileName = br.ReadString();
            var xml = br.ReadString();
            var model = XMLHelper.GetModel(xml);

            using (MemoryStream response = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(response))
                    bw.Write((int)ClientCommand.model);
                using (MemoryStream modelStream = StreamHelper.ModelToStream(model))
                    modelStream.WriteTo(response);

                context.Send(response);
            }
            File.WriteAllText(Path.Combine(DATA_DIR, fileName), xml);
            files.Add(fileName);
        }
    }
}