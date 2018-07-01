using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace HomeManager
{
    public class LightManager
    {
        public ObservableCollection<ILight> Lights = new ObservableCollection<ILight>();

        public bool AddLight(string Nickname, int pin)
        {
            Light light = new Light(Nickname, 100, false, pin, TransitionSpeed);


            if (Lights.ToList().Find(x => x.Nickname == light.Nickname || x.Pin == pin) != null)
            {
                Debug.WriteLine("Duplicate name or pin");
                return false;
            }
            else
            {
                Lights.Add(light);
                return true;
            }
        }
        public LightManager(string nickname)
        {
            Nickname = nickname;
        }

        public bool AddLight(string Nickname, int pin, Uri uri)
        {
            LightNetworked light = new LightNetworked(Nickname, 1023, false, pin, uri);

            if (Lights.ToList().Find(x => x.Nickname == light.Nickname || (x.Pin == light.Pin && x.IP == light.IP)) != null)
            {
                Debug.WriteLine("Duplicate name or pin");
                return false;
            }
            else
            {
                Lights.Add(light);
                return true;
            }
        }

        public override string ToString()
        {
            return Nickname;
        }

        public ILight this[int index]
        {
            get { return Lights[0]; }
            private set { /* set the specified index to value here */ }
        }

        public int TransitionSpeed = 50;

        public string Nickname { get; }
    }

}
