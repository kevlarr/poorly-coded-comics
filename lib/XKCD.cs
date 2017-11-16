/*
Sample JSON response:

{
    "month": "11", "num": 1914, "link": "", "year": "2017", "news": "",
    "safe_title": "Twitter Verification", "transcript": "",
    "alt": "When we started ...", "img": "https://imgs.xkcd.com/...",
    "title": "Twitter Verification", "day": "10"
}
 */

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace poorlycoded
{
    internal class Xkcd
    {
        private static string _url = "http://xkcd.com/info.0.json";
        private static string[] _headerValues = {"application/json"};


        public static async Task<Comic> FetchLatest()
        {
            var client = NewClient();
            var serializer = new DataContractJsonSerializer(typeof(Result));
            var stream = client.GetStreamAsync(_url);
            var res = serializer.ReadObject(await stream) as Result;
            return new Comic{
                Id = res.Id,
                Link = new Uri($"http://xkcd.com/{res.Id}"),
                Published = res.Published,
                Source = (int)Sources.Xkcd,
                Title = res.Title,
            };
        }

        private static HttpClient NewClient()
        {
            var client = new HttpClient();
            var headers = client.DefaultRequestHeaders;

            // While fun to add these, they aren't necessary
            headers.Add("User-Agent", "Comicbot/1.0");
            headers.Accept.Clear();

            foreach (string h in _headerValues)
                headers.Accept.Add(new MediaTypeWithQualityHeaderValue(h));
            return client;
        }

        [DataContract]
        private class Result
        {
            [DataMember(Name="num")]
            public int Id { get; set; }

            [DataMember(Name="title")]
            public string Title { get; set; }

            [DataMember(Name="day")]
            public string Day { get; set; }

            [DataMember(Name="month")]
            public string Month { get; set; }

            [DataMember(Name="year")]
            public string Year { get; set; }

            [IgnoreDataMember]
            public DateTime Published
            {
                get
                {
                    int year, month, day;
                    if (
                        int.TryParse(Year, out year) &&
                        int.TryParse(Month, out month) &&
                        int.TryParse(Day, out day)
                    )
                        return new DateTime(year, month, day);

                    return DateTime.Today;
                }
            }
        }
    }
}
