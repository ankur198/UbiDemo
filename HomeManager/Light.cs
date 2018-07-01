using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HomeManager
{
    public class Light : ILight
    {
        public string Nickname { get; set; }
        public int Pin { get; set; }
        public int TransitionSpeed { get; set; }

        public bool State
        {
            get
            {
                return _State;
            }
            set
            {
                _State = value;
                Debug.WriteLine("State Changed");
                if (_State == false)
                {
                    TurnOffAsync();
                }
                else TurnOnAsync();

            }
        }

        public int Brightness
        {
            get { return _PrefferedBrightness; }
            set
            {
                _State = true;
                _PrefferedBrightness = value;
                SetBrightnessTo(_PrefferedBrightness);
            }
        }

        private int _PrefferedBrightness { get; set; }
        private int _CurrentBrightness { get; set; }
        public Uri IP { get; set; }

        private bool _State;

        public Light(string nickname, int brightness, bool state, int pin, int transitionSpeed)
        {
            Nickname = nickname;
            _PrefferedBrightness = brightness;
            State = state;
            Pin = pin;
            TransitionSpeed = transitionSpeed;
            GpioManager.MakeOutput(Pin);
            IP = new Uri("http://127.0.0.0/");
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
            else if (_CurrentBrightness < value)
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
            _State = true;
            Debug.WriteLine("light on");
        }
        public async Task TurnOffAsync()
        {
            await SetBrightnessTo(0);
            _State = false;
            Debug.WriteLine("light off");
        }
    }

}
