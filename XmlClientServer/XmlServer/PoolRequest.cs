using WPFStorage.Base;

namespace XmlServer
{
    public class PoolRequest : ObservableObject
    {
        private string id;

        public string Id { get => id; set =>SetProperty(ref id, value; }
    }
}