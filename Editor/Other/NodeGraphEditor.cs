using UnityEditor;
using UnityEngine;
namespace SugarNode.Editor
{
    [CustomEditor(typeof(NodeGraph),true)]
    public class NodeGraphEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("在 节 点 编 辑 器 中 打 开", GUILayout.Height(50)))
            {
                NodeEditorWindow.OpenWindow();
                NodeEditorWindow.activeGraph = (NodeGraph)target;
            }
            base.OnInspectorGUI();
        }
    }
}