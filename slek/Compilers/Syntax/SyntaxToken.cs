using System;
using System.Collections.Generic;
using System.Text;

namespace slek.Compilers.Syntax
{
    public partial class SyntaxToken
    {
        public SyntaxTokenType TokenType { get; }

        public int FullWidth { get; set; }

        internal SyntaxToken(SyntaxTokenType type)
        {
            TokenType = type;
            FullWidth = Text.Length;
        }

        protected SyntaxToken(SyntaxTokenType type, int width)
        {
            TokenType = type;
            FullWidth = width;
        }

        public virtual string Text
            => SyntaxFactory.GetText(TokenType);

        public virtual object Value
        {
            get
            {
                switch (TokenType)
                {
                    case SyntaxTokenType.TrueKeyword:
                        return true;
                    case SyntaxTokenType.FalseKeyword:
                        return false;
                    default:
                        return Text;
                }
            }
        }

        internal static SyntaxToken WithValue<T>(SyntaxTokenType type, string text, T value)
            => new SyntaxTokenWithValue<T>(type, text, value);

        internal static SyntaxToken Identifier(string name)
            => new SyntaxIdentifier(SyntaxTokenType.Identifier, name);

        public static bool operator==(SyntaxToken source, SyntaxToken dest)
        {
            if (dest == null)
                return false;
            if (dest.TokenType != source.TokenType)
                return false;
            if (dest.Text != source.Text)
                return false;
            if (dest.Value.GetType() != source.GetType())
                return false;
            if (!dest.Value.Equals(source))
                return false;

            return true;
        }

        public static bool operator !=(SyntaxToken source, SyntaxToken dest)
            => !(source == dest);
    }
}
