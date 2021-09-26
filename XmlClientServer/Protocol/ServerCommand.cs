/*
Тело пакета:

parse:
[0(4байта)][имя файла][тело файла]
repeat:
[0(4байта)][имя файла]
response:
[0(4байта)][FormatVersion][Id][To][From][Text][countColor][Color][CountImage][image]


*/

using System;

namespace Protocol
{
    public enum  ServerCommand
    {
        hello,
        parse,
        repeat,
        generate,
        getList,
    }

    public enum ClientCommand
    {
        hello,
        list,
        xml,
        model,
        fail,
    }
}
