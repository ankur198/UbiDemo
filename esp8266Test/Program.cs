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
            set();
            get();
            Console.ReadLine();
        }

        async static void set()
        {
            HttpClient client = new HttpClient();
            
            client.BaseAddress = new System.Uri("http://192.168.1.104/");
            var x = await client.GetStringAsync("/setpin/0/103");
            Console.WriteLine(x);
        }
        async static void get()
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new System.Uri("http://192.168.1.104/");
            var x = await client.GetStringAsync("/getpin/0");
            Console.WriteLine(x);
        }
    }
}
