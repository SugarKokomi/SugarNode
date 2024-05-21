using UnityEngine;
using UnityEditor;
using static SugarNode.Editor.NodeEditorSettings;
namespace SugarNode.Editor
{
    internal partial class NodeEditorWindow : EditorWindow
    {
        private Vector2 TranslateScreenToGrid(Vector2 pos)
        {
            pos *= ScaleOffset;
            pos -= PositionOffset;
            return pos * PPI;
        }
        private Vector2 TranslateGridToScreen(Vector2 pos)
        {
            pos /= PPI;
            pos += PositionOffset;
            pos /= ScaleOffset;
            return pos;
        }
        private Rect TranslateGridToScreenRect(Rect rect)
        {
            rect.position = TranslateGridToScreen(rect.position);
            rect.size /= PPI;
            return rect;
        }
        private Rect TranslateScreenToGridRect(Rect rect)
        {
            rect.position = TranslateScreenToGrid(rect.position);
            rect.size *= PPI;
            return rect;
        }
        //使用此函数需要确保没有更改GUI.matrix,要不然计算出来是缩放之后的坐标系的UI
        private Vector2 TranslateWindowToGUI(Vector2 pos)
        {
            pos += position.position;//从Window转换到Screen坐标
            pos.y += TitleHeight;//去掉标题高度
            return GUIUtility.ScreenToGUIPoint(pos);
        }
        private Rect TranslateGridToGUIRect(Rect rect)
        {
            rect = TranslateGridToScreenRect(rect);//从网格转换到屏幕坐标
            rect.position += position.position + new Vector2(0, TitleHeight);//从Window转换到Screen坐标
            return GUIUtility.ScreenToGUIRect(rect);
        }
    }
}