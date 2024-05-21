using UnityEditor;
using UnityEngine;
using System.Linq;
using static SugarNode.Editor.NodeEditorSettings;
namespace SugarNode.Editor
{
    internal static class ResourceLoader
    {
        private static Texture2D m_nodeBody;
        /// <summary> 节点自身主要贴图 </summary>
        public static Texture2D NodeBodyTexture { get { if (m_nodeBody != null) return m_nodeBody; else { m_nodeBody = Resources.Load<Texture2D>("NodeBody"); return m_nodeBody; } } }
        private static Texture2D m_nodeHeight;
        /// <summary> 节点高亮贴图 </summary>
        public static Texture2D NodeHeightTexture { get { if (m_nodeHeight != null) return m_nodeHeight; else { m_nodeHeight = Resources.Load<Texture2D>("NodeHeightLight"); return m_nodeHeight; } } }
        private static Texture2D m_nodePort;
        /// <summary> 节点高亮贴图 </summary>
        public static Texture2D NodePortTexture { get { if (m_nodePort != null) return m_nodePort; else { m_nodePort = Resources.Load<Texture2D>("Port"); return m_nodePort; } } }
        private static Texture2D m_gridTexture;
        /// <summary> 网格背景贴图 </summary>
        public static Texture2D GridTexture => m_gridTexture != null ? m_gridTexture : ComputeGridTexture2D();
        /// <summary> 强制重新计算网格贴图，主要用于改设置时的立即生效 </summary>
        public static Texture2D ComputeGridTexture2D()
        {
            int texSize = 60;//贴图尺寸LOOKME:100以内只有60的约数最多，拥有最多网格尺寸的选择。100像素以外计算量和像素尺寸呈现平方关系，感觉过大没必要
            Texture2D result = new Texture2D(texSize, texSize);
            Color32[,] colors = new Color32[texSize, texSize];
            //计算生成网格颜色在此处
            for (int i = 0; i < texSize; i++)
            {
                for (int j = 0; j < texSize; j++)
                {
                    if (i == 0 || j == 0)//如果是大网格边缘，应用网格十字颜色
                        colors[i, j] = Setting.CrossColor;
                    else if (i % Setting.GridSize == 0 || j % Setting.GridSize == 0)//如果是小网格边缘，应用网格前景色
                        colors[i, j] = Setting.FrontColor;
                    else colors[i, j] = Setting.BackColor;//否则都使用网格背景色
                }
            }
            result.SetPixels32(colors.Cast<Color32>().ToArray());
            result.wrapMode = TextureWrapMode.Repeat;
            result.filterMode = FilterMode.Bilinear;
            result.name = "Grid";
            result.Apply();
            m_gridTexture = result;
            return result;
        }
        private static NodeStyle m_nodeStyle = null;
        public static NodeStyle NodeGUIStyle { get { m_nodeStyle ??= new NodeStyle(); return m_nodeStyle; } }
        /// <summary>
        /// 节点排布方式
        /// </summary>
        internal class NodeStyle
        {
            public GUIStyle nodeBody, nodeHeight, nodeTitle, nodePort;
            public NodeStyle()
            {
                nodeBody = new GUIStyle();
                nodeBody.normal.background = ResourceLoader.NodeBodyTexture;
                nodeBody.padding = new RectOffset(3, 3, 3, 3);//别问，问就是在编辑器拉的边框
                nodeBody.border = new RectOffset(20, 20, 64, 10);

                nodeHeight = new GUIStyle();
                nodeHeight.normal.background = ResourceLoader.NodeHeightTexture;
                nodeHeight.padding = new RectOffset(0, 0, 0, 0);//别问，问就是在编辑器拉的边框
                nodeHeight.border = new RectOffset(40, 40, 40, 40);
            }
            public void SetNodeBodyColorOffset(Color color)
            {
                return;

                /* var source = nodeBody.normal.background;
                for (int y = 0; y < source.height; y++)
                {
                    for (int x = 0; x < source.width; x++)
                    {
                        // 获取当前像素的颜色FIXME:报错：这张Texture的可读性false，因此无法改变颜色
                        Color pixelColor = source.GetPixel(x, y);

                        // 将每个颜色通道与所需的颜色相乘
                        pixelColor *= color;

                        // 将修改后的颜色设置回新的纹理
                        source.SetPixel(x, y, pixelColor);
                    }
                }
                source.Apply(); */
            }
        }
    }
}
