using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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

        private void btnAddLight_Click(object sender, RoutedEventArgs e)
        {
            if (txtIP.Text != "")
            {
                try
                {
                    Uri uri = new Uri("http://" + txtIP.Text);

                    HallLights.AddLight(txtNickname.Text, int.Parse(txtPin.Text), uri);
                }
                catch (Exception)
                {
                    Debug.WriteLine("ip failed");
                    //throw;
                }

            }
            else HallLights.AddLight(txtNickname.Text, int.Parse(txtPin.Text));
            txtIP.Text = "";
        }
    }
}
