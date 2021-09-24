using Protocol;
using System;
using System.IO;

namespace XmlClient
{
    internal class MyRequest : IDisposable
    {
        private MemoryStream memory;
        private BinaryWriter writer;

        public MyRequest()
        {
            memory = new MemoryStream();
            writer = new BinaryWriter(memory);
        }

        public MemoryStream Stream => memory;

        public void Dispose()
        {
            memory.Dispose();
            writer.Dispose();
        }

        internal void WriteCommand(ServerCommand getList)
        {
            writer.Write((int)getList);
        }
    }
}