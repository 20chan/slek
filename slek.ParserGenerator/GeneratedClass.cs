using System.Linq;
using System.Text;

namespace slek.ParserGenerator
{
    public sealed class GeneratedClass
    {
        public Modifier ClassModifier { get; }
        public string Name { get; }
        public bool IsInherited { get; }
        public string InheritedClassName { get; }
        public ClassField[] Fields { get; }

        public int BaseIndent { get; set; }

        readonly string indent1, indent2, indent3, indent4, indent5;

        public GeneratedClass(int indent, Modifier m, string name, bool isinherited, string inhclass, params ClassField[] fields)
        {
            ClassModifier = m;
            Name = name;
            IsInherited = isinherited;
            InheritedClassName = inhclass;
            Fields = fields;
            BaseIndent = indent;

            indent1 = new string(' ', BaseIndent);
            indent2 = new string(' ', BaseIndent + 4);
            indent3 = new string(' ', BaseIndent + 8);
            indent4 = new string(' ', BaseIndent + 12);
            indent5 = new string(' ', BaseIndent + 16);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append(indent1);
            builder.Append($"{ClassModifier} class {Name}");
            if (IsInherited)
                builder.Append($" : {InheritedClassName}");
            builder.AppendLine();
            builder.Append(indent1);
            builder.AppendLine("{");

            AppendFields(builder);
            AppendConstructor(builder);
            AppendEquals(builder);

            builder.Append(indent1);
            builder.AppendLine("}");
            return builder.ToString();
        }

        void AppendFields(StringBuilder sb)
        {
            foreach (var f in Fields)
                sb.AppendLine($"{indent2}{f.Modifier} {f.Type} {f.Name};");
        }

        void AppendConstructor(StringBuilder sb)
        {
            sb.Append($"{indent2}public {Name}(");

            for (int i = 0; i < Fields.Length - 1; i++)
                sb.Append($"{Fields[i].Type} {Fields[i].Name}, ");
            sb.AppendLine($"{Fields.Last().Type} {Fields.Last().Name})");
            sb.Append(indent2);
            sb.AppendLine("{");

            foreach (var f in Fields)
                sb.AppendLine($"{indent3}this.{f.Name} = {f.Name};");

            sb.Append(indent2);
            sb.AppendLine("}");
        }

        void AppendEquals(StringBuilder sb)
        {
            var returnfalse = $"{indent4}return false;";

            sb.AppendLine($"{indent2}public override bool Equals(object obj)");
            sb.AppendLine($"{indent2}{{");
            sb.AppendLine($"{indent3}var node = obj as {Name};");
            sb.AppendLine($"{indent3}if (node == null)");
            sb.AppendLine(returnfalse);
            sb.AppendLine($"{indent3}if (this.GetType() != node.GetType())");
            sb.AppendLine(returnfalse);
            foreach (var m in Fields)
            {
                sb.AppendLine($"{indent3}if (!(this.{m.Name}.Equals(node.{m.Name})))");
                sb.AppendLine(returnfalse);
            }
            sb.AppendLine($"{indent3}return true;");
            sb.AppendLine($"{indent2}}}");
        }

        public class ClassField
        {
            public Modifier Modifier;
            public string Type;
            public string Name;

            public ClassField(Modifier m, string type, string name)
            {
                Modifier = m;
                Type = type;
                Name = name;
            }
        }

        public enum Modifier
        {
            @public, @internal, @protected, @private
        }
    }
}
