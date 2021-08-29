using OpcodeGenerator.Proto.Cpp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OpcodeGenerator.Actions {
    public static class Core {
        public static void LoadActions(string xmlPath = "actions.xml") {
            if (!File.Exists(xmlPath)) {
                Console.WriteLine($"{xmlPath} not found");
                return;
            }

            var xDoc = XDocument.Load(xmlPath);
            if (xDoc.Root.Name.LocalName != "Actions") {
                Console.WriteLine($"{xmlPath} is not actions file");
                return;
            }

            var metaList = new List<string>();
            var protoList = new List<ProtoOpcodes>();
            foreach(var el in xDoc.Root.Elements()) {
                switch(el.Name.LocalName) {
                    case "OpcodeType":
                        if (el.Value.Length > 0)
                            Program.OpcodeType = el.Value;
                        break;
                    case "OpcodeTypeCs":
                        if (el.Value.Length > 0)
                            Program.OpcodeTypeCs = el.Value;
                        break;
                    case "ProtoOpcodes":
                        var proto = new ProtoOpcodes();
                        proto.Read(el);
                        protoList.Add(proto);
                        break;
                    case "ProtoMetaEraser":
                        if (el.Value.Length > 0)
                            metaList.Add(el.Value);
                        break;
                }
            }

            foreach(var proto in protoList) {
                proto.Process();
            }

            foreach(var meta in metaList) {
                MetaEraser.Process(meta);
            }
        }
    }
}
