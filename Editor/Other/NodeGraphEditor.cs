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
            GUILayout.Label("不建议在此对Nodes进行增减操作，\n会丢失内部的Node资源引用。\n需要增减请打开NodeEditor进行操作。");
            base.OnInspectorGUI();
        }
    }
}