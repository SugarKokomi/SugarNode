using UnityEngine;
using UnityEditor;
using static SugarNode.Editor.NodeEditorSettings;
using System;
namespace SugarNode.Editor
{
    partial class NodeEditorWindow
    {
        const float PPI = 0.001f;//其实是每单位像素对应UV坐标的多少单位\
        const int TitleHeight = 20;
        Rect drawGridRect => new Rect(Vector2.zero, position.size);
        Rect drawGridUV = Rect.zero;
        Rect contentRect => new Rect(0, TitleHeight, position.width, position.height);//Window坐标系下，去掉窗口的标题Title，纯内容的Rect
        /// <summary> 绘制节点编辑器的网格 </summary>
        //这个是因为这个绘制图片的GUI.DrawTextureWithTexCoords()这个逼玩意儿居然不支持负数的Rect.size
        //众所周知UV坐标系是左下角坐标系，EditorWindow是左上角坐标系，然后我又懒得算坐标转换
        //于是干脆直接沿着窗口中心反转Y轴得了，这样左下角的UV坐标系就变成左上角的窗口坐标系了（女少口阝可）
        //另外EditorWindow.position获取的是屏幕坐标系整个窗口的Rect，会把窗口的Title都包含进去
        //改天有缘再改吧（ps.或者头上画一排Info，显示激活NodeGraph的信息，或者部分快捷操作之类的？比如Prefab模式的AutoSave？）
        void DrawGrid()
        {
            //计算绘制网格图片的UV Rect范围
            drawGridUV.position = PositionOffset * -PPI;//左上角Offset开始
            drawGridUV.size = PPI * ScaleOffset * position.size;//* new Vector2(1, -1);//尺寸应用窗口尺寸的比例 ，且应用缩放
            //在缩放模式下绘制网格背景图
            var matrix = GUI.matrix;
            GUI.EndClip();
            var controlRect = GUIUtility.ScreenToGUIRect(position);
            controlRect.yMax += TitleHeight;
            GUIUtility.ScaleAroundPivot(new Vector2(1, -1), controlRect.center);
            GUI.DrawTextureWithTexCoords(drawGridRect, ResourceLoader.GridTexture, drawGridUV);
            //还原被缩放过的GUI变换矩阵和裁剪区域
            GUI.matrix = matrix;
            GUI.BeginClip(contentRect);
        }
        void DrawRightButtonMenu()//绘制鼠标右键的菜单
        {
            GenericMenu menu = new GenericMenu();
            if (!activeGraph)//节点图为空的时候，创建节点图
            {
                var childClass = GetSubclasses(typeof(NodeGraph));
                foreach (var ClassType in childClass)
                {
                    menu.AddItem(new GUIContent($"创建新的{ClassType.Name}"), false, () =>
                    {
                        Debug.Log("敬请期待");
                        //TODO:创建新的对话图等玩意儿在这里，重点是需要打开文件管理器的UI
                    });
                }
            }
            else//否则在激活的节点图上创建节点
            {
                var allowType = NodeGraph.GetAllowNodeTypeCache(activeGraph.GetType());
                foreach (var ClassType in allowType)
                {//TODO:ClassType.Name得换成命名空间+类名，以及用户自定义路径的Attribute
                    menu.AddItem(new GUIContent(ClassType.Name), false, () => AddNodeToActiveGraph(ClassType, TranslateWindowToGrid(mousePosition)));
                }
            }
            menu.AddItem(new GUIContent($"复位移动"), false, () => PositionOffset = Vector2.zero);
            menu.AddItem(new GUIContent($"复位缩放"), false, () => ScaleOffset = 1);
            menu.AddItem(new GUIContent($"全部复位"), true, () =>
            {
                PositionOffset = Vector2.zero;
                ScaleOffset = 1;
            });
            menu.ShowAsContext();
        }
        //绘制节点图
        void DrawNodeGraph()
        {
            if (!activeGraph) return;
            else
            {
                GUI.EndClip();//取消原本GUI的裁剪（缩放后绘制Rect不再和窗口适配了）
                Matrix4x4 originalMatrix = GUI.matrix;
                GUIUtility.ScaleAroundPivot(new Vector2(1 / ScaleOffset, 1 / ScaleOffset), PositionOffset);//基于用户鼠标滚轮调整的缩放画UI
                // 计算缩放后的内容Rect坐标
                Matrix4x4 invert = Matrix4x4.Inverse(GUI.matrix);
                Vector2 scaledContentRectMin = invert.MultiplyPoint(contentRect.position);
                Vector2 scaledContentRectMax = invert.MultiplyPoint(contentRect.max);
                GUI.BeginClip(new Rect(scaledContentRectMin, scaledContentRectMax - scaledContentRectMin));
                //在缩放后的裁剪窗口内画节点图
                foreach (var node in activeGraph.nodes)//循环画每个Node
                {
                    if (!node) continue;
                    DrawNode(node);
                    foreach (var outputPort in node.Outputs)//循环画每个节点的Output Port
                    {
                        foreach (var inputPort in outputPort.Connections)//循环画一个Port连接的每个其他节点的Input Port
                        {
                            DrawPortLine(outputPort, inputPort);
                        }
                    }
                }
                GUI.EndClip();
                GUI.matrix = originalMatrix;//还原GUI的Transform矩阵
                GUI.BeginClip(contentRect);//还原GUI的视窗裁剪
            }
        }
        void DrawNode(Node node)
        {
            //获取节点的Attribute缓存信息，在绘制本节点的时候应用上
            NodeWidthAttribute widthAttri = node.GetAttributeCache<NodeWidthAttribute>();
            NodeColorAttribute colorAttri = node.GetAttributeCache<NodeColorAttribute>();
            //转换空间坐标
            Vector2 windowPos = TranslateGridToWindow(node.position);
            windowPos = TranslateWindowToGUI(windowPos);
            //绘制节点
            Rect scaleWindowRect = new Rect(windowPos, new Vector2(widthAttri?.width ?? 1000, 500));
            GUILayout.BeginArea(scaleWindowRect);//这个玩意儿不需要加这个括号->  {},仅仅是方便查看
            {
                if (colorAttri != null)
                    ResourceLoader.NodeGUIStyle.SetNodeBodyColorOffset(colorAttri.color);
                GUILayout.BeginVertical(ResourceLoader.NodeGUIStyle.nodeBody, GUILayout.Width(scaleWindowRect.width));
                GUILayout.Space(scaleWindowRect.height);//TODO:在此处绘制节点的所有字段等玩意儿
                GUILayout.EndVertical();
            }
            GUILayout.EndArea();
        }
        void DrawPortLine(NodePort output, NodePort input)
        {

        }
        void DrawDragRect()
        {

        }
        void DrawDebugInfo()
        {
            GUILayout.BeginArea(new Rect(control.mousePosition, new Vector2(500, 200)));
            {
                GUI.color = Color.white;
                GUILayout.Label($"鼠标位置：{mousePosition}");
                GUILayout.Label($"鼠标所在网格位置：{TranslateWindowToGrid(mousePosition)}");
                GUILayout.Label($"鼠标距离网格中心的偏移位置：{PositionOffset - mousePosition*ScaleOffset}");
                GUILayout.Label($"网格偏移：{PositionOffset}");
                GUILayout.Label($"网格缩放：{ScaleOffset}");
            }
            GUILayout.EndArea();
            //Repaint();//不要不经过任何操作直接在OnGUI里Repaint(),约等于写死循环
        }
    }
}