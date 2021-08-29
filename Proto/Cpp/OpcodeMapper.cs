using System;
using System.IO;
using System.Linq;

namespace OpcodeGenerator.Proto.Cpp {
    public class OpcodeMapper {
        public string Namespace { get; }
        public string Name { get; }

        public OpcodeMapper(string fullName = "Opcodes") {
            if (fullName.Contains('.')) {
                var parts = fullName.Split('.');
                Namespace = string.Join("::", parts.Take(parts.Length - 1));
                Name = parts.Last();
            } else {
                Name = fullName;
            }
        }

        public void Write(string dir, OpcodesProto op) {
            var sb = new LineBuilder();
            sb.AppendLine("#pragma once");
            //sb.AppendLine("#include <map>");
            //sb.AppendLine("#include <google/protobuf/message.h>");
            sb.AppendLine("");

            if (!string.IsNullOrEmpty(Namespace)) {
                sb.AppendLine($"namespace {Namespace} {{");
                sb.Push();
            }

            if (op != null && op.Codes.Count != 0) {
                sb.AppendLine($"enum {Name} : {Program.OpcodeType} {{");
                sb.Push();
                foreach(var kv in op.Codes) {
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
                path = $"{Name}.h";
            } else {
                if (!Directory.Exists(dir)) {
                    Directory.CreateDirectory(dir);
                }
                path = Path.Combine(dir, $"{Name}.h");
            }

            Console.WriteLine($"Write cpp map file: {path}");
            File.WriteAllText(path, sb.ToString());
        }
    }
}
