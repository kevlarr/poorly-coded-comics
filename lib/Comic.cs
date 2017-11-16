using System;
using System.Collections.Generic;

namespace poorlycoded
{
    internal class Comic
    {
        // Properties are precisely ordered for CSV export
        private static List<string> _columns = new List<string>() {"Source", "Id", "Title", "Published", "Link"};

        public static string ColumnsToCsv()
        {
            return Join(c => $"\"{c}\"");
        }

        private static string Join(Converter<string, string> fn)
        {
            return String.Join(",", _columns.ConvertAll(fn));
        }

        public int Id { get; set; }
        public Uri Link { get; set; }
        public DateTime Published { get; set; }
        public int Source { get; set; }
        public string Title { get; set; }

        public string ToCsv()
        {
            Type t = typeof(Comic);
            return Join(c => $"\"{t.GetProperty(c)?.GetValue(this, null)}\"");
        }
    }
}
