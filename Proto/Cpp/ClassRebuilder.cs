using System;
using System.IO;
using System.Text;

namespace OpcodeGenerator.Proto.Cpp {
    public class ClassRebuilder {
        public string Dir { get; }
        public OpcodesProto Op { get; }

        public string FieldName { get; set; } = "Opcode";

        public ClassRebuilder(string dir, OpcodesProto op) {
            Dir = dir;
            Op = op;
        }

        public void Rebuild() {
            var files = Directory.EnumerateFiles(Dir, "*.pb.h", SearchOption.AllDirectories);
            foreach(var f in files) {
                Console.WriteLine($"Rebuild class: {f}");
                Rebuild(f);
            }
        }

        public void Rebuild(string fpath) {
            var sb = new StringBuilder();

            var rebuild = false;
            var flag = false;
            var lines = File.ReadAllLines(fpath);
            var i = 0;
            foreach(var line in lines) {
                i++;
                sb.AppendLine(line);

                const string s = "@@protoc_insertion_point(class_definition:";
                var pos = line.IndexOf(s, StringComparison.Ordinal);
                if (pos == -1)
                    continue;
                var pos2 = line.IndexOf(")", pos + s.Length, StringComparison.Ordinal);
                if (pos2 == -1)
                    continue;
                var path = line.Substring(pos + s.Length, pos2 - pos - s.Length);
                if (lines[i + 1].Contains(FieldName)) {
                    lines[i + 1] = $"  static constexpr {Program.OpcodeType} {FieldName} = {Op.Back[path]};";
                    rebuild = true;
                } else if (Op.Back.ContainsKey(path)) {
                    sb.AppendLine(" public:");
                    sb.AppendLine($"  static constexpr {Program.OpcodeType} {FieldName} = {Op.Back[path]};");
                    flag = true;
                }
            }

            if (rebuild) {
                File.WriteAllLines(fpath, lines);
            } else if (flag) {
                File.WriteAllText(fpath, sb.ToString());
            }
        }
    }
}
