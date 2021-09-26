using Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace XmlServer
{
    internal class ServerController
    {
        public static readonly string DATA_DIR = "data";
        private MyListener listener;
        private bool canListen;
        private List<string> files;

        public ServerController()
        {
            Directory.CreateDirectory(DATA_DIR);
            files = new List<string>(GetFileNames());
        }

        public int Port { get; private set; }

        public event Action<MyContext> ClientRequest;
        public event Action<Exception> ExeptionRequest;

        private static void SendFail(MyContext context)
        {
            using (MemoryStream response = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(response))
                    bw.Write((int)ClientCommand.fail);

                context.Send(response);
            }
        }

        private static void SendModel(MyContext context, MailModel model)
        {
            using (MemoryStream response = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(response))
                {
                    using (MemoryStream modelStream = StreamHelper.ModelToStream(model))
                    {
                        bw.Write((int)ClientCommand.model);
                        modelStream.WriteTo(response);

                        context.Send(response);
                    }
                }
            }
        }

        public void Start()
        {
            canListen = true;
            listener = new MyListener(Port);
            listener.Start();
            _ = Task.Run(Listing);
        }

        public IEnumerable<string> GetFileNames()
        {
            var dataDir = new DirectoryInfo(DATA_DIR);
            return dataDir.GetFiles().Select(f => f.Name);
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
                MyContext context = await listener.GetContextAsync();
                _ = Task.Run(() =>
                {
                    try
                    {
                        switch (context.Command)
                        {
                                //case ServerCommand.generate:
                                //    GenerateHandle(br, context);
                                //    break;
                            case ServerCommand.hello:
                                CheckConnect(context);
                                break;
                            case ServerCommand.parse:
                                ParseHandle(context);
                                break;
                            case ServerCommand.repeat:
                                RepeatHandle(context);
                                break;
                            case ServerCommand.getList:
                            default:
                                ListHandle(context);
                                break;
                        }
                        ClientRequest?.Invoke(context);
                    }
                    catch (Exception ex)
                    {
                        ExeptionRequest?.Invoke(ex);
                    }
                    finally
                    {
                        context.Dispose();
                    }
                });
            }
        }

        private void CheckConnect(MyContext context)
        {
            using (MemoryStream response = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(response))
                {
                    bw.Write((int)ClientCommand.hello);
                    bw.Flush();
                    context.Send(response);
                }
            }
        }

        private void RepeatHandle(MyContext context)
        {
            // Принимаем файл
            var br = context.GetReader();

            var fileName = br.ReadString();
            if (!files.Contains(fileName))
            {
                SendFail(context);
                return;
            }

            var xml = File.ReadAllText(Path.Combine(DATA_DIR, fileName));
            var model = XMLHelper.GetModel(xml);
            SendModel(context, model);
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
            MailModel model = StreamHelper.StreamToModel(context.Data);
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

        private void ParseHandle(MyContext context)
        {// Принимаем файл
            var br = context.GetReader();
            var fileName = br.ReadString();
            var xml = br.ReadString();
            var model = XMLHelper.GetModel(xml);

            SendModel(context, model);

            File.WriteAllText(Path.Combine(DATA_DIR, fileName), xml);
            files.Add(fileName);
        }
    }
}