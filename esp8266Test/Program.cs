using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace esp8266Test
{
    class Program
    {
        static void Main(string[] args)
        {
            set(5, 1023);
            get(5);
            set(15, 100);
            get(15);
            set(0, 200);
            get(0);
            set(2, 300);
            get(2);
            Console.ReadLine();
        }

        async static void set(int pin, int value)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new System.Uri("http://192.168.1.104/");
            var x = await client.GetStringAsync("/setpin/" + pin + "/" + value);
            Console.WriteLine(x);
        }
        async static void get(int pin)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new System.Uri("http://192.168.1.104/");
            var x = await client.GetStringAsync("/getpin/" + pin);
            Console.WriteLine(x);
        }
    }
}
