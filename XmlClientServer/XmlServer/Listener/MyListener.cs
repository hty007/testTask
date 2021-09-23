using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace XmlServer
{
    public class MyListener : IDisposable
    {
        //
        // TODO: https://ru.wikipedia.org/wiki/HTTPS
        //

        private int port;
        TcpListener listener;

        public MyListener(int port)
        {
            this.port = port;
            listener = new TcpListener(IPAddress.Any, port);
        }

        public async Task<MyContext> GetContextAsync()
        {
            // получаем входящее подключение
            TcpClient client = await listener.AcceptTcpClientAsync();
            var context = new MyContext(client);
            await context.ProcessAsync();
            return context;
        }

        public void Start() => listener.Start();
        //public void Stop() => listener.Stop();

        //public HttpListener

        public void Dispose()
        {
            listener.Stop();
        }
    }
}