using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

namespace CarEngine
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        internal static readonly EngineStarter engine = new EngineStarter();
        public MainPage()
        {
            this.InitializeComponent();
            engine.EngineStateChanged += OnEngineStateChanged;
            new IotHub(engine).NewMessageRecieved += MainPage_NewMessageRecieved;
        }

        private void MainPage_NewMessageRecieved(object sender, string e)
        {
            txtMsgRecieved.Text = e + "\n";
        }

        private void OnEngineStateChanged(object sender, EngineStarter.EngineState e)
        {
            txtEngineStatus.Text = "Engine is: " + e;
            if (e == EngineStarter.EngineState.EngineRunning || e == EngineStarter.EngineState.OnBattery)
            {
                btnStartStop.IsEnabled = true;
            }
        }

        private void btnStartStop_Click(object sender, RoutedEventArgs e)
        {
            btnStartStop.IsEnabled = false;
            engine.StartStop();
        }
    }
}
