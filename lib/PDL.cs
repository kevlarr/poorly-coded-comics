/*
Sample RSS item:

<item>
    <title>With the Humans</title>
    <pubDate>Fri, 10 Nov 2017 17:11:14 +0000</pubDate>
    <guid isPermaLink ="false">http://www.poorlydrawnlines.com/?p=6498</guid>
    ...
    <feedburner:origLink>http://www.poorlydrawnlines.com/comic/with-the-humans/</feedburner:origLink>
</item>
 */

using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace poorlycoded
{
    public class PDL
    {
        static string url = "http://feeds.feedburner.com/PoorlyDrawnLines?format=xml";

        public static async Task<Processed> FetchLatest()
        {
            var firstItem = new Processed();
            var settings = new XmlReaderSettings();
            settings.Async = true;
            using (var reader = XmlReader.Create(PDL.url, settings)) {
                await ProcessXml(reader, firstItem);
            }
            return firstItem;
        }

        public static async Task ProcessXml(XmlReader r, Processed p)
        {
            r.ReadToFollowing("item");

            while (await r.ReadAsync())
            {
                switch (r.LocalName)
                {
                    case "title":
                        p.Title = r.ReadInnerXml();
                        break;
                    case "pubDate":
                        DateTime dt;
                        if (DateTime.TryParse(r.ReadInnerXml(), out dt))
                            p.Published = dt;
                        break;
                    case "origLink":
                        p.Href = new Uri(r.ReadInnerXml());
                        break;
                    case "guid":
                        int guid;
                        var rgx = new Regex(@"(?<=p=)[0-9]+");
                        var match = rgx.Match(r.ReadInnerXml());
                        if (int.TryParse(match.Value, out guid))
                            p.ID = guid;
                        break;
                    // Only need the first (most recent) item
                    case "item":
                        return;
                }
            }
        }

        public class Processed
        {
            public int ID { get; set; }
            public Uri Href { get; set; }
            public DateTime Published { get; set; }
            public string Title { get; set; }
        }
    }
}
