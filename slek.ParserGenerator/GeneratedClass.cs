using System.Linq;

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

        public GeneratedClass(int indent, Modifier m, string name, bool isinherited, string inhclass, params ClassField[] fields)
        {
            ClassModifier = m;
            Name = name;
            IsInherited = isinherited;
            InheritedClassName = inhclass;
            Fields = fields;
            BaseIndent = indent;
        }

        public override string ToString()
        {
            var builder = new System.Text.StringBuilder();
            var indent = new string(' ', BaseIndent);
            var indent2 = new string(' ', BaseIndent + 4);
            var indent3 = new string(' ', BaseIndent + 8);

            builder.Append(indent);
            builder.Append($"{ClassModifier} class {Name}");
            if (IsInherited)
                builder.Append($" : {InheritedClassName}");
            builder.Append(indent);
            builder.AppendLine("{");
            
            foreach (var f in Fields)
                builder.AppendLine($"{indent2}{f.Modifier} {f.Type} {f.Name};");

            builder.Append($"public {Name}(");
            
            for (int i = 0; i < Fields.Length - 1; i++)
                builder.Append($"{Fields[i].Type} {Fields[i].Name} ,");
            builder.AppendLine($"{Fields.Last().Type} {Fields.Last().Name})");
            builder.Append(indent2);
            builder.AppendLine("{");

            foreach (var f in Fields)
                builder.AppendLine($"{indent3}this.{f.Name} = {f.Name}");

            builder.Append(indent2);
            builder.AppendLine("}");

            builder.Append(indent);
            builder.AppendLine("}");
            return builder.ToString();
        }

        public class ClassField
        {
            public Modifier Modifier;
            public string Type;
            public string Name;
        }

        public enum Modifier
        {
            @public, @internal, @protected, @private
        }
    }
}
