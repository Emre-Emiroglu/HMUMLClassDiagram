using System;
using HMUMLClassDiagram.Editor.Enums;

namespace HMUMLClassDiagram.Editor.Data
{
    [Serializable]
    public sealed class UmlRelationship
    {
        #region Fields
        public UmlClassData sourceClass;
        public UmlClassData targetClass;
        public UmlRelationshipType relationshipType;
        public string sourceMultiplicity;
        public string targetMultiplicity;
        public bool isBidirectional;
        #endregion
    }
}