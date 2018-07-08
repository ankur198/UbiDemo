using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace HomeManager
{
    public interface ILight : INotifyPropertyChanged
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