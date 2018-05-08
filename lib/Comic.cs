using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace poorlycoded
{
    internal class Comic
    {
        // Properties are precisely ordered for CSV export
        private static List<string> _columns =
          new List<string>() {"Source", "Id", "Title", "Published", "Link"};
        // Only split on commas surrounded by quotes
        private static string _pattern = "(?<=\"),(?=\")";

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

        // Reflection perhaps is not something to get used to,
        // but this is more a learning exercise, so...
        public void UpdateFromCsv(string csvRow)
        {
            Type t = typeof(Comic);
            PropertyInfo[] props = t.GetProperties();
            string[] values = Regex.Split(csvRow, _pattern);

            // For now, csv export is assumed to be ordered identically to _columns
            for (int i = 0; i < _columns.Count; ++i)
            {
                var prop = Array.Find(props, p => p.Name == _columns[i]);
                var v = values[i].Replace("\"", "");

                if (prop.Name == "Link")
                    Link = new Uri(v);
                else
                {
                    var convertedValue = Convert.ChangeType(v, prop.PropertyType);
                    prop.SetValue(this, convertedValue);
                }
            }
        }

        public string ToCsv()
        {
            Type t = typeof(Comic);
            return Join(c => $"\"{t.GetProperty(c)?.GetValue(this, null)}\"");
        }
    }
}
