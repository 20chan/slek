using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using slek.Compilers.Parser;
using slek.Compilers.Syntax;
using static slek.Compilers.Syntax.SyntaxTokenType;

namespace slek.Tests
{
    [TestClass]
    public class LexerTest
    {
        [TestMethod]
        public void TestLexID()
        {
        }

        void AssertLex(string code, params SyntaxToken[] tokens)
        {
            CollectionAssert.AreEqual(tokens, Lexer.Lex(code));
        }
    }
}
