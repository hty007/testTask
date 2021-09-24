using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace XmlServer
{
    public class MyContext : IDisposable
    {
        private TcpClient client;

        public MyContext(TcpClient client)
        {
            this.client = client;
        }

        public MemoryStream Data { get; private set; }
        public AddressFamily AddressFamily { get => client.Client.AddressFamily; }
        public EndPoint RemoteEndPoint { get => client.Client.RemoteEndPoint; }
        public EndPoint LocalEndPoint { get => client.Client.LocalEndPoint; }

        public async Task ProcessAsync()
        {
            var stream = client.GetStream();
            // Получаем все данные из потока 
            Data = await GetAll(stream);
        }

        private async Task<MemoryStream> GetAll(NetworkStream stream)
        {
            MemoryStream memory = new MemoryStream();
            int count = 0;
            do
            {
                var buf = new byte[256];
                count = stream.Read(buf, 0, buf.Length);
                await memory.WriteAsync(buf, 0, count);
            }
            while (stream.DataAvailable);
            memory.Position = 0;
            return memory;
        }

        internal void Send(MemoryStream ms)
        {
            var ns = client.GetStream();
            ms.WriteTo(ns);
            ns.Flush();
        }

        public void Dispose()
        {
            Data.Dispose();
            client.Dispose();
        }
    }
}