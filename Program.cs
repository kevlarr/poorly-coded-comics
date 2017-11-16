using System;
using System.Collections.Generic;
using System.IO;

namespace poorlycoded
{
    enum Sources {Pdl, Xkcd};

    class Program
    {
        private static string _filename = "comics.csv";

        public static void Main(string[] args)
        {
            Console.WriteLine("\nFetching Poorly Drawn Lines...");
            Comic pdl = Pdl.FetchLatest().Result;
            Console.WriteLine(
                $"{pdl.Id}: {pdl.Title} <{pdl.Link}> @ {pdl.Published}");
            Console.WriteLine("\nFetching XKCD...");
            Comic xkcd = Xkcd.FetchLatest().Result;
            Console.WriteLine(
                $"{xkcd.Id}: {xkcd.Title} <{xkcd.Link}> @ {xkcd.Published}");
            Console.WriteLine();

            // Comic pdlExisting, xkcdExisting;

            if (File.Exists(_filename))
            {
                var lines = new List<string>();
                using (StreamReader sr = File.OpenText(_filename))
                {
                    string s;
                    while ((s = sr.ReadLine()) != null)
                      lines.Add(s);
                }
                foreach(string line in lines)
                    Console.WriteLine(line);
            }
            SaveComics(pdl, xkcd);
        }
        private static void SaveComics(params Comic[] comics)
        {
            using (StreamWriter sr = File.CreateText(_filename))
            {
                sr.WriteLine(Comic.ColumnsToCsv());
                foreach (Comic c in comics)
                    sr.WriteLine(c.ToCsv());
            }
        }
    }
}
