using System;

namespace slek.Compilers.Exceptions
{
    public class LexerException : Exception
    {
        public LexerException(string msg = "") : base(msg)
        {

        }
    }
}
