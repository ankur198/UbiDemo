using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;

namespace CarEngine
{
    class IotHub
    {
        EngineStarter Engine;
        public List<string> MessageRecieved = new List<string>();
        internal event EventHandler<string> NewMessageRecieved;

        void InvokeNewMessageRecieved(string Message)
        {
            ReceiveCloudToDeviceMessageAsync();
            if (NewMessageRecieved != null)
            {
                NewMessageRecieved.Invoke(this, Message);
            }
        }

        public IotHub(EngineStarter engine)
        {
            CreateClient();
            Engine = engine;
            Engine.EngineStateChanged += Engine_EngineStateChanged;
            ReceiveCloudToDeviceMessageAsync();

        }

        private void Engine_EngineStateChanged(object sender, EngineStarter.EngineState e)
        {
            SendDeviceToCloudMessageAsync(e.ToString());
        }

        private void CreateClient()
        {
            if (deviceClient == null)
            {
                // create Azure IoT Hub client from embedded connection string
                deviceClient = DeviceClient.CreateFromConnectionString(deviceConnectionString, TransportType.Mqtt);
            }
        }

        DeviceClient deviceClient = null;

        //
        // Note: this connection string is specific to the device "uwp". To configure other devices,
        // see information on iothub-explorer at http://aka.ms/iothubgetstartedVSCS
        //
        const string deviceConnectionString = "HostName=Home198.azure-devices.net;DeviceId=uwp;SharedAccessKey=4lN4tE33IoqaortTrpTUBMSPdfoy2RBFhpF7IqAn7uE=";


        //
        // To monitor messages sent to device "kraaa" use iothub-explorer as follows:
        //    iothub-explorer monitor-events --login HostName=Home198.azure-devices.net;SharedAccessKeyName=service;SharedAccessKey=WO4klPRGoqldD4IudJCSOYQntRBm5444oh/7FNu/VTs= "uwp"
        //

        // Refer to http://aka.ms/azure-iot-hub-vs-cs-2017-wiki for more information on Connected Service for Azure IoT Hub

        public async Task SendDeviceToCloudMessageAsync(string state)
        {
            CreateClient();

            var str = "{\"deviceId\":\"uwp\",\"messageId\":1,\"text\":" + state + "}";

            var message = new Message(Encoding.ASCII.GetBytes(str));

            await deviceClient.SendEventAsync(message);
        }

        public async Task<string> ReceiveCloudToDeviceMessageAsync()
        {
            CreateClient();

            while (true)
            {
                var receivedMessage = await deviceClient.ReceiveAsync();

                if (receivedMessage != null)
                {
                    var messageData = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                    var prop = receivedMessage.Properties;
                    Engine.Property(prop);
                    await deviceClient.CompleteAsync(receivedMessage);
                    MessageRecieved.Add(messageData);
                    InvokeNewMessageRecieved(messageData);
                    return messageData;
                }

                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }


    }

}