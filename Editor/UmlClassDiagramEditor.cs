using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace HMUMLClassDiagram.Editor
{
    public sealed class UmlClassDiagramEditor : EditorWindow
    {
        #region ReadonlyFields
        private readonly List<UmlClassNode> _classNodes = new();
        private readonly List<UmlRelationshipNode> _relationshipNodes = new();
        #endregion
        
        #region Fields
        private Vector2 _drag;
        private Vector2 _offset;
        private Vector2 _contextClickPosition;
        private UmlClassNode _selectedNode;
        private bool _isDraggingNode;
        #endregion

        #region Core
        [MenuItem("Tools/HMUMLClassDiagram/UmlClassDiagramEditor")]
        private static void OpenWindow() => GetWindow<UmlClassDiagramEditor>("Uml Class Diagram Editor");
        private void OnGUI()
        {
            DrawGrid(32, .25f, Color.gray);
            DrawGrid(128, .5f, Color.gray);

            DrawClassNodes();
            DrawRelationshipNodes();
            ProcessEvents(Event.current);

            if (GUI.changed)
                Repaint();
        }
        #endregion

        #region DrawGrid
        private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
        {
            int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
            int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

            Handles.BeginGUI();
            Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

            _offset += _drag * 0.5f;
            Vector3 newOffset = new(_offset.x % gridSpacing, _offset.y % gridSpacing, 0);

            for (int i = 0; i < widthDivs; i++)
                Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset,
                    new Vector3(gridSpacing * i, position.height, 0f) + newOffset);

            for (int j = 0; j < heightDivs; j++)
                Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset,
                    new Vector3(position.width, gridSpacing * j, 0f) + newOffset);

            Handles.color = Color.white;
            Handles.EndGUI();
        }
        #endregion

        #region DrawClassNodes
        private void DrawClassNodes()
        {
            GUIStyle nodeStyle = GetClassNodeStyle();
            
            foreach (UmlClassNode classNode in _classNodes)
            {
                if (!classNode.classData)
                    continue;

                float drawY = UmlClassNodeDrawHeader(classNode, nodeStyle);

                drawY = UmlClassNodeDrawFields(classNode, drawY);

                drawY = UmlClassNodeDrawProperties(classNode, drawY);

                UmlClassNodeDrawMethods(classNode, drawY);

                GUI.EndGroup();
            }
        }
        private static GUIStyle GetClassNodeStyle()
        {
            GUIStyle nodeStyle = new GUIStyle(GUI.skin.box)
            {
                normal =
                {
                    background = Texture2D.whiteTexture,
                    textColor =  Color.black
                },
                alignment = TextAnchor.UpperCenter,
                fontStyle = FontStyle.Bold
            };
            
            return nodeStyle;
        }
        private static float UmlClassNodeDrawHeader(UmlClassNode classNode, GUIStyle nodeStyle)
        {
            float headerHeight = UmlClassNodeSetupHeaderHeight(classNode, nodeStyle, out var drawY);

            drawY = UmlClassNodeDrawNamespace(classNode, drawY);
            
            drawY = UmlClassNodeDrawClassTypeIcon(classNode, drawY);

            UmlClassNodeDrawClassName(classNode, drawY);

            return headerHeight; 
        }
        private static float UmlClassNodeSetupHeaderHeight(UmlClassNode classNode, GUIStyle nodeStyle, out float drawY)
        {
            float y = 32;
            
            if (classNode.classData.fields.Count > 0)
                y += classNode.classData.fields.Count * 18 + 2;
            if (classNode.classData.properties.Count > 0)
                y += classNode.classData.properties.Count * 18 + 2;
            if (classNode.classData.methods.Count > 0)
                y += classNode.classData.methods.Count * 18 + 2;

            classNode.rect.height = y + 5;

            GUI.BeginGroup(classNode.rect, nodeStyle);

            float headerHeight = 40;
            
            Rect headerRect = new Rect(0, 0, classNode.rect.width, headerHeight);
            
            GUI.Box(headerRect, GUIContent.none); 

            drawY = 2;
            
            return headerHeight;
        }
        private static float UmlClassNodeDrawNamespace(UmlClassNode classNode, float drawY)
        {
            if (string.IsNullOrEmpty(classNode.classData.namespaceName))
                return drawY;
            
            GUIStyle nsStyle = new GUIStyle(EditorStyles.label)
            {
                fontSize = 9,
                fontStyle = FontStyle.Italic,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = Color.gray }
            };
            
            Rect nsRect = new Rect(0, drawY, classNode.rect.width, 14);
            
            GUI.Label(nsRect, classNode.classData.namespaceName, nsStyle);
            
            drawY += 14;

            return drawY;
        }
        private static float UmlClassNodeDrawClassTypeIcon(UmlClassNode classNode, float drawY)
        {
            Texture icon;
            string typeLabel = null;

            if (classNode.classData.isInterface)
            {
                icon = EditorGUIUtility.IconContent("d_Interface Icon").image;
                typeLabel = "«interface»";
            }
            else if (classNode.classData.isAbstract)
            {
                icon = EditorGUIUtility.IconContent("d_ScriptableObject Icon").image;
                typeLabel = "«abstract»";
            }
            else
            {
                icon = EditorGUIUtility.IconContent("cs Script Icon").image;
            }

            if (icon)
            {
                Rect iconRect = new Rect(5, drawY, 16, 16);
                
                GUI.DrawTexture(iconRect, icon, ScaleMode.ScaleToFit);
            }

            if (string.IsNullOrEmpty(typeLabel))
                return drawY;
            
            GUIStyle typeStyle = new GUIStyle(EditorStyles.label)
            {
                fontSize = 10,
                fontStyle = FontStyle.Italic,
                alignment = TextAnchor.UpperLeft,
                normal = { textColor = Color.black }
            };
            
            Rect typeRect = new Rect(24, drawY, classNode.rect.width - 24, 16);
            
            GUI.Label(typeRect, typeLabel, typeStyle);
            
            drawY += 16;

            return drawY;
        }
        private static void UmlClassNodeDrawClassName(UmlClassNode classNode, float drawY)
        {
            GUIStyle nameStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 12,
                normal = { textColor = Color.black }
            };
            
            Rect nameRect = new Rect(0, drawY, classNode.rect.width, 18);
            
            GUI.Label(nameRect, classNode.classData.className, nameStyle);
        }
        private float UmlClassNodeDrawFields(UmlClassNode classNode, float drawY)
        {
            if (classNode.classData.fields.Count <= 0)
                return drawY;
            
            DrawHorizontalLine(drawY);
            
            drawY += 2;

            foreach (UmlField field in classNode.classData.fields)
            {
                Rect fieldRect = new Rect(5, drawY, classNode.rect.width - 10, 16);
                
                GUI.Label(fieldRect,
                    $"{field.accessModifierType.ToString().ToLower()} {field.name} : {GetTypeString(field.type, field.customTypeName)}");
                
                drawY += 18;
            }

            return drawY;
        }
        private float UmlClassNodeDrawProperties(UmlClassNode classNode, float drawY)
        {
            if (classNode.classData.properties.Count <= 0)
                return drawY;
            
            DrawHorizontalLine(drawY);
            
            drawY += 2;

            foreach (UmlProperty prop in classNode.classData.properties)
            {
                Rect propRect = new Rect(5, drawY, classNode.rect.width - 10, 16);
                
                GUI.Label(propRect,
                    $"{prop.accessModifierType.ToString().ToLower()} {prop.name} : {GetTypeString(prop.type, prop.customTypeName)} {{get; set;}}");
                
                drawY += 18;
            }

            return drawY;
        }
        private void UmlClassNodeDrawMethods(UmlClassNode classNode, float drawY)
        {
            if (classNode.classData.methods.Count <= 0)
                return;
            
            DrawHorizontalLine(drawY);
            
            drawY += 2;

            foreach (UmlMethod method in classNode.classData.methods)
            {
                Rect methodRect = new Rect(5, drawY, classNode.rect.width - 10, 16);
                
                GUI.Label(methodRect,
                    $"{method.accessModifierType.ToString().ToLower()} {GetTypeString(method.returnType, method.customReturnTypeName)} {method.name}()");
                
                drawY += 18;
            }
        }
        private void DrawHorizontalLine(float y)
        {
            Handles.BeginGUI();
            Handles.color = Color.black;
            Handles.DrawLine(new Vector3(0, y, 0), new Vector3(position.width, y, 0));
            Handles.color = Color.white;
            Handles.EndGUI();
        }
        #endregion

        #region DrawRelationshipNodes
        private void DrawRelationshipNodes()
        {
            foreach (UmlRelationshipNode relationshipNode in _relationshipNodes)
                Handles.DrawLine(relationshipNode.GetSourcePosition(), relationshipNode.GetTargetPosition());
        }
        #endregion

        #region Inputs
        private void ProcessEvents(Event e)
        {
            _drag = Vector2.zero;

            switch (e.type)
            {
                case EventType.MouseDown:
                    HandleMouseDown(e);
                    break;
                case EventType.MouseUp:
                    HandleMouseUp(e);
                    break;
                case EventType.MouseDrag:
                    HandleMouseDrag(e);
                    break;
                case EventType.ContextClick:
                    HandleContextClick(e);
                    break;
            }
        }
        private void HandleMouseDown(Event e)
        {
            if (e.button != 0)
                return;
            
            _selectedNode = GetNodeAtPosition(e.mousePosition);
                        
            if (_selectedNode != null)
            {
                _isDraggingNode = true;
                            
                GUI.changed = true;
            }
            else
                _selectedNode = null;
        }
        private void HandleMouseUp(Event e)
        {
            if (e.button != 0 || !_isDraggingNode)
                return;
            
            _isDraggingNode = false;
        }
        private void HandleMouseDrag(Event e)
        {
            if (e.button != 0)
                return;
            
            if (_isDraggingNode && _selectedNode != null)
            {
                _selectedNode.rect.position += e.delta;
                            
                GUI.changed = true;
            }
            else
            {
                _drag = e.delta;

                foreach (UmlClassNode classNode in _classNodes)
                    classNode.rect.position += e.delta;

                GUI.changed = true;
            }
        }
        private void HandleContextClick(Event e)
        {
            _contextClickPosition = e.mousePosition;

            UmlClassNode clickedNode = GetNodeAtPosition(e.mousePosition);

            if (clickedNode != null)
                ShowNodeContextMenu(clickedNode);
            else
                ShowGlobalContextMenu();

            e.Use();
        }
        private UmlClassNode GetNodeAtPosition(Vector2 pos) =>
            _classNodes.FirstOrDefault(classNode => classNode.rect.Contains(pos));
        private void ShowNodeContextMenu(UmlClassNode node)
        {
            GenericMenu menu = new GenericMenu();

            menu.AddItem(new GUIContent("Edit/Set Class Name"), false, () =>
            {
                Vector2 screenPos = GUIUtility.GUIToScreenPoint(_contextClickPosition);
                Rect rect = new Rect(screenPos.x, screenPos.y, 0, 0);

                PopupWindow.Show(rect, new TextInputPopup("Class Name", node.classData.className, newValue =>
                {
                    node.classData.className = newValue;
                    GUI.changed = true;
                }));
            });

            menu.AddItem(new GUIContent("Edit/Set Namespace"), false, () =>
            {
                Vector2 screenPos = GUIUtility.GUIToScreenPoint(_contextClickPosition);
                Rect rect = new Rect(screenPos.x, screenPos.y, 0, 0);

                PopupWindow.Show(rect, new TextInputPopup("Namespace", node.classData.namespaceName, newValue =>
                {
                    node.classData.namespaceName = newValue;
                    GUI.changed = true;
                }));
            });

            menu.AddSeparator("Edit/");
            
            menu.AddItem(new GUIContent("Edit/Toggle Abstract"), false, () => OnClickToggleAbstract(node));
            menu.AddItem(new GUIContent("Edit/Toggle Interface"), false, () => OnClickToggleInterface(node));
            
            menu.AddSeparator("");
            
            menu.AddItem(new GUIContent("Delete Node"), false, () => OnClickDeleteNode(node));

            menu.ShowAsContext();
        }
        private void ShowGlobalContextMenu()
        {
            GenericMenu menu = new GenericMenu();
            
            menu.AddItem(new GUIContent("Add Class Node"), false, OnClickAddClassNode);
            menu.ShowAsContext();
        }
        private void OnClickAddClassNode()
        {
            UmlClassData umlClassData = CreateInstance<UmlClassData>();
            umlClassData.className = "New Class";

            UmlClassNode newNode = new(umlClassData, _contextClickPosition);
            _classNodes.Add(newNode);

            GUI.changed = true;
        }
        #endregion

        #region InputCallbacks
        private static void OnClickToggleAbstract(UmlClassNode node)
        {
            node.classData.isAbstract = !node.classData.isAbstract;
            
            if (node.classData.isAbstract)
                node.classData.isInterface = false;
            
            GUI.changed = true;
        }
        private static void OnClickToggleInterface(UmlClassNode node)
        {
            node.classData.isInterface = !node.classData.isInterface;
            
            if (node.classData.isInterface)
                node.classData.isAbstract = false;
            
            GUI.changed = true;
        }
        private void OnClickDeleteNode(UmlClassNode node)
        {
            _classNodes.Remove(node);
            
            GUI.changed = true;
        }
        #endregion
        
        #region HelperUtilities
        private static string GetTypeString(UmlType type, string customType)
        {
            return type switch
            {
                UmlType.Int => "int",
                UmlType.Float => "float",
                UmlType.Bool => "bool",
                UmlType.String => "string",
                UmlType.Void => "void",
                UmlType.Custom => string.IsNullOrEmpty(customType) ? "object" : customType,
                _ => "object"
            };
        }
        #endregion
    }
}