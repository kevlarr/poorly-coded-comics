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
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace poorlycoded
{
    public class XKCD
    {
        static string url = "http://xkcd.com/info.0.json";

        public static async Task<Processed> FetchLatest(HttpClient c)
        {
            var serializer = new DataContractJsonSerializer(typeof(Processed));
            var streamTask = c.GetStreamAsync(XKCD.url);
            return serializer.ReadObject(await streamTask) as Processed;
        }

        [DataContract]
        public class Processed
        {
            [DataMember(Name="day")]
            public string Day { get; set; }

            [DataMember(Name="num")]
            public int ID { get; set; }

            [DataMember(Name="month")]
            public string Month { get; set; }

            [DataMember(Name="alt")]
            public string Text { get; set; }

            [DataMember(Name="title")]
            public string Title { get; set; }

            [DataMember(Name="year")]
            public string Year { get; set; }
        }
    }
}
