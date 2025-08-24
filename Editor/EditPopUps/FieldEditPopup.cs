using System;
using HMUMLClassDiagram.Editor.Data;
using HMUMLClassDiagram.Editor.Enums;
using UnityEditor;
using UnityEngine;

namespace HMUMLClassDiagram.Editor.EditPopUps
{
    public sealed class FieldEditPopup : PopupWindowContent
    {
        #region ReadonlyFields
        private readonly UmlField _field;
        private readonly Action _onChanged;
        #endregion

        #region Fields
        private string _name;
        private UmlType _type;
        private string _customTypeName;
        private UmlAccessModifierType _accessModifier;
        #endregion

        #region Constructor
        public FieldEditPopup(UmlField field, Action onChanged)
        {
            _field = field;
            _onChanged = onChanged;
            _name = field.name;
            _type = field.type;
            _customTypeName = field.customTypeName;
            _accessModifier = field.accessModifierType;
        }
        #endregion

        #region Core
        public override Vector2 GetWindowSize() => new(256, 128);
        public override void OnGUI(Rect rect)
        {
            EditorGUILayout.LabelField("Edit Field", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            _name = EditorGUILayout.TextField("Name", _name);
            _accessModifier = (UmlAccessModifierType)EditorGUILayout.EnumPopup("Access", _accessModifier);
            _type = (UmlType)EditorGUILayout.EnumPopup("Type", _type);

            if (_type == UmlType.Custom)
                _customTypeName = EditorGUILayout.TextField("Custom Type", _customTypeName);

            GUILayout.FlexibleSpace();

            EditorGUILayout.BeginHorizontal();
            
            if (GUILayout.Button("Save"))
            {
                ApplyChanges();
                
                editorWindow.Close();
            }

            if (GUILayout.Button("Cancel"))
                editorWindow.Close();
            
            EditorGUILayout.EndHorizontal();
        }
        #endregion

        #region Executes
        private void ApplyChanges()
        {
            _field.name = _name;
            _field.accessModifierType = _accessModifier;
            _field.type = _type;
            _field.customTypeName = _customTypeName;
            _onChanged?.Invoke();
        }
        #endregion
    }
}