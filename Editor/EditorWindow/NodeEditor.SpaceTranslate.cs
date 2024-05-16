using UnityEngine;
using UnityEditor;
using static SugarNode.Editor.NodeEditorSettings;
namespace SugarNode.Editor
{
    internal partial class NodeEditorWindow : EditorWindow
    {
        private Vector2 TranslateWindowToGrid(Vector2 pos)
        {
            pos *= ScaleOffset;
            pos -= PositionOffset;
            return pos * PPI;
        }
        private Vector2 TranslateGridToWindow(Vector2 pos)
        {
            pos /= PPI;
            pos += PositionOffset;
            pos /= ScaleOffset;
            return pos;
        }
        //使用此函数需要确保没有更改GUI.matrix,要不然计算出来是缩放之后的坐标系的UI
        private Vector2 TranslateWindowToGUI(Vector2 pos)
        {
            pos += position.position;//从Window转换到Screen坐标
            pos.y += TitleHeight;//去掉标题高度
            return GUIUtility.ScreenToGUIPoint(pos);
        }
    }
}