using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HomeManager
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        LightManager HallLights = new LightManager();
        public MainPage()
        {
            this.InitializeComponent();

            HallLights.AddLight("Tubelight", 13);
        }

        private async void btnOn_Click(object sender, RoutedEventArgs e)
        {
            MakeUiEnabled(false);
            await HallLights.TurnOnAsync("Tubelight");
            MakeUiEnabled(true);
        }


        private async void btnOff_Click(object sender, RoutedEventArgs e)
        {
            MakeUiEnabled(false);
            await HallLights.TurnOffAsync("Tubelight");
            MakeUiEnabled(true);
        }

        private void MakeUiEnabled(bool val)
        {
            btnOn.IsEnabled = val;
            btnOff.IsEnabled = val;
        }

        private void sliderTransition_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            HallLights.TransitionSpeed = (int)sliderTransition.Value;
        }

        private void sliderBrightness_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            HallLights.SetBrightness("Tubelight", (int)sliderBrightness.Value);
        }
    }
}
