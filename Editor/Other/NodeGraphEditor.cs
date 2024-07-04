using UnityEditor;
using UnityEngine;
namespace SugarNode.Editor
{
    [CustomEditor(typeof(NodeGraph), true)]
    public class NodeGraphEditor : UnityEditor.Editor
    {
        [SerializeField]
        protected static bool showBaseInspectorGUI = false;
        public override void OnInspectorGUI()
        {
            DrawGraphEditorTips();
            if (showBaseInspectorGUI)
                base.OnInspectorGUI();
        }
        protected virtual void DrawGraphEditorTips()
        {
            if (GUILayout.Button("在 节 点 编 辑 器 中 打 开", GUILayout.Height(50)))
            {
                NodeEditorWindow.OpenWindow();
                NodeEditorWindow.activeGraph = (NodeGraph)target;
            }
            GUILayout.Label("不建议在此对Nodes进行增减操作，\n会丢失内部的Node资源引用。\n需要增减请打开NodeEditor进行操作。\n");

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("前往GitHub"))
            {
                Application.OpenURL("https://github.com/SugarKokomi/SugarNode/");
            }
            showBaseInspectorGUI = EditorGUILayout.Toggle("预览Nodes", showBaseInspectorGUI);
            EditorGUILayout.EndHorizontal();
        }
    }
}