using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Timers;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pinger
{
    class Program
    {
        static async Task Main(string[] args)
        {
            const string url = "http://localhost:8080";

            await Ping(url);
        }

        static async Task Ping(String url)
        {
            using var client = new HttpClient();

            Console.WriteLine("Pinger starts ...");

            var isAvailablePinging = false; // is pinging was successful
            var serviceIsOffline = false;
            var n = 0; // failure response count
            var prevTick = 0;
            
            // ping
            while (true)
            {
                int curTick = DateTime.Now.Second;
                // ping every 2 seconds
                if (curTick - prevTick < 2 && curTick >= prevTick) continue;
                prevTick = curTick;
                try
                {
                    var response = await client.GetAsync(url);
                    if (response.StatusCode.ToString() == "OK")
                    {
                        // server pinging success
                        isAvailablePinging = true;
                        // service is online
                        serviceIsOffline = false;
                        n = 0;
                    }
                    else
                    {
                        // server pinging fails
                        isAvailablePinging = false;
                        n++;
                    }
                }
                catch (Exception error)
                {
                    // server pinging fails and throws error
                    isAvailablePinging = false;
                    n++;
                }

                // print status
                if (isAvailablePinging)
                {
                    // server is available
                    Console.WriteLine("Service " + url + " is available!");
                }
                else
                {
                    // service pinging failures less than 10 times 
                    if (n < 10)
                    {
                        Console.WriteLine("Pinging to " + url + " fails!");
                    }
                    // service is not available more than 10 times, print status and wait for positive response
                    else if (!serviceIsOffline)
                    {
                        serviceIsOffline = true;
                        Console.WriteLine("Service " + url + " IS NOT AVAILABLE!");
                    }
                }
            }
        }
    }
}