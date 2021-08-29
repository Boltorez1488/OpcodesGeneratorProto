using OpcodeGenerator.Actions;
using System;

namespace OpcodeGenerator {
    internal class Program {
        public static string OpcodeType = "unsigned long";
        public static string OpcodeTypeCs = "ulong";

        private static void Main(string[] args) {
            if (args.Length < 1) {
                Console.WriteLine("Google Protobuf 3 - Opcodes generator by @boltorez1488");
                Console.WriteLine("Help: use opgen.exe <path_to_actions>");
                Console.WriteLine("");
                Console.WriteLine("OpcodeType - Global type for all opcodes in C++");
                Console.WriteLine("OpcodeTypeCs - Global type for all opcodes in C#");

                Console.WriteLine("ProtoMetaEraser - Path to erase all meta-info in C++ classes");

                Console.WriteLine("ProtoOpcodes: <array>");
                Console.WriteLine("\tData - Main db to save/load all opcodes everytime, don't rewrite this file");
                Console.WriteLine("\tFieldName - Field name to place field into classes");
                Console.WriteLine("\tEnumName - Full name to mapper");
                Console.WriteLine("\tInclude - Path to proto file[s]");

                Console.WriteLine("\tCppPath - C++ path[es] to *pb files that rebuild it");
                Console.WriteLine("\tCppMapPath - C++ path[es] to create global map enum");

                Console.WriteLine("\tCsClassPath - C# path[es] to create ClassMap");
                Console.WriteLine("\tCsEnumPath - C# path[es] to create Enum");
                return;
            }

            Core.LoadActions(args[0]);
        }
    }
}
