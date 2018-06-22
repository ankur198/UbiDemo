using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeManager
{
    public class LightManager
    {
        private Dictionary<string, int> Lights = new Dictionary<string, int>();

        public bool AddLight(string NickName, int Pin)
        {
            if (Lights.ContainsKey(NickName))
            {
                Debug.WriteLine("Duplicate name");
                return false;
            }
            else
            {
                Lights.Add(NickName, Pin);
                GpioManager.MakeOutput(Pin);
                return true;
            }
        }

        public int TransitionSpeed = 50;
        internal int MaxBrightness = 100;

        private bool GetPin(string Nickname, out int Pin)
        {
            if (Lights.ContainsKey(Nickname))
            {
                Pin = Lights[Nickname];
                return true;
            }
            Pin = 0;
            return false;
        }

        public async Task TurnOnAsync(string NickName)
        {
            int pin;
            if (GetPin(NickName, out pin))
            {
                for (int i = 0; i < MaxBrightness; i++)
                {
                    GpioManager.SetPwm(pin, i);
                    await Task.Delay(TransitionSpeed);
                }
                Debug.WriteLine("light on");
            }
        }

        public async Task TurnOffAsync(string NickName)
        {
            int pin;
            if (GetPin(NickName, out pin))
            {
                Debug.WriteLine("turning light off");
                for (int i = 100; i > -1; i--)
                {
                    GpioManager.SetPwm(pin, i);
                    await Task.Delay(TransitionSpeed);
                    Debug.WriteLine(i.ToString());
                }
                Debug.WriteLine("light off");
            }
        }

        public void SetBrightness(string Nickname, int Brightness)
        {
            int pin;
            if (GetPin(Nickname, out pin))
            {
                GpioManager.SetPwm(pin, Brightness);
            }
        }


    }
}
