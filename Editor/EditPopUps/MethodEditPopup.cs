using System;
using System.Collections.Generic;
using HMUMLClassDiagram.Editor.Data;
using HMUMLClassDiagram.Editor.Enums;
using UnityEditor;
using UnityEngine;

namespace HMUMLClassDiagram.Editor.EditPopUps
{
    public sealed class MethodEditPopup : PopupWindowContent
    {
        #region ReadonlyFields
        private readonly UmlMethod _method;
        private readonly Action _onChanged;
        private readonly List<UmlParameter> _parameters;
        #endregion

        #region Fields
        private string _name;
        private UmlAccessModifierType _accessModifier;
        private UmlType _returnType;
        private string _customReturnTypeName;
        private bool _isStatic;
        private bool _isAbstract;
        private bool _isVirtual;
        private bool _isOverride;
        private bool _useBase;
        #endregion

        #region Constructor
        public MethodEditPopup(UmlMethod method, Action onChanged)
        {
            _method = method;
            _onChanged = onChanged;
            _parameters = new List<UmlParameter>(method.parameters);
            _name = method.name;
            _accessModifier = method.accessModifierType;
            _returnType = method.returnType;
            _customReturnTypeName = method.customReturnTypeName;
            _isStatic = method.isStatic;
            _isAbstract = method.isAbstract;
            _isVirtual = method.isVirtual;
            _isOverride = method.isOverride;
            _useBase = method.useBase;
        }
        #endregion

        #region Core
        public override Vector2 GetWindowSize() => new(256, 384);
        public override void OnGUI(Rect rect)
        {
            EditorGUILayout.LabelField("Edit Method", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            _name = EditorGUILayout.TextField("Name", _name);
            _accessModifier = (UmlAccessModifierType)EditorGUILayout.EnumPopup("Access", _accessModifier);

            _returnType = (UmlType)EditorGUILayout.EnumPopup("Return Type", _returnType);
            if (_returnType == UmlType.Custom)
                _customReturnTypeName = EditorGUILayout.TextField("Custom Return Type", _customReturnTypeName);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Flags", EditorStyles.boldLabel);
            _isStatic = EditorGUILayout.Toggle("Static", _isStatic);
            _isAbstract = EditorGUILayout.Toggle("Abstract", _isAbstract);
            _isVirtual = EditorGUILayout.Toggle("Virtual", _isVirtual);
            _isOverride = EditorGUILayout.Toggle("Override", _isOverride);
            _useBase = EditorGUILayout.Toggle("Use Base", _useBase);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Parameters", EditorStyles.boldLabel);

            for (int i = 0; i < _parameters.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                _parameters[i].name = EditorGUILayout.TextField(_parameters[i].name);
                _parameters[i].type = (UmlType)EditorGUILayout.EnumPopup(_parameters[i].type, GUILayout.Width(96));

                if (_parameters[i].type == UmlType.Custom)
                    _parameters[i].customTypeName = EditorGUILayout.TextField(_parameters[i].customTypeName);

                if (GUILayout.Button("X", GUILayout.Width(32)))
                {
                    _parameters.RemoveAt(i);
                    GUI.changed = true;
                    break;
                }

                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Add Parameter"))
            {
                _parameters.Add(new UmlParameter
                {
                    name = "param",
                    type = UmlType.Int
                });
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.Space();

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
            _method.name = _name;
            _method.accessModifierType = _accessModifier;
            _method.parameters = new List<UmlParameter>(_parameters);
            _method.returnType = _returnType;
            _method.customReturnTypeName = _customReturnTypeName;
            _method.isStatic = _isStatic;
            _method.isAbstract = _isAbstract;
            _method.isVirtual = _isVirtual;
            _method.isOverride = _isOverride;
            _method.useBase = _useBase;
            _onChanged?.Invoke();
        }
        #endregion
        
    }
}