# Opcodes Generator
Automatically generates opcodes for Google Protobuf 3 to apply to Client / Server protocols. 

Based on protoc version 3.12.4.0

**Support: C#, C++**

Features:
- Automatic generation, you just need to create proto files and specify all parameters
- Does not destroy past codes
- Can clear meta information from C++ code to make life harder for reversers. But to the detriment of reflection :confused:

## Help
```
opgen.exe <path to actions xml file>

OpcodeType - Global type for all opcodes in C++
OpcodeTypeCs - Global type for all opcodes in C#
ProtoMetaEraser - Path to erase all meta-info in C++ classes
ProtoOpcodes: <array>
        Data - Main db to save/load all opcodes everytime, don't rewrite this file
        FieldName - Field name to place field into classes
        EnumName - Full name to mapper
        Include - Path to proto file[s]
        CppPath - C++ path[es] to *pb files that rebuild it
        CppMapPath - C++ path[es] to create global map enum
        CsClassPath - C# path[es] to create ClassMap
        CsEnumPath - C# path[es] to create Enum
        
Write comment '@skip' before message, that skip it opcode creator
```
