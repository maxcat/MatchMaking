using System;
using System.IO;

namespace MatchMakingCore
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            new Container().Test();

            Console.WriteLine($"current directory {Directory.GetCurrentDirectory()}");
            Console.WriteLine("Hello World!");
        }
    }
}
