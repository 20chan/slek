using System.Diagnostics;

namespace slek.ParserGenerator
{
    class Node
    { }

    [DebuggerDisplay("{ToString()}")]
    class Or : Node
    {
        public Node L, R;
        public Or(Node l, Node r)
        {
            L = l;
            R = r;
        }

        public override string ToString()
            => $"{L} | {R}";
    }

    [DebuggerDisplay("{ToString()}")]
    class Maybe : Node
    {
        public Node Node;
        public Maybe(Node node)
        {
            Node = node;
        }

        public override string ToString()
            => $"{Node}?";
    }

    [DebuggerDisplay("{ToString()}")]
    class Repeat : Node
    {
        public Node Node;
        public Repeat(Node node)
        {
            Node = node;
        }

        public override string ToString()
            => $"{Node}*";
    }

    [DebuggerDisplay("{ToString()}")]
    class Sequence : Node
    {
        public Node[] Nodes;
        public Sequence(Node[] nodes)
        {
            Nodes = nodes;
        }

        public override string ToString()
            => $"({string.Join<Node>(' ', Nodes)})";
    }

    [DebuggerDisplay("{ToString()}")]
    class Define : Node
    {
        public string Name;
        public Node Value;
        public Define(string name, Node value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString()
            => $"{Name} := {Value}";
    }

    [DebuggerDisplay("{ToString()}")]
    class Value : Node
    {
        public string Name;
        public Value(string name)
        {
            Name = name;
        }

        public override string ToString()
            => Name;
    }
}
