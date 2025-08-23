using System.Collections.Generic;
using UnityEngine;

namespace HMUMLClassDiagram.Editor
{
    [CreateAssetMenu(fileName = "NewUMLClassData", menuName = "Tools/HMUMLClassDiagram/UMLClassData")]
    public class UmlClassData : ScriptableObject
    {
        public string className;
        public string namespaceName;
        public bool isAbstract;
        public bool isInterface;

        public List<UmlField> fields = new();
        public List<UmlProperty> properties = new();
        public List<UmlMethod> methods = new();
    }
}