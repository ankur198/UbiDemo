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
    static class LocalServer
    {
        static StreamSocketListener listener = new StreamSocketListener();
        static HttpListener httpListener = new HttpListener();

        public static async void Start()
        {

            //await listener.BindServiceNameAsync("4545");
            //listener.ConnectionReceived += Listener_ConnectionReceivedAsync;

            httpListener.Prefixes.Add("http://*:4545/");
            httpListener.Start();
            httpListener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;

            while (true)
            {
                try
                {
                    var context = await httpListener.GetContextAsync();
                    Debug.WriteLine(context.Request.Url);
                    //var x = new StreamReader(context.Request.InputStream.AsInputStream().AsStreamForRead()).ReadLine();

                    var req = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding).ReadToEnd();
                    Debug.WriteLine(req);

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

        private static async void Listener_ConnectionReceivedAsync(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            string request;
            using (var x = args.Socket.InputStream.AsStreamForRead())
            {
                StreamReader reader = new StreamReader(x);
                request = await reader.ReadToEndAsync();
            }
            string response = ProcessRequest(request);

            using (var x = args.Socket.OutputStream.AsStreamForWrite())
            {
                StreamWriter writer = new StreamWriter(x);
                await writer.WriteLineAsync(response);
                await writer.FlushAsync();
                writer.Dispose();
            }
            args.Socket.Dispose();
        }

        private static string ProcessRequest(string request)
        {
            string response = "";
            switch (request)
            {
                case "GetAll":
                    response = Message.PrepMessage();
                    break;

                default:
                    response = "Invalid request";
                    break;
            }
            return response;
        }
    }
}
