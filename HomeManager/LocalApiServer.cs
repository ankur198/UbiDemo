using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Sockets;

//See-https://www.codeproject.com/Articles/599978/An-HttpListener-Server-for-Handling-AJAX-POST-Requ

namespace HomeManager
{
    static class LocalApiServer
    {
        static HttpListener httpListener = new HttpListener();

        public static async void Start()
        {


            httpListener.Prefixes.Add("http://*:4545/");
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

                    if (req!="")
                    {
                        MakeChanges(req);
                    }

                    context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                    context.Response.Headers.Add("Access-Control-Allow-Methods", "POST, GET");
                    using (var y = context.Response.OutputStream.AsOutputStream().AsStreamForWrite())
                    {
                        StreamWriter streamWriter = new StreamWriter(y);
                        streamWriter.Write(Message.PrepMessage());
                        streamWriter.Flush();
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        private static void MakeChanges(string Request)
        {
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<LightMessageObject>(Request);

            var Room = LightManager.Rooms.ToList().FirstOrDefault(room => room.Nickname == obj.Room);

            var Light = Room.Lights.ToList().Find(l => l.Nickname == obj.Nickname);

            if (Light!=null)
            {
                Light.State = obj.State;
                Light.Brightness = obj.Brightness;
            }
        }
    }
}
