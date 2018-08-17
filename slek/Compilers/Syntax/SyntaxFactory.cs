﻿using System;
using System.Collections.Generic;
using System.Text;

namespace slek.Compilers.Syntax
{
    public static class SyntaxFactory
    {
        private static SyntaxTokenType FirstToken = SyntaxTokenType.KnownKeywordStart;
        private static SyntaxTokenType LastToken = SyntaxTokenType.KnownKeywordEnd;
        private static SyntaxToken[] _tokens;

        static SyntaxFactory()
        {
            _tokens = new SyntaxToken[LastToken - FirstToken + 1];

            for (var typ = FirstToken; typ <= LastToken; typ++)
            {
                _tokens[typ - FirstToken] = new SyntaxToken(typ);
            }
        }

        public static string GetText(SyntaxTokenType type)
        {
            switch (type)
            {
                case SyntaxTokenType.PlusToken:
                    return "+";
                case SyntaxTokenType.MinusToken:
                    return "-";
                case SyntaxTokenType.AsteriskToken:
                    return "*";
                case SyntaxTokenType.SlashToken:
                    return "/";
                case SyntaxTokenType.PercentToken:
                    return "%";
                case SyntaxTokenType.ConcatToken:
                    return "++";
                case SyntaxTokenType.GreaterToken:
                    return ">";
                case SyntaxTokenType.LessToken:
                    return "<";
                case SyntaxTokenType.GreaterEqualToken:
                    return ">=";
                case SyntaxTokenType.LessEqualToken:
                    return "<=";
                case SyntaxTokenType.EqualToken:
                    return "==";
                case SyntaxTokenType.NotEqualToken:
                    return "!=";
                case SyntaxTokenType.LShiftToken:
                    return "<<";
                case SyntaxTokenType.RShiftToken:
                    return ">>";
                case SyntaxTokenType.ExclamationToken:
                    return "!";
                case SyntaxTokenType.TildeToken:
                    return "~";
                case SyntaxTokenType.VBarToken:
                    return "|";
                case SyntaxTokenType.AmperToken:
                    return "&";
                case SyntaxTokenType.DoubleVBarToken:
                    return "||";
                case SyntaxTokenType.DoubleAmperToken:
                    return "&&";
                case SyntaxTokenType.CaretToken:
                    return "^";
                case SyntaxTokenType.AssignToken:
                    return "=";
                case SyntaxTokenType.LParenToken:
                    return "(";
                case SyntaxTokenType.RParenToken:
                    return ")";
                case SyntaxTokenType.LBraceToken:
                    return "{";
                case SyntaxTokenType.RBraceToken:
                    return "}";
                case SyntaxTokenType.TrueKeyword:
                    return "true";
                case SyntaxTokenType.FalseKeyword:
                    return "false";
                case SyntaxTokenType.WildcardKeyword:
                    return "_";
                case SyntaxTokenType.EOF:
                    return "\0";

                case SyntaxTokenType.ImportKeyword:
                    return "import";
                case SyntaxTokenType.BreakKeyword:
                    return "break";
                    case SyntaxTokenType.ContinueKeyword:
                    return "continue";
                case SyntaxTokenType.ReturnKeyword:
                    return "return";
                case SyntaxTokenType.ModuleKeyword:
                    return "module";
                case SyntaxTokenType.StructKeyword:
                    return "struct";
                case SyntaxTokenType.FnKeyword:
                    return "fn";
                case SyntaxTokenType.VarKeyword:
                    return "var";
                case SyntaxTokenType.IfKeyword:
                    return "if";
                case SyntaxTokenType.WhileKeyword:
                    return "while";
                case SyntaxTokenType.ForKeyword:
                    return "for";
                default:
                    return string.Empty;
            }
        }

        public static SyntaxToken KeywordToken(SyntaxTokenType type)
            => _tokens[type - FirstToken];

        public static SyntaxToken Literal(string text, int value)
            => SyntaxToken.WithValue(SyntaxTokenType.IntegerLiteral, text, value);

        public static SyntaxToken Literal(string text, float value)
            => SyntaxToken.WithValue(SyntaxTokenType.RealLiteral, text, value);

        public static SyntaxToken Literal(string value)
            => SyntaxToken.WithValue(SyntaxTokenType.StringLiteral, value, value);

        public static SyntaxToken Identifier(string name)
            => SyntaxToken.Identifier(name);
    }
}
