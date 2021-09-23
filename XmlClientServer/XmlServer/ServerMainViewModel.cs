using System;
using WPFStorage.Base;

namespace XmlServer
{
    internal class ServerMainViewModel: ObservableObject
    {


        public ServerMainViewModel()
        {
            ConfigureServerCommand = new RelayCommand(ConfigureServer);

            //MainServer.Start();
        }

        public RelayCommand ConfigureServerCommand { get; }

        public void Closed(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ConfigureServer()
        {
            throw new NotImplementedException();
        }


        private object RelayCommand(object configureServer)
        {
            throw new NotImplementedException();
        }
    }
}