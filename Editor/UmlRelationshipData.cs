using UnityEngine;

namespace HMUMLClassDiagram.Editor
{
    [CreateAssetMenu(fileName = "NewUMLRelationshipData", menuName = "Tools/HMUMLClassDiagram/UMLRelationshipData")]
    public sealed class UmlRelationshipData : ScriptableObject
    {
        #region Fields
        public UmlRelationship relationship;
        #endregion
    }
}