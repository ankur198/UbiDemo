using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;

class AzureIoTHub
{
    private static void CreateClient()
    {
        if (deviceClient == null)
        {
            // create Azure IoT Hub client from embedded connection string
            deviceClient = DeviceClient.CreateFromConnectionString(deviceConnectionString, TransportType.Mqtt);
        }
    }

    static DeviceClient deviceClient = null;


    const string deviceConnectionString = "HostName=Home198.azure-devices.net;DeviceId=HomePi;SharedAccessKey=ILkXr/EDuCeaHFtMhblzeG0oD+CFgupPtQFveOLhh/E=";

    public static event EventHandler<string> OnMessageRecieved;


    public static async Task SendDeviceToCloudMessageAsync(string Message)
    {
        CreateClient();
        var str = "{\"deviceId\":\"HomePi\",\"messageId\":1,\"text\":\"Hello, Cloud from a UWP C# app!\"}";
        var message = new Message(Encoding.ASCII.GetBytes(str));

        await deviceClient.SendEventAsync(message);
    }

    private static async Task<string> ReceiveCloudToDeviceMessageAsync()
    {
        CreateClient();

        while (true)
        {
            var receivedMessage = await deviceClient.ReceiveAsync();

            if (receivedMessage != null)
            {
                var messageData = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                await deviceClient.CompleteAsync(receivedMessage);
                return messageData;
            }

            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }

    public static async void RecieveCloudToDeviceMessageIndefinately()
    {
        while (true)
        {
            var message = await ReceiveCloudToDeviceMessageAsync();
            OnMessageRecieved?.Invoke(null, message);
            await Task.Delay(TimeSpan.FromSeconds(10));
        }
    }
}
