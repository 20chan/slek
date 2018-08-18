using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using slek.Compilers.Parser;
using slek.Compilers.Syntax;

using static slek.Compilers.Syntax.SyntaxTokenType;
using static slek.Compilers.Syntax.SyntaxFactory;
using slek.Compilers.Exceptions;

namespace slek.Tests
{
    [TestClass]
    public class LexerTest_
    {
        [TestCategory("Lexer"), TestMethod]
        public void TestNumericLexing()
        {
            AssertLex("0", Literal("0", 0));
            AssertLex("012", Literal("012", 12));
            AssertLex("0.11", Literal("0.11", 0.11f));
        }

        [TestCategory("Lexer"), TestMethod]
        public void TestStringLexing()
        {
            AssertLex("\"abc\"", Literal("abc"));
            AssertLex("\"a  bc  \" ", Literal("a  bc  "));
            AssertLex("\"\\\\\"", Literal("\\"));
            AssertLex("\"slash\\\"\"", Literal("slash\""));
        }

        [TestCategory("Lexer"), TestMethod]
        public void TestWildcardLexing()
        {
            AssertLex("_", KeywordToken(WildcardKeyword));
            AssertLex("__", Identifier("__"));
        }

        [TestCategory("Lexer"), TestMethod]
        public void TestKeywordLexing()
        {
            AssertLex("+", KeywordToken(PlusToken));
            AssertLex("-", KeywordToken(MinusToken));
            AssertLex("*", KeywordToken(AsteriskToken));
            AssertLex("/", KeywordToken(SlashToken));
            AssertLex("%", KeywordToken(PercentToken));
            AssertLex("++", KeywordToken(ConcatToken));
            AssertLex(">", KeywordToken(GreaterToken));
            AssertLex("<", KeywordToken(LessToken));
            AssertLex(">=", KeywordToken(GreaterEqualToken));
            AssertLex("<=", KeywordToken(LessEqualToken));
            AssertLex("==", KeywordToken(EqualToken));
            AssertLex("!=", KeywordToken(NotEqualToken));
            AssertLex("<<", KeywordToken(LShiftToken));
            AssertLex(">>", KeywordToken(RShiftToken));
            AssertLex("!", KeywordToken(ExclamationToken));
            AssertLex("~", KeywordToken(TildeToken));
            AssertLex("|", KeywordToken(VBarToken));
            AssertLex("&", KeywordToken(AmperToken));
            AssertLex("||", KeywordToken(DoubleVBarToken));
            AssertLex("&&", KeywordToken(DoubleAmperToken));
            AssertLex("^", KeywordToken(CaretToken));
            AssertLex("=", KeywordToken(AssignToken));
            AssertLex("true", KeywordToken(TrueKeyword));
            AssertLex("false", KeywordToken(FalseKeyword));
        }

        [TestCategory("Lexer"), TestMethod]
        public void TestWhitespacesLexing()
        {
            AssertLex("  1", Literal("1", 1));
            AssertLex("1  ", Literal("1", 1));
            AssertLex("  1  ", Literal("1", 1));
            AssertLex("  1   +  1  ",
                Literal("1", 1),
                KeywordToken(PlusToken),
                Literal("1", 1)
            );
        }

        [TestCategory("Lexer"), TestMethod]
        public void TestNumericExpressionLexing()
        {
            AssertLex("1+1",
                Literal("1", 1),
                KeywordToken(PlusToken),
                Literal("1", 1)
            );
            AssertLex("314 *    42.05",
                Literal("314", 314),
                KeywordToken(AsteriskToken),
                Literal("42.05", 42.05f)
            );
            AssertLex("1+1*1/1-1",
                Literal("1", 1),
                KeywordToken(PlusToken),
                Literal("1", 1),
                KeywordToken(AsteriskToken),
                Literal("1", 1),
                KeywordToken(SlashToken),
                Literal("1", 1),
                KeywordToken(MinusToken),
                Literal("1", 1)
            );
        }

        [TestCategory("Lexer"), TestMethod]
        public void TestIdentifier()
        {
            AssertLex("a", Identifier("a"));
            AssertLex("main", Identifier("main"));
            AssertLex("foo12", Identifier("foo12"));
            AssertLex("pi_is_314_sure", Identifier("pi_is_314_sure"));
            AssertLex("a + 1",
                Identifier("a"),
                KeywordToken(PlusToken),
                Literal("1", 1)
            );
            AssertLex("1 + a + b*3",
                Literal("1", 1),
                KeywordToken(PlusToken),
                Identifier("a"),
                KeywordToken(PlusToken),
                Identifier("b"),
                KeywordToken(AsteriskToken),
                Literal("3", 3)
            );
        }

        [TestCategory("Lexer"), TestMethod]
        public void TestError()
        {
            AssertLexError("1.16.1");
        }

        [TestCategory("Lexer"), TestMethod]
        public void TestAssign()
        {
            AssertLex("a = 1 + 1",
                Identifier("a"),
                KeywordToken(AssignToken),
                Literal("1", 1),
                KeywordToken(PlusToken),
                Literal("1", 1)
            );
            AssertLex(@"a = 1 + 1
b = a + 1",
                Identifier("a"),
                KeywordToken(AssignToken),
                Literal("1", 1),
                KeywordToken(PlusToken),
                Literal("1", 1),
                Identifier("b"),
                KeywordToken(AssignToken),
                Identifier("a"),
                KeywordToken(PlusToken),
                Literal("1", 1)
            );
        }

        void AssertLex(string code, params SyntaxToken[] tokens)
        {
            CollectionAssert.AreEqual(tokens, Lexer.Lex(code));
        }

        void AssertLexError(string code)
        {
            Assert.ThrowsException<LexerException>(() => Lexer.Lex(code));
        }
    }
}
