using System;

namespace HMUMLClassDiagram.Editor
{
    [Serializable]
    public class UmlProperty
    {
        public string name;
        public UmlAccessModifier accessModifier;
        public UmlType type;
        public string customTypeName;
        public bool hasGetter = true;
        public bool hasSetter = true;
    }
}