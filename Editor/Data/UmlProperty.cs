using System;
using HMUMLClassDiagram.Editor.Enums;

namespace HMUMLClassDiagram.Editor.Data
{
    [Serializable]
    public sealed class UmlProperty
    {
        #region Fields
        public string name;
        public UmlAccessModifierType accessModifierType;
        public UmlType type;
        public string customTypeName;
        public bool hasGetter = true;
        public bool hasSetter = true;
        #endregion
    }
}