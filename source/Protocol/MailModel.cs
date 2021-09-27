namespace Protocol
{
    public class MailModel
    {
        public int FormatVersion { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public uint Id { get; set; }
        public string Text { get; set; }
        // 4 byte
        public string Color { get; set; }
        public byte[] Image { get; set; }
    }
}
