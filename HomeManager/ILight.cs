using System;
using System.Threading.Tasks;

namespace HomeManager
{
    public interface ILight
    {
        string Nickname { get; set; }
        int Pin { get; set; }
        bool State { get; set; }
        int Brightness { get; set; }
        Uri IP { get; set; }

        Task TurnOnAsync();
        Task TurnOffAsync();
    }
}