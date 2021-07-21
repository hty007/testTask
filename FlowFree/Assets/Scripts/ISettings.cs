using UnityEngine;

namespace FlowFree
{
    public interface ISettings
    {
        bool SelectLavel { get; set; }

        Color GetColor(int number);
    }
}