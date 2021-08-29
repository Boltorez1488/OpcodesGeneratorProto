using System;
using System.IO;
using System.Text;

namespace OpcodeGenerator.Proto.Cpp {
    public class MetaEraser {
        public static void Process(string dir) {
            var files = Directory.EnumerateFiles(dir, "*.pb.cc", SearchOption.AllDirectories);
            foreach (var f in files) {
                Console.WriteLine($"Clear meta-info: {f}");
                Clear(f);
            }
        }

        public static void Clear(string fpath) {
            var sb = new StringBuilder();
            var lines = File.ReadAllLines(fpath);
            var enableSkip = false;
            foreach(var line in lines) {
                if (!enableSkip) {
                    if (line.Contains("descriptor_table_protodef_")) {
                        var pos = line.IndexOf('"');
                        if (pos != -1) {
                            var pos2 = line.IndexOf('"', pos + 1);
                            if (pos2 != -1) {
                                sb.AppendLine(line.Remove(pos + 1, pos2 - (pos + 1)));
                                continue;
                            }
                        } else {
                            sb.AppendLine(line);
                            enableSkip = true;
                            sb.AppendLine("\"\"");
                            continue;
                        }
                    }
                    sb.AppendLine(line);
                } else {
                    if (!line.Contains(";")) 
                        continue;
                    enableSkip = false; 
                    sb.AppendLine(line);
                }
            }

            File.WriteAllText(fpath, sb.ToString());
        }
    }
}
