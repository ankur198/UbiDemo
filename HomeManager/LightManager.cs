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
        public ObservableCollection<Light> Lights = new ObservableCollection<Light>();

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

}
