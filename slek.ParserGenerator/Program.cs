using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Field = slek.ParserGenerator.GeneratedClass.ClassField;
using static slek.ParserGenerator.GeneratedClass.Modifier;

namespace slek.ParserGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var nodes = File.ReadAllLines("node.txt").Select(s => s.Split()).ToArray();
            var generated = Generate(nodes);
            File.WriteAllLines("out.txt", generated.Select(s => s.ToString()), Encoding.UTF8);
            foreach (var g in generated)
                Console.Write(g);
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

        static Node[] Parse(string[][] bnfs)
        {
            return bnfs.Select(s => ParseOne(s)).ToArray();
            Node ParseOne(string[] bnf)
            {
                int i = 2; 
                var name = bnf[0];
                return new Define(name, ParseSeq(bnf, ref i));
            }
            Node ParseSeq(string[] bnf, ref int i)
            {
                var nodes = new List<Node>();
                while (i < bnf.Length && bnf[i] != ")")
                {
                    nodes.Add(ParseOr(bnf, ref i));
                }
                return new Sequence(nodes.ToArray());
            }
            Node ParseOr(string[] bnf, ref int i)
            {
                var l = ParseMaybe(bnf, ref i);
                if (i == bnf.Length) return l;
                if (bnf[i] == "|")
                {
                    i++;
                    return new Or(l, ParseOr(bnf, ref i));
                }
                else return l;
            }
            Node ParseMaybe(string[] bnf, ref int i)
            {
                var l = ParseAtom(bnf, ref i);
                if (i == bnf.Length) return l;
                if (bnf[i] == "?")
                {
                    i++;
                    return new Maybe(l);
                }
                else if (bnf[i] == "*")
                {
                    i++;
                    return new Repeat(l);
                }
                else return l;
            }
            Node ParseAtom(string[] bnf, ref int i)
            {
                if (bnf[i] == "(")
                {
                    i++;
                    var node = ParseSeq(bnf, ref i);
                    if (bnf[i++] != ")")
                        throw new Exception();
                    return node;
                }
                return new Value(bnf[i++]);
            }
        }

        static GeneratedClass[] Generate(string[][] nodes)
        {
            var result = new List<GeneratedClass>();
            foreach (var n in nodes)
            {
                string name = n[0], inhSource = n[1];
                bool isInherited = true;
                if (n[1] == "{")
                    isInherited = false;
                var parameters = new List<Field>();
                for (int i = 3; i < n.Length - 1; i += 2)
                    parameters.Add(new Field(@public, n[i], n[i + 1]));

                result.Add(new GeneratedClass(4, @public, name, isInherited, inhSource, parameters.ToArray()));
            }
            return result.ToArray();
        }
    }
}
