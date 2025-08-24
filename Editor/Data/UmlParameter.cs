using System;
using HMUMLClassDiagram.Editor.Enums;

namespace HMUMLClassDiagram.Editor.Data
{
    [Serializable]
    public sealed class UmlParameter
    {
        #region Fields
        public string name;
        public UmlType type;
        public string customTypeName;
        #endregion
    }
}