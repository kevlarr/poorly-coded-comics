using System;
namespace poorlycoded
{
    enum Sources {Pdl, Xkcd};

    class Program
    {
        static void Main(string[] args)
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
        }
    }
}
