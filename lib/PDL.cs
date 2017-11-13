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

        public static async Task<Comic> FetchLatest()
        {
            var res = new Result();
            var settings = new XmlReaderSettings();
            settings.Async = true;
            using (var reader = XmlReader.Create(PDL.url, settings)) {
                await ProcessXml(reader, res);
            }
            return new Comic(res.Id, res.Title, res.Link, res.Published);
        }

        private static async Task ProcessXml(XmlReader r, Result res)
        {
            // TODO: MSDN shoes ReadToFollowingAsync but not found..?
            r.ReadToFollowing("item");

            while (await r.ReadAsync())
            {
                switch (r.LocalName)
                {
                    case "title":
                        res.Title = r.ReadInnerXml();
                        break;
                    case "pubDate":
                        DateTime dt;
                        if (DateTime.TryParse(r.ReadInnerXml(), out dt))
                            res.Published = dt;
                        break;
                    case "origLink":
                        res.Link = r.ReadInnerXml();
                        break;
                    case "guid":
                        int guid;
                        var rgx = new Regex(@"(?<=p=)[0-9]+");
                        var match = rgx.Match(r.ReadInnerXml());
                        if (int.TryParse(match.Value, out guid))
                            res.Id = guid;
                        break;
                    // Only need the first (most recent) item
                    case "item":
                        return;
                }
            }
        }

        private class Result
        {
            public int Id { get; set; }
            public string Link { get; set; }
            public DateTime Published { get; set; }
            public string Title { get; set; }
        }
    }
}
