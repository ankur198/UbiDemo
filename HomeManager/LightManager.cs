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

        public int TransitionSpeed = 5;

        internal async void MessageRecieved(string e)
        {
            var d = e.Split(',');
            var nickname = d[0];

            if (Lights.Count(x => x.Nickname == nickname) == 1)
            {
                Light light = Lights.Find(x => x.Nickname == nickname);
                var state = d[1] == "1" ? true : false;
                var brightness = int.Parse(d[2]);

                if (brightness != light.Brightness)
                {
                    light.Brightness = brightness;
                    return;
                }
                else if (state != light.State)
                {
                    if (state == true)
                    {
                        await light.TurnOnAsync();
                    }
                    else
                    {
                        await light.TurnOffAsync();
                    }
                }
            }
        }
    }

    public class Light
    {
        public string Nickname { get; set; }
        public bool State { get; set; }
        public int Pin { get; set; }
        public int TransitionSpeed { get; set; }

        public Light(string nickname, int brightness, bool state, int pin, int transitionSpeed)
        {
            Nickname = nickname;
            _Brightness = brightness;
            State = state;
            Pin = pin;
            TransitionSpeed = transitionSpeed;
        }

        public async Task TurnOnAsync()
        {
            for (int i = 0; i <= _Brightness; i++)
            {
                GpioManager.SetPwm(Pin, i);
                await Task.Delay(TransitionSpeed);
            }
            State = true;
            Debug.WriteLine("light on");
        }
        public async Task TurnOffAsync()
        {
            Debug.WriteLine("turning light off");
            for (int i = _Brightness; i > -1; i--)
            {
                GpioManager.SetPwm(Pin, i);
                await Task.Delay(TransitionSpeed);
                Debug.WriteLine(i.ToString());
            }
            State = false;
            Debug.WriteLine("light off");
        }
        private int _Brightness;

        public int Brightness
        {
            get { return _Brightness; }
            set
            {
                GpioManager.SetPwm(Pin, value);
                State = true;
                _Brightness = value;
            }
        }
    }

}
