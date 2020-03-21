using CoronaBL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoCodeTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var apiKey = args[0];
            Console.WriteLine("1. Adresse eingeben");
            string address1 = Console.ReadLine();
            Console.WriteLine("2. Adresse eingeben");
            string address2 = Console.ReadLine();

            var gc = new Geocode(apiKey);
            var distance = gc.GetDistance(address1, address2);
            Console.WriteLine("Distance: "+distance);

        }
    }
}
