using System;
using System.Collections.ObjectModel;
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
        LightManager HallLights = new LightManager("HallLights");
        ObservableCollection<LightManager> Rooms = LightManager.Rooms;

        public MainPage()
        {
            this.InitializeComponent();
            Rooms.Add(HallLights);
            HallLights.AddLight("Tubelight", 13);
            LocalApiServer.Start();
            LocalWebServer.Start();
        }

        private void btnAddLight_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(comboSelectedRoom.SelectedIndex);
            if (comboSelectedRoom.SelectedIndex != -1)
            {
                int index = comboSelectedRoom.SelectedIndex;
                if (txtIP.Text != "")
                {
                    try
                    {
                        Uri uri = new Uri("http://" + txtIP.Text);

                        Rooms[index].AddLight(txtNickname.Text, int.Parse(txtPin.Text), uri);
                    }
                    catch (Exception)
                    {
                        Debug.WriteLine("ip failed");
                        //throw;
                    }

                }
                else Rooms[index].AddLight(txtNickname.Text, int.Parse(txtPin.Text));
            }
            Debug.WriteLine(Message.PrepMessage());
        }

        private void btnAddRoom_Click(object sender, RoutedEventArgs e)
        {
            Rooms.Add(new LightManager(txtRoomNickname.Text));
        }
    }
}
