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

        public void WriteCommand(ServerCommand cmd)
        {
            writer.Write((int)cmd);
        }

        public void WriteString(string fileName)
        {
            writer.Write(fileName);
        }

        internal void WriteFile(string fileName)
        {
            var content = File.ReadAllText(fileName);
            writer.Write(content);
        }
    }
}