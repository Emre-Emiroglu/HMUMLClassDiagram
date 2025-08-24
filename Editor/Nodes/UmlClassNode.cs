using System;
using HMUMLClassDiagram.Editor.Data;
using UnityEngine;

namespace HMUMLClassDiagram.Editor.Nodes
{
    [Serializable]
    public sealed class UmlClassNode
    {
        #region Fields
        public UmlClassData classData;
        public Rect rect;
        public bool isSelected;
        #endregion

        #region Constructor
        public UmlClassNode(UmlClassData data, Vector2 position)
        {
            classData = data;
            rect = new Rect(position.x, position.y, 256, 128);
            isSelected = false;
        }
        #endregion
    }
}