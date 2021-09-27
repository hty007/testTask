using System;
using System.Net;
using WPFStorage.Base;

namespace XmlClient
{
    public class SettingViewModel: ObservableObject
    {
        private int port;
        private string server;
        private bool invalidIp;

        public SettingViewModel()
        {
            ApplyCommand = new RelayCommand(Apply);
        }

        public event Action ClickAppleSetting;

        public int Port { get => port; set => SetProperty(ref port, value); }
        public bool ValidIp { get => invalidIp; set => SetProperty(ref invalidIp, value); }
        public string Ip
        {
            get => server;
            set
            {
                ValidIp = IPAddress.TryParse(value, out IPAddress address);
                SetProperty(ref server, value);
            }
        }

        public RelayCommand ApplyCommand { get; }

        private void Apply()
        {
            ClickAppleSetting?.Invoke();
        }
    }
}