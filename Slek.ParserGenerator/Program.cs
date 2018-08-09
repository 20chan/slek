using System;
using System.IO;

namespace Slek.ParserGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var text = File.ReadAllLines("bnf.txt");

            foreach (var line in text)
            {
            }
        }
    }
}
