using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IoT.Lightning.Providers;
using Windows.ApplicationModel.Core;
using Windows.Devices;
using Windows.Devices.Pwm;

namespace HomeManager
{
    internal static class GpioManager
    {
        private static PwmController pwmController;
        private static Dictionary<int, PwmPin> Pins = new Dictionary<int, PwmPin>();

        private static bool IsInitialising = false;

        private async static Task<bool> Initialise()
        {
            IsInitialising = true;
            bool status = false;
            if (LightningProvider.IsLightningEnabled)
            {
                LowLevelDevicesController.DefaultProvider = LightningProvider.GetAggregateProvider();
                var pwmControllers = await PwmController.GetControllersAsync(LightningPwmProvider.GetPwmProvider());
                pwmController = pwmControllers[1];
                pwmController.SetDesiredFrequency(700);
                status = true;
            }
            else
            {
                Debug.WriteLine("Lightning not enabled");
                IsInitialising = false;
            }
            return status;
        }

        internal async static void MakeOutput(int pin)
        {
            while (IsInitialising)
            {
                await Task.Delay(500);
            }

            if (pwmController == null)
            {
                await Initialise();
            }
            if (pwmController != null)
            {
                var x = pwmController.OpenPin(pin);
                Pins.Add(pin, x);
            }

        }

        internal static async void SetPwm(int pin, int val)
        {
            await CoreApplication.MainView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                var x = Pins[pin];
                float pwm = val;
                pwm = pwm / 100;
                Debug.Write(pwm + " ");
                x.SetActiveDutyCyclePercentage(pwm);
                x.Start();
            });
        }
    }
}
