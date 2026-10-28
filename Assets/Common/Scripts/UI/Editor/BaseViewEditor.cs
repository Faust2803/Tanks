using UI;
using UnityEditor;

namespace Common.Scripts.UI.Editor
{
    [CustomEditor(typeof(BaseView), true)]
    public class BaseViewEditor : UnityEditor.Editor
    {
        private bool _animationPanelExpanded = true;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // -------------------------
            // Animation Panel
            // -------------------------

            _animationPanelExpanded = EditorGUILayout.Foldout(
                _animationPanelExpanded,
                "Base Panel Propertyes",
                true
            );

            if (_animationPanelExpanded)
            {
                EditorGUI.indentLevel++;

                DrawProperty("_animationPanel");
                DrawProperty("_openAnimation");
                DrawProperty("_closeAnimation");
                DrawProperty("_deleteAfterClose");
                DrawProperty("_openCloseDuration");
                DrawProperty("_closeButton");
                DrawProperty("_cameraOff");

                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space(5);

            // -------------------------
            // Остальные поля
            // -------------------------

            DrawPropertiesExcluding(
                serializedObject,
                "_animationPanel",
                "_openAnimation",
                "_closeAnimation",
                "_deleteAfterClose",
                "_openCloseDuration",
                "_closeButton",
                "_cameraOff"
            );

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawProperty(string propertyName)
        {
            SerializedProperty property =
                serializedObject.FindProperty(propertyName);

            if (property != null)
            {
                EditorGUILayout.PropertyField(
                    property,
                    true
                );
            }
        }
    }
}