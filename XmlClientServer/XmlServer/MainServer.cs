using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace XmlServer
{
    internal class MainServer
    {
        public static int Port { get; private set; } = 8888;
        public static string Address { get; private set; } = "127.0.0.1";

        internal static void Start()
        {
            // sourse https://metanit.com/sharp/net/4.2.php
            // https://metanit.com/sharp/net/4.3.php

            TcpListener server = null;
            try
            {
                IPAddress localAddr = IPAddress.Parse(Address);
                server = new TcpListener(localAddr, Port);

                // запуск слушателя
                server.Start();

                while (true)
                {
                    //Console.WriteLine("Ожидание подключений... ");

                    // получаем входящее подключение
                    TcpClient client = server.AcceptTcpClient();
                    //Console.WriteLine("Подключен клиент. Выполнение запроса...");

                    // получаем сетевой поток для чтения и записи
                    NetworkStream stream = client.GetStream();

                    // сообщение для отправки клиенту
                    string response = "Привет мир";
                    // преобразуем сообщение в массив байтов
                    byte[] data = Encoding.UTF8.GetBytes(response);

                    // отправка сообщения
                    stream.Write(data, 0, data.Length);
                    //Console.WriteLine("Отправлено сообщение: {0}", response);
                    // закрываем поток
                    stream.Close();
                    // закрываем подключение
                    client.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (server != null)
                    server.Stop();
            }
        }
    }
}