﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace XmlServer
{
    public class MyListener : IDisposable
    {
        TcpListener listener;

        public bool IsWork { get; private set; }

        public MyListener(string ipAdress, int port)
        {
            listener = new TcpListener(IPAddress.Parse(ipAdress), port);
        }

        public async Task<MyContext> GetContextAsync()
        {
            if (!IsWork)
                throw new Exception("Перед 'GetContextAsync' необходимо запустить сервер методом 'Start'");
            
            // получаем входящее подключение
            TcpClient client = await listener.AcceptTcpClientAsync();
            var context = new MyContext(client);
            await context.ProcessAsync();
            return context;
        }

        public void Start()
        {
            IsWork = true;
            listener.Start();
        }

        public void Stop()
        {
            IsWork = false;
            listener.Stop();
        }

        //public HttpListener

        public void Dispose()
        {
            listener.Stop();
        }
    }
}