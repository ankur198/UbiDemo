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
        public List<Light> Lights = new List<Light>();

        public bool AddLight(string Nickname, int pin)
        {
            Light light = new Light(Nickname, 100, false, pin, TransitionSpeed);

            if (Lights.Find(x => x.Nickname == light.Nickname) != null)
            {
                Debug.WriteLine("Duplicate name");
                return false;
            }
            else
            {
                Lights.Add(light);
                GpioManager.MakeOutput(light.Pin);
                return true;
            }
        }

        public Light this[int index]
        {
            get { return Lights[0]; }
            private set { /* set the specified index to value here */ }
        }

        public int TransitionSpeed = 50;

    }

    public class Light
    {
        public string Nickname { get; set; }
        public bool State { get; set; }
        public int Pin { get; set; }
        public int TransitionSpeed { get; set; }

        public int Brightness
        {
            get { return _PrefferedBrightness; }
            set
            {
                State = true;
                _PrefferedBrightness = value;
                SetBrightnessTo(_PrefferedBrightness);
            }
        }
        private int _PrefferedBrightness;
        private int _CurrentBrightness = 0;

        public Light(string nickname, int brightness, bool state, int pin, int transitionSpeed)
        {
            Nickname = nickname;
            _PrefferedBrightness = brightness;
            State = state;
            Pin = pin;
            TransitionSpeed = transitionSpeed;
        }

        private async Task SetBrightnessTo(int value)
        {
            if (_CurrentBrightness > value)
            {
                for (int i = _CurrentBrightness; i >= value; i--)
                {
                    GpioManager.SetPwm(Pin, i);
                    await Task.Delay(TransitionSpeed);
                }
            }
            else if (_CurrentBrightness<value)
            {
                for (int i = _CurrentBrightness; i <= value; i++)
                {
                    GpioManager.SetPwm(Pin, i);
                    await Task.Delay(TransitionSpeed);
                }
            }
            _CurrentBrightness = value;
        }

        public async Task TurnOnAsync()
        {
            await SetBrightnessTo(_PrefferedBrightness);
            State = true;
            Debug.WriteLine("light on");
        }
        public async Task TurnOffAsync()
        {
            await SetBrightnessTo(0);
            State = false;
            Debug.WriteLine("light off");
        }


    }

}
