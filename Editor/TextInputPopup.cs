using System;
using UnityEditor;
using UnityEngine;

namespace HMUMLClassDiagram.Editor
{
    public sealed class TextInputPopup : PopupWindowContent
    {
        #region ReadonlyFields
        private readonly string _title;
        private readonly Action<string> _onSubmit;
        #endregion
        
        #region Fields
        private string _value;
        #endregion

        #region Constructor
        public TextInputPopup(string title, string initialValue, Action<string> onSubmit)
        {
            _title = title;
            _value = initialValue;
            _onSubmit = onSubmit;
        }
        #endregion

        #region Core
        public override Vector2 GetWindowSize() => new(256, 64);
        public override void OnGUI(Rect rect)
        {
            GUILayout.Label(_title, EditorStyles.boldLabel);
                
            _value = EditorGUILayout.TextField(_value);

            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            
            if (GUILayout.Button("OK"))
            {
                _onSubmit?.Invoke(_value);
                
                editorWindow.Close();
            }
            if (GUILayout.Button("Cancel"))
            {
                editorWindow.Close();
            }
            
            GUILayout.EndHorizontal();
        }
        #endregion
    }
}