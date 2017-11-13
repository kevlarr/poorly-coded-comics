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

        public static async Task<Comic> FetchLatest(HttpClient c)
        {
            var serializer = new DataContractJsonSerializer(typeof(Result));
            var streamTask = c.GetStreamAsync(XKCD.url);
            var res = serializer.ReadObject(await streamTask) as Result;
            return new Comic(res.Id, res.Title, "http://xkcd.com", res.Published);
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
