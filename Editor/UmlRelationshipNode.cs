using System;
using UnityEngine;

namespace HMUMLClassDiagram.Editor
{
    [Serializable]
    public sealed class UmlRelationshipNode
    {
        #region Fields
        public UmlRelationshipData relationshipData;
        public UmlClassNode sourceNode;
        public UmlClassNode targetNode;
        public bool isSelected;
        #endregion

        #region Constructor
        public UmlRelationshipNode(UmlRelationshipData data, UmlClassNode source, UmlClassNode target)
        {
            relationshipData = data;
            sourceNode = source;
            targetNode = target;
            isSelected = false;
        }
        #endregion

        #region Executes
        public Vector2 GetSourcePosition() => new(sourceNode.rect.xMax, sourceNode.rect.center.y);
        public Vector2 GetTargetPosition() => new(targetNode.rect.xMin, targetNode.rect.center.y);
        #endregion
    }
}