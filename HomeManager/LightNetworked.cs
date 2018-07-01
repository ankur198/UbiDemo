using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HomeManager
{
    class LightNetworked : ILight
    {
        public LightNetworked(string nickname, int brightness, bool state, int pin, Uri iP)
        {
            Nickname = nickname;
            _Brightness = brightness;
            _State = state;
            Pin = pin;
            IP = iP;
        }

        public int Brightness
        {
            get
            {
                return _Brightness;
            }
            set
            {
                //_State = true;
                _Brightness = value;
                if (_State) TurnOnAsync();

            }
        }
        private int _Brightness = 0;

        public Uri IP { get; set; }
        public string Nickname { get; set; }
        public int Pin { get; set; }

        public bool State
        {
            get
            {
                return _State;
            }
            set
            {
                if (value != _State)
                {
                    if (value == true)
                    {
                        TurnOnAsync();
                    }
                    else
                    {
                        TurnOffAsync();
                    }
                }
            }
        }
        private bool _State = false;

        public async Task TurnOffAsync()
        {
            _State = false;

            var msg = $"/1/6027/setpin/{Pin}/0";
            await SendMessageToDevice(msg);

        }

        public async Task TurnOnAsync()
        {
            _State = true;
            int b = _Brightness * 10;
            var msg = $"/1/6027/setpin/{Pin}/{b}";
            await SendMessageToDevice(msg);

        }

        private async Task<string> SendMessageToDevice(string message)
        {
            Debug.WriteLine(message);
            HttpClient client = new HttpClient();
            client.BaseAddress = IP;
            var x = await client.GetAsync(message);
            Debug.WriteLine(x);
            return x.ToString();
        }
    }
}
