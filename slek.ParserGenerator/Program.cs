using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace slek.ParserGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var bnf = File.ReadAllText("bnf.txt");
            var lexed = Lex(bnf);
            foreach (var l in lexed) Console.WriteLine(l);
            Console.Read();
        }
        
        static string[] Lex(string bnf)
        {
            int idx = 0;
            return LexIter().ToArray();

            IEnumerable<string> LexIter()
            {
                while (idx < bnf.Length)
                    yield return LexOne();
            }
            string LexOne()
            {
                if (char.IsWhiteSpace(bnf[idx]))
                {
                    idx++;
                    return LexOne();
                }
                if (char.IsLetter(bnf[idx]))
                    return LexID();
                if (bnf[idx] == '\'')
                    return LexString();
                if ("()*?|".IndexOf(bnf[idx]) != -1)
                    return bnf[idx++].ToString();
                if (bnf[idx] == ':')
                {
                    idx += 2;
                    return ":=";
                }

                throw new Exception();
            }
            string LexID()
            {
                int start = idx;
                while (idx < bnf.Length && (char.IsLetter(bnf[idx]) || bnf[idx] == '_'))
                    idx++;
                return bnf.Substring(start, idx - start);
            }
            string LexString()
            {
                int start = idx++;
                while (bnf[idx] != '\'')
                    idx++;
                idx++;
                return bnf.Substring(start, idx - start);
            }
        }
    }
}
