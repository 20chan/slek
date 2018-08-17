namespace slek.Compilers.Syntax
{
    public enum SyntaxTokenType
    {
        None = 0,
        IntegerLiteral,
        RealLiteral,
        StringLiteral,

        KnownKeywordStart,

        PlusToken = KnownKeywordStart, // +
        MinusToken, // -
        AsteriskToken, // *
        SlashToken, // /
        PercentToken, // %
        ConcatToken, // ++
        GreaterToken, // >
        LessToken, // <
        GreaterEqualToken, // >=
        LessEqualToken, // <=
        EqualToken, // ==
        NotEqualToken, // !=
        LShiftToken, // <<
        RShiftToken, // >>
        ExclamationToken, // !
        TildeToken, // ~
        VBarToken, // |
        AmperToken, // &
        DoubleVBarToken, // ||
        DoubleAmperToken, // &&
        CaretToken, // ^
        AssignToken, // =
        LParenToken, // (
        RParenToken, // )
        LBraceToken, // {
        RBraceToken, // }
        TrueKeyword, // true
        FalseKeyword, // false
        WildcardKeyword, // _

        ImportKeyword,
        BreakKeyword,
        ContinueKeyword,
        ReturnKeyword,
        ModuleKeyword,
        StructKeyword,
        FnKeyword,
        VarKeyword,
        IfKeyword,
        WhileKeyword,
        ForKeyword,

        EOF,
        KnownKeywordEnd = EOF,
        Identifier,
    }
}
