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
    internal class Pdl
    {
        private static string _url = "http://feeds.feedburner.com/PoorlyDrawnLines?format=xml";

        public static async Task<Comic> FetchLatest()
        {
            var comic = new Comic{ Source = (int)Sources.Pdl };
            var settings = new XmlReaderSettings();
            settings.Async = true;
            using (var reader = XmlReader.Create(_url, settings)) {
                await ProcessXml(reader, comic);
            }
            return comic;
        }

        private static async Task ProcessXml(XmlReader r, Comic c)
        {
            // TODO: MSDN shoes ReadToFollowingAsync but not found..?
            r.ReadToFollowing("item");

            while (await r.ReadAsync())
            {
                switch (r.LocalName)
                {
                    case "title":
                        c.Title = r.ReadInnerXml();
                        break;
                    case "pubDate":
                        DateTime dt;
                        if (DateTime.TryParse(r.ReadInnerXml(), out dt))
                            c.Published = dt;
                        break;
                    case "origLink":
                        c.Link = new Uri(r.ReadInnerXml());
                        break;
                    case "guid":
                        int guid;
                        var rgx = new Regex(@"(?<=p=)[0-9]+");
                        var match = rgx.Match(r.ReadInnerXml());
                        if (int.TryParse(match.Value, out guid))
                            c.Id = guid;
                        break;
                    // Only need the first (most recent) item
                    case "item":
                        return;
                }
            }
        }
    }
}
