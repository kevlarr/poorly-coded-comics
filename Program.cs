using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace poorlycoded
{
    class Program
    {
        static string version = "0.1.0";
        static string[] headerValues = {
            "application/json",
        };

        static void Main(string[] args)
        {
            var client = NewClient();

            Console.WriteLine("Fetching XKCD...");
            var xkcd = XKCD.FetchLatest(client).Result;
            Console.WriteLine($"{xkcd.ID}: {xkcd.Title}");

            Console.WriteLine("Fetching Poorly Drawn Lines...");
            var pdl = PDL.FetchLatest().Result;

            Console.WriteLine(pdl.Title);
            Console.WriteLine(pdl.Href);
            Console.WriteLine(pdl.Published);
            Console.WriteLine(pdl.ID);
        }

        static HttpClient NewClient()
        {
            var client = new HttpClient();
            var headers = client.DefaultRequestHeaders;

            // While fun to add these, they aren't necessary
            headers.Add("User-Agent", $"Comicbot/{Program.version}");
            headers.Accept.Clear();
            foreach (string h in Program.headerValues)
                headers.Accept.Add(new MediaTypeWithQualityHeaderValue(h));

            return client;
        }
    }
}
