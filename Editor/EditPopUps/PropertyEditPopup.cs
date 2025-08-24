using System;
using HMUMLClassDiagram.Editor.Data;
using HMUMLClassDiagram.Editor.Enums;
using UnityEditor;
using UnityEngine;

namespace HMUMLClassDiagram.Editor.EditPopUps
{
    public sealed class PropertyEditPopup : PopupWindowContent
    {
        #region ReadonlyFields
        private readonly UmlProperty _property;
        private readonly  Action _onChanged;
        #endregion

        #region Fields
        private string _name;
        private UmlAccessModifierType _accessModifier;
        private UmlType _type;
        private string _customTypeName;
        private bool _hasGetter;
        private bool _hasSetter;
        #endregion

        #region Constructor
        public PropertyEditPopup(UmlProperty property, Action onChanged)
        {
            _property = property;
            _onChanged = onChanged;
            _name = property.name;
            _accessModifier = property.accessModifierType;
            _type = property.type;
            _customTypeName = property.customTypeName;
            _hasGetter = property.hasGetter;
            _hasSetter = property.hasSetter;
        }
        #endregion

        #region Core
        public override Vector2 GetWindowSize() => new(256, 192);
        public override void OnGUI(Rect rect)
        {
            EditorGUILayout.LabelField("Edit Property", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            _name = EditorGUILayout.TextField("Name", _name);
            _accessModifier = (UmlAccessModifierType)EditorGUILayout.EnumPopup("Access", _accessModifier);
            _type = (UmlType)EditorGUILayout.EnumPopup("Type", _type);

            if (_type == UmlType.Custom)
                _customTypeName = EditorGUILayout.TextField("Custom Type", _customTypeName);

            _hasGetter = EditorGUILayout.Toggle("Has Getter", _hasGetter);
            _hasSetter = EditorGUILayout.Toggle("Has Setter", _hasSetter);

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
            _property.name = _name;
            _property.accessModifierType = _accessModifier;
            _property.type = _type;
            _property.customTypeName = _customTypeName;
            _property.hasGetter = _hasGetter;
            _property.hasSetter = _hasSetter;
            _onChanged?.Invoke();
        }
        #endregion
    }
}