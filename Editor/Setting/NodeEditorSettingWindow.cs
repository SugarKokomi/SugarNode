using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using static SugarNode.Editor.NodeEditorSettings;

namespace SugarNode.Editor
{
    //这是位于Edit/Preferences/节点编辑器设置的窗口，用于给节点编辑器的数值进行默认的设置
    //备注：相当于换了个位置，给NodeEditorSettings这个类写了一个OnGUI()
    internal static class NodeEditorSettingWindow
    {
        static SettingsProvider m_provider;
        [SettingsProvider]
        private static SettingsProvider PreferencesGUI()
        {
            m_provider ??= new SettingsProvider("Sugar Node Editor Setting", SettingsScope.User)
            {
                label = "SugarNode编辑器设置",
                activateHandler = (searchContext, rootElement) =>
                {
                    ColorField cColor = new ColorField("网格十字颜色");
                    ColorField fColor = new ColorField("网格线颜色");
                    ColorField bColor = new ColorField("网格背景颜色");
                    PopupField<int> gridWidth = new PopupField<int>("网格尺寸", gridWidthChoose, 7);
                    FloatField scaleRangeMin = new FloatField("最小缩放系数");
                    FloatField scaleRangeMax = new FloatField("最大缩放系数");
                    FloatField moveSpeed = new FloatField("鼠标灵敏度");
                    FloatField scaleSpeed = new FloatField("滚轮灵敏度");
                    Button resetButton = new Button();
                    resetButton.text = "重设为默认值";
                    resetButton.clicked += () =>
                    {
                        Setting.Reset();
                        cColor.value = Setting.CrossColor;
                        fColor.value = Setting.FrontColor;
                        bColor.value = Setting.BackColor;
                        gridWidth.value = Setting.GridSize;
                        scaleRangeMin.value = Setting.scaleRangeMin;
                        scaleRangeMax.value = Setting.scaleRangeMax;
                        moveSpeed.value = Setting.moveSpeed;
                        scaleSpeed.value = Setting.scaleSpeed;
                    };
                    //赋初始值--------------------------------------------
                    cColor.value = Setting.CrossColor;
                    fColor.value = Setting.FrontColor;
                    bColor.value = Setting.BackColor;
                    gridWidth.value = Setting.GridSize;
                    scaleRangeMin.value = Setting.scaleRangeMin;
                    scaleRangeMax.value = Setting.scaleRangeMax;
                    moveSpeed.value = Setting.moveSpeed;
                    scaleSpeed.value = Setting.scaleSpeed;
                    //自定义UI排布方式--------------------------------------
                    rootElement.Add(cColor);
                    rootElement.Add(fColor);
                    rootElement.Add(bColor);
                    rootElement.Add(gridWidth);

                    VisualElement root1 = new VisualElement();
                    root1.style.flexDirection = FlexDirection.Row;
                    rootElement.Add(root1);
                    scaleRangeMin.style.width = 200;
                    scaleRangeMax.style.width = 200;
                    root1.Add(scaleRangeMin);
                    root1.Add(scaleRangeMax);

                    VisualElement root2 = new VisualElement();
                    root2.style.flexDirection = FlexDirection.Row;
                    rootElement.Add(root2);
                    moveSpeed.style.width = 200;
                    scaleSpeed.style.width = 200;
                    root2.Add(moveSpeed);
                    root2.Add(scaleSpeed);

                    resetButton.style.width = 200;
                    rootElement.Add(resetButton);
                    //注册值改变事件--------------------------------------------
                    cColor.RegisterValueChangedCallback(col => Setting.CrossColor = col.newValue);
                    fColor.RegisterValueChangedCallback(col => Setting.FrontColor = col.newValue);
                    bColor.RegisterValueChangedCallback(col => Setting.BackColor = col.newValue);
                    gridWidth.RegisterValueChangedCallback(value => Setting.GridSize = value.newValue);
                    scaleRangeMin.RegisterValueChangedCallback(value =>
                    {
                        scaleRangeMin.value = Mathf.Clamp(value.newValue, 0, scaleRangeMax.value);
                        Setting.scaleRangeMin = value.newValue;
                    });
                    scaleRangeMax.RegisterValueChangedCallback(value =>
                    {
                        scaleRangeMax.value = Mathf.Clamp(value.newValue, scaleRangeMin.value, 100);
                        Setting.scaleRangeMax = value.newValue;
                    });
                    moveSpeed.RegisterValueChangedCallback(value =>
                    {
                        moveSpeed.value = Mathf.Clamp(value.newValue, 0, 10);
                        Setting.moveSpeed = value.newValue;
                    });
                    scaleSpeed.RegisterValueChangedCallback(value =>
                    {
                        scaleSpeed.value = Mathf.Clamp(value.newValue, 0, 10);
                        Setting.scaleSpeed = value.newValue;
                    });
                }/* ,keywords = new HashSet<string>(new[] { fColor.label, bColor.label, gridWidth.label, gridWidth.label }) */
            };
            return m_provider;
        }
        private static readonly List<int> gridWidthChoose = new List<int>() { 1, 2, 3, 4, 5, 6, 10, 12, 15, 20, 30, 60 };//60的所有约数

    }
}