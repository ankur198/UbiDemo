using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeManager
{
    class Message
    {
        public static string PrepMessage()
        {
            List<LightMessageObject> messageObjects = new List<LightMessageObject>();

            foreach (var Room in LightManager.Rooms)
            {
                foreach (var Light in Room.Lights)
                {
                    var x = new LightMessageObject
                    {
                        Nickname = Light.Nickname,
                        Brightness = Light.Brightness,
                        State = Light.State,
                        Room = Room.Nickname
                    };
                    messageObjects.Add(x);
                }
            }

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(messageObjects);
            return json;
        }
    }
}
