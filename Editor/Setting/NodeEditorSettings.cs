using System.IO;
using UnityEngine;
using UnityEditor;

namespace SugarNode.Editor
{
    internal class NodeEditorSettings : ScriptableObject
    {
        [SerializeField]
        private Color crossColor, frontGroundColor, backGroundColor;
        [SerializeField]
        private int gridSize;
        public float scaleRangeMin, scaleRangeMax;
        public float moveSpeed, scaleSpeed;
        NodeEditorSettings()
        {
            SetDefaultValue();
        }
        //--------------------------------------------------------------------------------------------
        public Color CrossColor
        {
            get => crossColor;
            set
            {
                crossColor = value;
                UpdateNodeEditorGUI(this);
            }
        }
        public Color FrontColor
        {
            get => frontGroundColor;
            set
            {
                frontGroundColor = value;
                UpdateNodeEditorGUI(this);
            }
        }
        public Color BackColor
        {
            get => backGroundColor;
            set
            {
                backGroundColor = value;
                UpdateNodeEditorGUI(this);
            }
        }
        public int GridSize
        {
            get => gridSize;
            set
            {
                gridSize = value;
                UpdateNodeEditorGUI(this);
            }
        }
        private const string SettingFolderPath = "Assets/Settings";
        private const string SettingFilePath = SettingFolderPath + "/NodeEditorSetting.asset";
        private static NodeEditorSettings m_Setting;
        public static NodeEditorSettings Setting
        {
            get
            {
                if (m_Setting != null) return m_Setting;
                else
                {
                    m_Setting = AssetDatabase.LoadAssetAtPath<NodeEditorSettings>(SettingFilePath);
                    if (m_Setting == null)
                    {
                        m_Setting = CreateInstance<NodeEditorSettings>();
                        m_Setting.backGroundColor = Color.white;
                        if (!Directory.Exists(SettingFolderPath))
                            Directory.CreateDirectory(SettingFolderPath);
                        AssetDatabase.CreateAsset(m_Setting, SettingFilePath);
                        AssetDatabase.SaveAssets();
                    }
                    return m_Setting;
                }
            }
        }
        public void Reset()
        {
            SetDefaultValue();
            ResourceLoader.ComputeGridTexture2D();
            NodeEditorWindow.Instence.Repaint();
        }
        private void SetDefaultValue()
        {
            crossColor = new Color(.8f, .8f, .8f, 1);
            frontGroundColor = new Color(.5f, .5f, .5f, 1);
            backGroundColor = new Color(.3f, .3f, .3f, 1);
            gridSize = 10;
            scaleRangeMin = 5f;
            scaleRangeMax = 20;
            moveSpeed = 1;
            scaleSpeed = 0.5f;
        }
        public static void UpdateNodeEditorGUI(NodeEditorSettings self)
        {
            ResourceLoader.ComputeGridTexture2D();
            NodeEditorWindow.Instence.Repaint();
        }
    }
}