using System;
using UnityEditor;
using UnityEngine;
using static SugarNode.Editor.NodeEditorSettings;

namespace SugarNode.Editor
{
    partial class NodeEditorWindow
    {
        private Event control => Event.current;
        private Vector2 mousePosition = Vector2.zero;
        private void ComputeUserControl()
        {
            mousePosition = control.mousePosition;//因为匿名函数的原因所以绕路
            if (control.isMouse)
            {//计算鼠标拖拽和移动事件
                if (control.button == 2)//鼠标中键拖拽
                {
                    PositionOffset += ScaleOffset * Setting.moveSpeed * control.delta;//乘ScaleOffset是因为鼠标的实际操控应该匹配网格坐标而非屏幕坐标
                    control.Use();
                }
                else if (control.button == 1)//TODO:鼠标右键菜单
                {
                    DrawRightButtonMenu();
                }
                else if (control.button == 0)//计算鼠标左键点击节点还是端口
                {

                }
            }
            else if (control.isScrollWheel)
            {//LOOKME:缩放要稍微麻烦一点，因为要沿着鼠标中心缩放，故每次缩放都要计算网格位置
                Vector2 offset = PositionOffset - mousePosition * ScaleOffset;//记录缩放之前的鼠标指向网格原点的向量offset
                ScaleOffset += control.delta.y * Setting.scaleSpeed / 3;//缩放网格,除以3是因为鼠标滚轮的时候，control.delta.y固定传递3的数值，仅仅是为了匹配Setting的缩放数值
                PositionOffset = mousePosition * ScaleOffset + offset;//将鼠标向量加回去
                control.Use();
            }
            else if (control.isKey)
            {
                if (control.keyCode == KeyCode.F && control.type == EventType.KeyDown)
                {
                    /* if (activeGraph && activeGraph.nodes.Count >= 1)//TODO:看向选择的节点
                        LookAtPoint(TranslateGridToWindow(activeGraph.nodes[0].position));
                    else LookAtPoint(Vector2.zero); */
                    LookAtPoint(Vector2.zero);
                    control.Use();
                }
                else if (control.keyCode == KeyCode.Delete)
                {
                    //TODO:检查选择缓存，删除选择的节点
                }
            }
        }
        /// <summary>
        /// 在网格坐标系中，看向某个位置
        /// </summary>
        /// <param name="pos"></param>
        private void LookAtPoint(Vector2 pos)
        {
            //ScaleOffset = 5;
            var windowPos = TranslateGridToWindow(pos);
            PositionOffset = windowPos;// + position.size / 2;
        }
        private void OnSelectionChanged()
        {
            if (Selection.activeObject is NodeGraph nodeGraph)
                activeGraph = nodeGraph;
        }
        private void AddNodeToActiveGraph(Type newNodeType, Vector2 nodePosition)
        {
            var newNode = activeGraph.AddNode(newNodeType);
            newNode.position = nodePosition;//control.mousePosition;不知道为什么访问不到为null捏~
            AssetDatabase.AddObjectToAsset(newNode, activeGraph);
            AssetDatabase.SaveAssets();
            Repaint();
        }
        private void DelectNodeInActiveGraph(Node delectObj)
        {
            activeGraph.RemoveNode(delectObj);//从节点列表移除
            AssetDatabase.RemoveObjectFromAsset(delectObj);//从文件中删除资源
            Destroy(delectObj);//删掉文件
        }
        private Node CopyNode(Node node)
        {
            return default;
        }
    }
}