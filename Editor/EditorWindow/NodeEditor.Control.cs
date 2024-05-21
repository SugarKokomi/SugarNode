using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static SugarNode.Editor.NodeEditorSettings;

namespace SugarNode.Editor
{
    partial class NodeEditorWindow
    {
        private Event control => Event.current;
        private Vector2 mousePosition = Vector2.zero;//缓存记录鼠标位置
        private bool isDragging = false;//是否正在拖拽鼠标
        private Vector2 dragStartPos = Vector2.zero;//缓存记录拖拽的起点
        private Rect selectionRect => new Rect(dragStartPos, mousePosition - dragStartPos);
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
                    switch (control.type)
                    {
                        case EventType.MouseDown:
                            dragStartPos = mousePosition;
                            break;
                        case EventType.MouseDrag:
                            isDragging = true;
                            ComputeSelectNodes();
                            break;
                        case EventType.MouseUp:
                            isDragging = false;
                            ComputeSelectNodes();
                            break;
                        default: break;
                    }
                    control.Use();
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
                    if (selectionCache.Count >= 1)//TODO:看向选择的节点
                        LookAtNode(selectionCache.FirstOrDefault());
                    else
                        LookAtPoint(Vector2.zero);
                    control.Use();
                }
                else if (control.keyCode == KeyCode.Delete)
                {
                    //TODO:检查选择缓存，删除选择的节点
                }
            }
        }
        /// <summary> 在网格坐标系中，看向某个位置 </summary>
        private void LookAtPoint(Vector2 pos)
        {
            var windowPos = TranslateGridToScreen(pos);//转换到窗口空间
            windowPos = position.size / 2 - windowPos;//得到窗口空间下，目标点指向窗口中心的向量
            PositionOffset += windowPos * ScaleOffset;//将缩放后的偏移向量加在坐标偏移向量里
        }
        /// <summary> 看向一个Node </summary>
        private void LookAtNode(Node node)
        {
            uint x_offset_GUI = node.GetNodeWidth();//GUI坐标系中的宽度
            float x_offset = PPI * x_offset_GUI / 2;//网格坐标系中，偏移节点宽度的一半（高度姑且先偏移网格的0.5格）
            Vector2 offsetPoint = node.position + new Vector2(x_offset, 0.5f);
            LookAtPoint(offsetPoint);
        }
        /// <summary> 基于Unity的Project窗口选择的NodeGraph改变时，重新绘制节点图 </summary>
        private void OnSelectionChanged()
        {
            if (Selection.activeObject is NodeGraph nodeGraph)
                activeGraph = nodeGraph;
        }
        #region 资源操作
        /// <summary> 将节点添加到激活的节点图中 </summary>
        /// <param name="newNodeType">节点类型</param>
        /// <param name="nodePosition">Grid坐标系中的位置</param>
        private void AddNodeToActiveGraph(Type newNodeType, Vector2 nodePosition)
        {
            var newNode = activeGraph.AddNode(newNodeType);
            newNode.position = nodePosition;//control.mousePosition;不知道为什么访问不到为null捏~
            AssetDatabase.AddObjectToAsset(newNode, activeGraph);
            AssetDatabase.SaveAssets();
            Repaint();
        }
        /// <summary> 删掉一个节点 </summary>
        private void DelectNodeInActiveGraph(Node delectObj)
        {
            activeGraph.RemoveNode(delectObj);//从节点列表移除
            AssetDatabase.RemoveObjectFromAsset(delectObj);//从文件中删除资源
            AssetDatabase.SaveAssets();
            Destroy(delectObj);//删掉文件
        }
        private Node CopyNode(Node node)
        {
            return default;
        }
        #endregion
        private void ComputeSelectNodes()
        {
            var seletRect = TranslateScreenToGridRect(selectionRect);
            selectionCache = activeGraph.nodes.Where(
                node =>seletRect.Overlaps(node.GetNodeRectInGridSpace()))//在Grid空间下检查框选范围的选择节点
                .ToHashSet();
        }
    }
}