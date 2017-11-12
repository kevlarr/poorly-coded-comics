using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace poorlycoded
{
    public class XKCD
    {
        static HttpClient client = new HttpClient();

        public static async Task FetchLatest(HttpClient client)
        {
            var task = client.GetStringAsync("http://xkcd.com/info.0.json");
            var msg = await task;

            Console.WriteLine(msg);
        }
    }
}
