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
        static Uri ip = new Uri("http://192.168.1.101");
        static void Main(string[] args)
        {
            doSample();
            Console.ReadLine();
        }

        private static async void doSample()
        {
            await set(16, 1023);
            await get(16);
            await status();
            await verify('2');

            while (true)
            {
                await set(16, 0);
                await Task.Delay(TimeSpan.FromSeconds(1));
                await set(16, 1023);
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }

        async static Task set(int pin, int value)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = ip;
            var x = await client.GetStringAsync("/1/6027/setpin/" + pin + "/" + value);
            Console.WriteLine(x);
        }
        async static Task get(int pin)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = ip;
            var x = await client.GetStringAsync("/1/6027/getpin/" + pin);
            Console.WriteLine(x);
        }
        async static Task status()
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = ip;
            var x = await client.GetStringAsync("/1/6027/status/");
            Console.WriteLine(x);
        }

        async static Task verify(char c)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = ip;
            var x = await client.GetStringAsync("/1/6027/verify/" + c);
            Console.WriteLine(x);
        }

    }
}
