﻿using Protocol;
using WPFStorage.Base;

namespace XmlServer
{
    public class PoolRequest : ObservableObject
    {
        private string id;
        private ServerCommand command;

        public string Id { get => id; set => SetProperty(ref id, value); }
        public ServerCommand Command { get => command; internal set => SetProperty(ref command, value); }
    }
}