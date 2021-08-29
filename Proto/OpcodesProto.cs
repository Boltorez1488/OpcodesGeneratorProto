using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using SilentOrbit.ProtocolBuffers;

namespace OpcodeGenerator.Proto {
    public class OpcodesProto {
        public ulong Id = 1;
        public Dictionary<ulong, string> Codes = new Dictionary<ulong, string>();
        public Dictionary<string, ulong> Back = new Dictionary<string, ulong>();
        
        public void Add(string path) {
            if (Back.ContainsKey(path))
                return;
            var id = Id++;
            Codes[id] = path;
            Back[path] = id;
        }

        public void ParseProto(params string[] protoPaths) {
            var parser = new FileParser();
            var data = parser.Import(protoPaths);

            foreach (var msg in data.Messages) {
                if (msg.Value.Comments.Contains("@skip"))
                    continue;
                Add(msg.Value.FullProtoName);
            }
        }

        public void LoadData(string dataPath) {
            if (!File.Exists(dataPath)) {
                return;
            }

            var xDoc = XDocument.Load(dataPath);
            if (xDoc.Root?.Name.LocalName != "OpcodesData")
                return;

            ulong max = 1;
            foreach(var el in xDoc.Root.Elements()) {
                if (el.Name.LocalName != "Opcode")
                    continue;
                var idAttr = el.Attribute("Id");
                if (idAttr == null)
                    continue;
                var id = ulong.Parse(idAttr.Value);
                var path = el.Value;

                Codes[id] = path;
                Back[path] = id;

                if (id >= max)
                    max = id + 1;
            }

            Id = max;
        }

        public void SaveData(string dataPath) {
            var xDoc = new XDocument();
            var root = new XElement("OpcodesData");
            foreach(var kv in Codes) {
                var op = new XElement("Opcode", kv.Value);
                op.Add(new XAttribute("Id", kv.Key));
                root.Add(op);
            }
            xDoc.Add(root);
            xDoc.Save(dataPath);
        }
    }
}