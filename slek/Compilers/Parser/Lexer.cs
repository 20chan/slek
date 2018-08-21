using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using slek.Compilers.Exceptions;
using slek.Compilers.Syntax;

using static slek.Compilers.Syntax.SyntaxFactory;
using static slek.Compilers.Syntax.SyntaxTokenType;

namespace slek.Compilers.Parser
{
    public class Lexer
    {
        readonly string Code;
        int idx;
        char Peek => Code[idx];
        char Pop() => Code[idx++];
        bool IsEOF => idx == Code.Length || IsWhitespacesOnlyLeft();

        public Lexer(string code)
        {
            Code = code;
            idx = 0;
        }

        public static SyntaxToken[] Lex(string code)
        {
            return LexIterator().ToArray();

            IEnumerable<SyntaxToken> LexIterator()
            {
                var lexer = new Lexer(code);
                while (!lexer.IsEOF)
                    yield return lexer.LexOne();
            }
        }

        private bool IsWhitespacesOnlyLeft()
        {
            if (Code.Length == idx) return true;
            for (int i = idx; i < Code.Length; i++)
            {
                if (!char.IsWhiteSpace(Code, i))
                    return false;
            }
            return true;
        }

        private void Error()
            => throw new LexerException();

        SyntaxToken LexOne()
        {
            if (IsEOF)
                Error();
            switch (Peek)
            {
                case '+':
                case '-':
                case '*':
                case '/':
                case '%':
                case '>':
                case '<':
                case '=':
                case '!':
                case '~':
                case '|':
                case '&':
                case '^':
                case '(':
                case ')':
                    return LexKeyword();
                case ' ':
                case '\t':
                case '\r':
                case '\n':
                    Pop();
                    return LexOne();
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    return LexNumeric();
                case '"':
                    return LexString();
                default:
                    if (char.IsLetterOrDigit(Peek) || Peek == '_')
                        return LexIdentifier();
                    else
                        Error();
                    return null;
            }
        }

        private SyntaxToken LexKeyword()
        {
            var top = Pop();
            switch (top)
            {
                case '+':
                    if (!IsEOF && Peek == '+')
                    {
                        Pop();
                        return KeywordToken(ConcatToken);
                    }
                    else return KeywordToken(PlusToken);
                case '>':
                    if (!IsEOF && Peek == '=')
                    {
                        Pop();
                        return KeywordToken(GreaterEqualToken);
                    }
                    else if (!IsEOF && Peek == '>')
                    {
                        Pop();
                        return KeywordToken(RShiftToken);
                    }
                    else return KeywordToken(GreaterToken);
                case '<':
                    if (!IsEOF && Peek == '=')
                    {
                        Pop();
                        return KeywordToken(LessEqualToken);
                    }
                    else if (!IsEOF && Peek == '<')
                    {
                        Pop();
                        return KeywordToken(LShiftToken);
                    }
                    else return KeywordToken(LessToken);
                case '|':
                    if (!IsEOF && Peek == '|')
                    {
                        Pop();
                        return KeywordToken(DoubleVBarToken);
                    }
                    else return KeywordToken(VBarToken);
                case '&':
                    if (!IsEOF && Peek == '&')
                    {
                        Pop();
                        return KeywordToken(DoubleAmperToken);
                    }
                    else return KeywordToken(AmperToken);
                case '=':
                    if (!IsEOF && Peek == '=')
                    {
                        Pop();
                        return KeywordToken(EqualToken);
                    }
                    else return KeywordToken(AssignToken);
                case '!':
                    if (!IsEOF && Peek == '=')
                    {
                        Pop();
                        return KeywordToken(NotEqualToken);
                    }
                    else return KeywordToken(ExclamationToken);
                case '-':
                    return KeywordToken(MinusToken);
                case '*':
                    return KeywordToken(AsteriskToken);
                case '/':
                    return KeywordToken(SlashToken);
                case '%':
                    return KeywordToken(PercentToken);
                case '~':
                    return KeywordToken(TildeToken);
                case '^':
                    return KeywordToken(CaretToken);
                case '(':
                    return KeywordToken(LParenToken);
                case ')':
                    return KeywordToken(RParenToken);
                case '{':
                    return KeywordToken(LBraceToken);
                case '}':
                    return KeywordToken(RBraceToken);
                default:
                    throw new LexerException();
            }
        }

        private SyntaxToken LexNumeric()
        {
            int start = idx;
            while (!IsEOF)
            {
                if (Peek == '.')
                    return LexRealFromDot(start);
                if (!char.IsNumber(Peek))
                    break;

                Pop();
            }
            int len = idx - start;
            var text = Code.Substring(start, len);
            return Literal(text, Convert.ToInt32(text));
        }

        private SyntaxToken LexRealFromDot(int startIdx)
        {
            Pop(); // .
            if (IsEOF)
                Error();
            if (!char.IsNumber(Pop()))
                Error();

            while (!IsEOF)
            {
                if (!char.IsNumber(Peek))
                    break;
                Pop();
            }
            int len = idx - startIdx;
            var text = Code.Substring(startIdx, len);
            return Literal(text, Convert.ToSingle(text));
        }

        private SyntaxToken LexString()
        {
            Pop();
            var result = new StringBuilder();
            while (!IsEOF && Peek != '"')
            {
                var cur = Pop();
                if (cur == '\\')
                {
                    var unescaped = Pop();
                    if (unescaped == 'n')
                        result.Append('\n');
                    else if (unescaped == 'r')
                        result.Append('\r');
                    else if (unescaped == 't')
                        result.Append('\t');
                    else if (unescaped == '\\')
                        result.Append('\\');
                    else if (unescaped == '"')
                        result.Append('"');
                    else if (unescaped == '0')
                        result.Append('\0');
                    else
                        throw new LexerException();
                }
                else if (cur == '\r' || cur == '\n')
                    throw new LexerException();
                else
                    result.Append(cur);
            }
            Pop();
            return Literal(result.ToString());
        }

        private SyntaxToken LexIdentifier()
        {
            int start = idx;
            while (!IsEOF)
            {
                if (Peek != '_' && !char.IsLetterOrDigit(Peek))
                    break;

                Pop();
            }
            int len = idx - start;
            var text = Code.Substring(start, len);
            if (text == "true")
                return KeywordToken(TrueKeyword);
            else if (text == "false")
                return KeywordToken(FalseKeyword);
            else if (text == "_")
                return KeywordToken(WildcardKeyword);
            else
                return Identifier(text);
        }
    }
}
