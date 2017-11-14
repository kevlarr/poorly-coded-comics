using System;

namespace poorlycoded
{
    internal class Comic
    {
        public int Id { get; set; }
        public Uri Link { get; set; }
        public DateTime Published { get; set; }
        public int Source { get; set; }
        public string Title { get; set; }
    }
}
