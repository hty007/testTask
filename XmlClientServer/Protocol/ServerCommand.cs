/*
Тело пакета:

parse:
[0(4байта)][имя файла][тело файла]
repeat:
[0(4байта)][имя файла]
response:
[0(4байта)][key][value]... [key][value]


*/

using System;

namespace Protocol
{
    public enum  ServerCommand
    {
        parse,
        repeat,
        response,
    }
}
