using System;

namespace HMUMLClassDiagram.Editor
{
    [Serializable]
    public sealed class UmlField
    {
        #region Fields
        public string name;
        public UmlAccessModifierType accessModifierType;
        public UmlType type;
        public string customTypeName;
        public bool isStatic;
        public bool isReadonly;
        public bool isConst;
        #endregion
    }
}