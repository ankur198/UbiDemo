using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using Windows.Networking.Connectivity;

namespace HomeManager
{
    static class LocalWebServer
    {
        static HttpListener httpListener = new HttpListener();

        public static async void Start()
        {
            httpListener.Prefixes.Add("http://*:4546/");
            httpListener.Start();
            httpListener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;

            while (true)
            {
                try
                {
                    var context = await httpListener.GetContextAsync();
                    Debug.WriteLine(context.Request.Url);

                    var req = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding).ReadToEnd();
                    Debug.WriteLine(req);

                    context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                    context.Response.Headers.Add("Access-Control-Allow-Methods", "POST, GET");
                    using (var y = context.Response.OutputStream.AsOutputStream().AsStreamForWrite())
                    {
                        StreamWriter streamWriter = new StreamWriter(y);
                        var m = PrepWebsite();
                        streamWriter.Write(m);
                        await streamWriter.FlushAsync();

                    }

                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        private static string PrepWebsite()
        {
            string ip = GetIP();
            string html = @"<!DOCTYPE html><html><head><meta charset='utf-8' /><meta http-equiv='X-UA-Compatible' content='IE=edge'><title>Page Title</title><meta name='viewport' content='width=device-width, initial-scale=1'><link rel='stylesheet' href=crossorigin='anonymous'><link rel='stylesheet' type='text/css' media='screen' href='main.css' /><script src='https://code.jquery.com/jquery-3.3.1.slim.min.js' integrity='sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo'crossorigin='anonymous'></script><script src='https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.3/umd/popper.min.js' integrity='sha384-ZMP7rVo3mIykV+2+9J3UJ46jBk0WLaUAdn689aCwoqbBJiSnjAK/l8WvCWPIPm49'crossorigin='anonymous'></script><script src='https://stackpath.bootstrapcdn.com/bootstrap/4.1.1/js/bootstrap.min.js' integrity='sha384-smHYKdLADwkXOn1EmN1qk/HfnUcbVRZyYmZ4qpPea6sjB/pTJ0euyQp0Mk8ck+5T'crossorigin='anonymous'></script></head><body class='container'><div class='card'><div id='data' class='container'> </div></div><br><script>function GetData() {let request = new XMLHttpRequest();request.open('GET', 'http://" +
                       ip + @":4545', false);request.send(null); let res = request.responseText;return JSON.parse(res);}function SendData(message) {console.log(message);let json = JSON.stringify(message);console.log(json);let request = new XMLHttpRequest();request.open('POST', 'http://" +
                       ip + @":4545', false);request.send(json);}function MakeDom() {let lights = GetData();let currentRoom = '';lights.forEach(element => {if (currentRoom !== element.Room) {let heading = document.createElement('h3');heading.innerText = element.Room;currentRoom = element.Room; document.getElementById('data').appendChild(heading);}let Light = document.createElement('div');let Nickname = document.createElement('div');Nickname.textContent = element.Nickname;Light.appendChild(Nickname); Light.className = 'card';let State = document.createElement('input'); State.type = 'checkbox';State.checked = element.State;State.onchange = function () {element.State = State.checked; SendData(element);};let Brightness = document.createElement('input'); Brightness.type = 'range'; Brightness.min = '0'; Brightness.max = '100'; Brightness.value = element.Brightness; Brightness.onchange = function () {element.Brightness = Brightness.value; SendData(element); };Light.appendChild(State); Light.appendChild(Brightness); document.getElementById('data').appendChild(Light);});} MakeDom(); </script></body></html>";
            return html;
        }

        private static string GetIP()
        {
            foreach (var localHost in NetworkInformation.GetHostNames())
            {
                if (localHost.IPInformation != null)
                {
                    return localHost.ToString();
                }
            }
            return "";
        }
    }
}
