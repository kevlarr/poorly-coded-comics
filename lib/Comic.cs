using System;

namespace poorlycoded
{
    public class Comic
    {
        public int Id { get; }
        public Uri Link { get; }
        public DateTime Published { get; }
        public string Title { get; }

        public Comic(int id, string title, string link, DateTime published)
        {
            Id = id;
            Link = new Uri(link);
            Title = title;
            Published = published;
            // TODO: source from enum
        }
    }
}
