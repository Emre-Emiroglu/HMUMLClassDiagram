using System;

namespace HMUMLClassDiagram.Editor
{
    [Serializable]
    public class UmlParameter
    {
        public string name;
        public UmlType type;
        public string customTypeName;
    }
}