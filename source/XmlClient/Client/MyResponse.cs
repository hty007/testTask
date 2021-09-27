using Protocol;
using System;
using System.IO;
using System.Net.Sockets;

namespace XmlClient
{
    internal class MyResponse : IDisposable
    {
        private MemoryStream memory;
        private BinaryWriter writer;
        private BinaryReader reader;

        public MyResponse()
        {
            memory = new MemoryStream();
            writer = new BinaryWriter(memory);
            reader = new BinaryReader(memory);
            
        }

        public MemoryStream Stream => memory;
        public BinaryReader Reader => reader;

        public long Position => Stream.Position;

        public void Dispose()
        {
            memory.Dispose();
            writer.Dispose();
            reader.Dispose();
        }



        internal ClientCommand ReadCommand()
        {
            memory.Position = 0;
            return (ClientCommand)reader.ReadInt32();
        }

        public void GetData(NetworkStream stream)
        {
            byte[] data = new byte[256];
            do
            {
                int bytes = stream.Read(data, 0, data.Length);
                writer.Write(data,0, bytes);
            }
            while (stream.DataAvailable); // пока данные есть в потоке
        }
    }
}