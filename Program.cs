using System;
using System.Collections.Generic;
using System.IO;

namespace poorlycoded
{
    enum Source {Pdl, Xkcd};

    class Program
    {
        private static string _filename = "comics.csv";
        private static Dictionary<Source, string> _titles = new Dictionary<Source, string> {
            { Source.Pdl, "Poorly Drawn Lines" },
            { Source.Xkcd, "XKCD" },
        };

        public static void Main(string[] args)
        {
            // foreach (Source s in Enum.GetValues(typeof(Source)))
                // Console.WriteLine(_titles[s]);

            // Console.WriteLine("\nFetching Poorly Drawn Lines...");
            // Comic pdlNew = Pdl.FetchLatest().Result;
            // Console.WriteLine(
                // $"{pdl.Id}: {pdl.Title} <{pdl.Link}> @ {pdl.Published}");
            // Console.WriteLine("\nFetching XKCD...");
            // Comic xkcdNew = Xkcd.FetchLatest().Result;
            // Console.WriteLine(
                // $"{xkcd.Id}: {xkcd.Title} <{xkcd.Link}> @ {xkcd.Published}");

            var (pdlOld, xkcdOld) = ReadExistingComics();
            var (pdlNew, xkcdNew) = FetchRecentComics();

            CompareComics(pdlOld, pdlNew);
            CompareComics(xkcdOld, xkcdNew);

            SaveNewestComics(pdlNew, xkcdNew);
        }

        private static void CompareComics(Comic cOld, Comic cNew)
        {
            // TODO: check that both belong to same source
            var title = _titles[(Source)cOld.Source];

            if (cOld != null && cOld.Id == cNew.Id)
            {
                // TODO: what to do on not new
                Console.WriteLine($"No new {title}");
            }
            else
            {
                // TODO: what to do on new
                Console.WriteLine($"{title} has a new comic!");
            }
        }

        private static (Comic, Comic) FetchRecentComics()
        {
            return (Pdl.FetchLatest().Result, Xkcd.FetchLatest().Result);
        }

        private static (Comic, Comic) ReadExistingComics()
        {
            if (File.Exists(_filename))
            {
                var pdl = new Comic();
                var xkcd = new Comic();

                using (StreamReader sr = File.OpenText(_filename))
                {
                    // Skip the columns and assume the right order for now,
                    // but in future extract them and pass to UpdateFromCsv
                    sr.ReadLine();
                    pdl.UpdateFromCsv(sr.ReadLine());
                    xkcd.UpdateFromCsv(sr.ReadLine());
                    return (pdl, xkcd);
                }
            }
            return (null, null);
        }

        private static void SaveNewestComics(params Comic[] comics)
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
