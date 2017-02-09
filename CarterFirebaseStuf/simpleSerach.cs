using System;
using System.Net;
using Bing;

namespace BingSearchAzureAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            const string bingKey = "3676119700b04cd0b12d98dc57dc5aff";
            var bing = new BingSearchContainer(
                new Uri("https://api.datamarket.azure.com/Bing/Search/")) { Credentials = new NetworkCredential(bingKey, bingKey) };

            var query = bing.Web("Cat", null, null, null, null, null, null, null);
            var results = query.Execute();

            foreach (var result in results)
            {
                Console.WriteLine(result.Url);
            }

            Console.ReadKey();
        }
    }
}
