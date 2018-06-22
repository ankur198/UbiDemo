using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Text;
using Microsoft.Azure.Devices;
using System.Threading.Tasks;

namespace IotHomeWeb.Controllers
{
    public class ValuesController : ApiController
    {
        static string response;
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            if (id == 1)
            {
                response = "Tubelight,1,100";
            }
            else if (id == 0)
            {
                response = "Tubelight,0,100";
            }
            else return "error";

            Task.Run(()=>sendToHome(response));
            return response;
        }

        // POST api/values
        [HttpPost]
        public void Post(string value)
        {
            response = value;
            //sendToHome(value);
            //return value;
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

        private async void sendToHome(string msg)
        {
            var serviceClient = ServiceClient.CreateFromConnectionString("HostName=Home198.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=C0oGr02QT+Z2KHPrwrSuJJunVpPJZgE32kLfs6YHq1I=", Microsoft.Azure.Devices.TransportType.Amqp);
            var message = new Message(Encoding.ASCII.GetBytes(msg));
            await serviceClient.SendAsync("HomePi", message);
        }
    }
}
