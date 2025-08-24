using System;
using System.Collections.Generic;

namespace HMUMLClassDiagram.Editor
{
    [Serializable]
    public sealed class UmlMethod
    {
        #region Fields
        public string name;
        public UmlAccessModifierType accessModifierType;
        public UmlType returnType;
        public string customReturnTypeName;
        public bool isStatic;
        public bool isAbstract;
        public bool isVirtual;
        public bool isOverride;
        public bool useBase;
        public List<UmlParameter> parameters = new();
        #endregion
    }
}