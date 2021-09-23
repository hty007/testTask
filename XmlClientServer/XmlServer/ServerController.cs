using Protocol;
using System;
using System.IO;
using System.Threading.Tasks;

namespace XmlServer
{
    internal class ServerController
    {
        private static readonly string DATA_DIR = "data";
        private MyListener listener;
        private bool canListen;

        public ServerController()
        {
            Directory.CreateDirectory(DATA_DIR);
        }

        public int Port { get; private set; }

        public void Start()
        {
            canListen = true;
            listener = new MyListener(Port);
            Listing();
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
                _ = Task.Run(() => RequestHandle(context));
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
                    case ServerCommand.parse:
                        ParseHandle(br);
                        break;
                    case ServerCommand.repeat:
                        RepeatHandle(br);
                        break;
                    default:
                        break;
                }
            }
        }

        private void RepeatHandle(BinaryReader br)
        {
            throw new NotImplementedException();
        }

        private void ParseHandle(BinaryReader br)
        {// Принимаем файл
            var fileName = br.ReadString();
            var xml = br.ReadString();
            var model = XMLHelper.GetModel(xml);


            File.WriteAllText(Path.Combine(DATA_DIR, fileName), xml);
        }
    }

    
}