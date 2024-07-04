using UnityEngine;
using UnityEditor;
using static SugarNode.Editor.NodeEditorSettings;
using System.Collections.Generic;
//using System.Reflection;
//[assembly: AssemblyVersion("0.1")]
namespace SugarNode.Editor
{
    internal partial class NodeEditorWindow : EditorWindow
    {
        private static NodeEditorWindow m_instence;
        /// <summary> 单例实例 </summary>
        public static NodeEditorWindow Instence
        {
            get
            {
                if (m_instence == null)
                {
                    m_instence = EditorWindow.GetWindow(typeof(NodeEditorWindow)) as NodeEditorWindow;
                    m_instence.titleContent.text = "节点编辑器";
                }
                return m_instence;
            }
        }
        private Vector2 m_positionOffset = Vector2.zero;
        /// <summary> 网格偏移 </summary>
        public Vector2 PositionOffset
        {
            get => m_positionOffset;
            set => m_positionOffset = value;
        }
        private float m_scaleOffset = 5f;
        /// <summary> 网格缩放系数 </summary>
        public float ScaleOffset
        {
            get => m_scaleOffset;
            set => m_scaleOffset = Mathf.Clamp(value, Setting.scaleRangeMin, Setting.scaleRangeMax);
        }
        private static NodeGraph m_activeGraph;
        /// <summary> 当前正在绘制的NodeGraph图 </summary>
        public static NodeGraph activeGraph
        {
            get => m_activeGraph;
            set { m_activeGraph = value; Instence.Repaint(); }
        }
        private HashSet<Node> selectionCache = new HashSet<Node>();
        //----------------------Function-------------------------------------------------
        [MenuItem("Window/打开SugarNodeEditor窗口")]
        public static void OpenWindow()
        {
            ResourceLoader.ComputeGridTexture2D();
            Instence.Show();
        }
        void OnGUI()
        {
            ComputeUserControl();//计算用户的键鼠操作
            DrawGrid();//绘制网格            
            DrawNodeGraph();//绘制节点图
            if(isDragging && !isDraggingNode) 
                DrawDragRect();
            if (debugMode)
                DrawDebugInfo();//开发过程Debug专用
        }
        void OnEnable()
        {
            //注册鼠标选中物体的改变函数
            Selection.selectionChanged += OnSelectionChanged;
        }
        void OnDisable()
        {
            Selection.selectionChanged -= OnSelectionChanged;
        }

    }


}
