using UnityEngine;
using UnityEditor;
//using System.Reflection;
namespace SugarNode.Editor
{
    partial class NodeEditorWindow
    {
        const float PPI = 0.005f;//其实是每单位像素对应UV坐标的多少单位\
        const int TitleHeight = 20;
        bool debugMode = false;
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
                        //TODO:创建新的对话图等玩意儿在这里，重点是需要打开文件管理器的UI（或者直接不要这个功能，删了这个else）
                    });
                }
            }
            else//否则在激活的节点图上创建节点
            {
                var allowBaseType = NodeGraph.GetAllowNodeTypeCache(activeGraph.GetType());//这里返回的是允许的所有基类Node（因为OnlyAllowNode可以不止写一个）
                foreach (var parentClass in allowBaseType)//对于每个NodeGraph允许的基类
                {
                    var childClasses = GetSubclasses(parentClass);
                    foreach (var childClass in childClasses)//对于每个具体能实例化的类
                    {
                        menu.AddItem(new GUIContent(GetRightMenuPath(childClass)),
                        false,
                        () => AddNodeToActiveGraph(childClass, TranslateScreenToGrid(mousePosition)));
                    }
                }
            }
            menu.AddItem(new GUIContent($"复位移动和缩放"), false, () =>
            {
                PositionOffset = Vector2.zero;
                ScaleOffset = 2;
            });
            menu.AddItem(new GUIContent("Debug模式"), debugMode, () => debugMode = !debugMode);
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
                GUIUtility.ScaleAroundPivot(new Vector2(1 / ScaleOffset, 1 / ScaleOffset), PositionOffset);//基于用户鼠标滚轮拖动过的网格坐标原点缩放画UI
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
            Rect nodeGUIRect = TranslateGridToGUIRect(node.GetNodeRectInGridSpace());//获取节点在网格空间的Rect，并转换到GUI坐标系下

            bool thisNodewasSelected = selectionCache.Contains(node);
            if (thisNodewasSelected)//如果被选择了，还需要绘制选择框
                GUILayout.BeginVertical(ResourceLoader.NodeGUIStyle.nodeHeight, GUILayout.Width(nodeGUIRect.width));
            {//绘制节点本体
                GUILayout.BeginArea(nodeGUIRect);//这个玩意儿不需要加这个括号->  {},仅仅是方便查看
                {
                    GUILayout.BeginVertical(ResourceLoader.NodeGUIStyle.nodeBody, GUILayout.Width(nodeGUIRect.width));//绘制节点背景
                    EditorGUILayout.LabelField(node.GetNodeTitle());
                    node.OnNodeEditorGUI();//绘制节点的数据部分
                    GUILayout.EndVertical();
                }
                GUILayout.EndArea();
            }
            if (thisNodewasSelected) GUILayout.EndVertical();

        }
        void DrawPortLine(NodePort output, NodePort input)
        {

        }
        void DrawDragRect()
        {
            GUI.Box(selectionRect, "");
        }
        void DrawDebugInfo()
        {
            GUILayout.BeginArea(new Rect(control.mousePosition, new Vector2(500, 200)));
            {
                GUI.color = Color.white;
                GUILayout.Label($"鼠标位置(屏幕坐标)：{mousePosition}");
                GUILayout.Label($"鼠标位置(网格坐标)：{TranslateScreenToGrid(mousePosition)}");
                GUILayout.Label($"鼠标与网格原点偏移：{PositionOffset - mousePosition * ScaleOffset}");
                GUILayout.Label($"网格偏移：{PositionOffset}");
                GUILayout.Label($"网格缩放：{ScaleOffset}");
            }
            GUILayout.EndArea();
        }
    }
}