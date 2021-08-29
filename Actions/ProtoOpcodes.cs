using OpcodeGenerator.Proto;
using OpcodeGenerator.Proto.Cpp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace OpcodeGenerator.Actions {
    public class ProtoOpcodes {
        public List<string> Includes = new List<string>();
        public List<string> CppPathes = new List<string>();
        public List<string> CppMapPathes = new List<string>();

        public List<string> CsClassPathes = new List<string>();
        public List<string> CsEnumPathes = new List<string>();

        public string FieldName = "Opcode";
        public string EnumName = "Opcodes";

        public string Data = "opcodes.xdb";

        public void Read(XElement root) {
            foreach(var el in root.Elements()) {
                switch(el.Name.LocalName) {
                    case "Data":
                        if (el.Value.Length > 0)
                            Data = el.Value;
                        break;
                    case "Include":
                        if (el.Value.Length > 0) {
                            if (el.Value.EndsWith("*.proto")) {
                                var dir = Path.GetDirectoryName(el.Value);
                                if (dir == null)
                                    break;
                                Includes.AddRange(Directory.EnumerateFiles(dir, "*.proto", SearchOption.AllDirectories));
                            } else {
                                Includes.Add(el.Value);
                            }
                        }
                        break;
                    case "FieldName":
                        if (el.Value.Length > 0)
                            FieldName = el.Value;
                        break;
                    case "EnumName":
                        if (el.Value.Length > 0)
                            EnumName = el.Value;
                        break;
                    case "CsClassPath":
                        if (el.Value.Length > 0)
                            CsClassPathes.Add(el.Value);
                        break;
                    case "CsEnumPath":
                        if (el.Value.Length > 0)
                            CsEnumPathes.Add(el.Value);
                        break;
                    case "CppPath":
                        if (el.Value.Length > 0)
                            CppPathes.Add(el.Value);
                        break;
                    case "CppMapPath":
                        if (el.Value.Length > 0)
                            CppMapPathes.Add(el.Value);
                        break;
                }
            }
        }

        public void Process() {
            if (Includes.Count == 0) {
                Console.WriteLine("Includes is empty");
                return;
            }

            var op = new OpcodesProto();
            op.LoadData(Data);
            op.ParseProto(Includes.ToArray());
            op.SaveData(Data);

            if (CppMapPathes.Count > 0) {
                var mapper = new OpcodeMapper(EnumName);
                foreach (var path in CppMapPathes) {
                    if (!Directory.Exists(path))
                        continue;
                    mapper.Write(path, op);
                }
            }

            if (CppPathes.Count > 0) {
                foreach (var path in CppPathes) {
                    if (!Directory.Exists(path))
                        continue;
                    var rebuild = new ClassRebuilder(path, op);
                    rebuild.Rebuild();
                }
            }

            if (CsClassPathes.Count > 0) {
                var mapper = new Proto.Cs.OpcodeMapper(EnumName, FieldName);
                foreach (var path in CsClassPathes) {
                    if (!Directory.Exists(path))
                        continue;
                    mapper.WriteClass(path, op);
                }
            }

            if (CsEnumPathes.Count > 0) {
                var mapper = new Proto.Cs.OpcodeMapper(EnumName, FieldName);
                foreach (var path in CsEnumPathes) {
                    if (!Directory.Exists(path))
                        continue;
                    mapper.WriteEnum(path, op);
                }
            }
        }
    }
}
