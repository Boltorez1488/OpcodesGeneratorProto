using System;
using System.IO;
using System.Linq;

namespace OpcodeGenerator.Proto.Cs {
    public class OpcodeMapper {
        public string Namespace { get; }
        public string Name { get; }
        public string FieldName { get; }

        public OpcodeMapper(string fullName = "Opcodes", string fieldName = "Opcode") {
            FieldName = fieldName;
            if (fullName.Contains('.')) {
                var parts = fullName.Split('.');
                Namespace = string.Join(".", parts.Take(parts.Length - 1));
                Name = parts.Last();
            } else {
                Name = fullName;
            }
        }

        public void WriteEnum(string dir, OpcodesProto op) {
            var sb = new LineBuilder();
            if (!string.IsNullOrEmpty(Namespace)) {
                sb.AppendLine($"namespace {Namespace} {{");
                sb.Push();
            }

            if (op != null && op.Codes.Count != 0) {
                sb.AppendLine($"enum {Name} : {Program.OpcodeTypeCs} {{");
                sb.Push();
                foreach (var kv in op.Codes) {
                    sb.AppendLine($"{kv.Value.Replace('.', '_')} = {kv.Key},");
                }
                sb.Pop();
                sb.AppendLine("}");
            }

            if (!string.IsNullOrEmpty(Namespace)) {
                sb.Pop();
                sb.AppendLine("}");
            }

            string path;
            if (string.IsNullOrEmpty(dir)) {
                path = $"{Name}.cs";
            } else {
                if (!Directory.Exists(dir)) {
                    Directory.CreateDirectory(dir);
                }
                path = Path.Combine(dir, $"{Name}.cs");
            }

            Console.WriteLine($"Write cs map enum file: {path}");
            File.WriteAllText(path, sb.ToString());
        }

        public void WriteClass(string dir, OpcodesProto op) {
            var sb = new LineBuilder();
            foreach (var kv in op.Codes) {
                WriteClass(sb, kv.Value, kv.Key);
                sb.AppendLine("");
            }

            string path;
            if (string.IsNullOrEmpty(dir)) {
                path = $"{Name}.cs";
            } else {
                if (!Directory.Exists(dir)) {
                    Directory.CreateDirectory(dir);
                }
                path = Path.Combine(dir, $"{Name}Map.cs");
            }

            Console.WriteLine($"Write cs map enum file: {path}");
            File.WriteAllText(path, sb.ToString());
        }

        private void WriteClass(LineBuilder sb, string classPath, ulong id) {
            string space = "", name = classPath;
            if (classPath.Contains('.')) {
                var parts = classPath.Split('.');
                space = string.Join(".", parts.Take(parts.Length - 1));
                name = parts.Last();
            }

            if (!string.IsNullOrEmpty(space)) {
                sb.AppendLine($"namespace {space} {{");
                sb.Push();
            }

            sb.AppendLine($"public sealed partial class {name} {{");
            sb.Push();
            sb.AppendLine($"public static readonly {Program.OpcodeTypeCs} {FieldName} = {id};");
            sb.Pop();
            sb.AppendLine("}");

            if (string.IsNullOrEmpty(space)) 
                return;

            sb.Pop();
            sb.AppendLine("}");
        }
    }
}
