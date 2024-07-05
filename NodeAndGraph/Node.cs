#define SugarNode
using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;
using UnityEditor.Graphs;
using SugarNode.Editor;

namespace SugarNode
{
    [Serializable]
    public abstract class Node : ScriptableObject
    {
        [SerializeField]
        internal Vector2 position = Vector2.zero;
        public readonly HashSet<InputPort> Inputs = new HashSet<InputPort>();
        public readonly HashSet<OutputPort> Outputs = new HashSet<OutputPort>();
        public NodeGraph Owner { get; internal set; }
        public virtual object GetPortValue(NodePort nodePort) => default;
        public virtual T GetOutputValue<T>(OutputPort<T> outputPort) => outputPort.value;
        internal const uint DefaultNodeWidth = 2;
        //----------------------------------------------------------------
        public Node()
        {
            CheckNodePortOwner();
        }
        public void AddPort(NodePort port)
        {
            if (port is InputPort input) Inputs.Add(input);
            else Outputs.Add((OutputPort)port);
            port.SetOwner(this);
        }
        public void CheckNodePortOwner()//通过反射解析子类的NodePort
        {
            FieldInfo[] fields = this.GetType().GetFields(BindingFlags.Public);//获取public字段
            foreach (var field in fields)
            {//获取非静态的NodePort,这个if里面需要解析出普通的NodePort和List以及Array（不会吧不会吧，不会真有人要用Dictionary<TKey,NodePort>吧）

                if (typeof(NodePort).IsAssignableFrom(field.FieldType) && !field.IsStatic)
                {
                    NodePort nodePort = (NodePort)field.GetValue(this);
                    nodePort ??= (NodePort)Activator.CreateInstance(field.FieldType);
                    AddPort(nodePort);
                }
                else if (field.FieldType.IsArray && !field.IsStatic)
                {
                    var nodePorts = (NodePort[])field.GetValue(this);
                    foreach (var nodePort in nodePorts)
                    {
                        AddPort(nodePort);
                    }
                }
                else if (field.FieldType == typeof(List<>) && !field.IsStatic)
                {
                    var nodePorts = (List<NodePort>)field.GetValue(this);
                    foreach (var nodePort in nodePorts)
                    {
                        AddPort(nodePort);
                    }
                }
            }
        }
        public virtual void OnNodeEditorGUI()
        {
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.GetIterator();
            serializedObject.Update();
            bool enterChild = true;
            while (property.NextVisible(enterChild))
            {
                enterChild = false;
                if (property.name == "position" || property.name == "m_Script") continue;
                EditorGUILayout.PropertyField(property);
            }
            serializedObject.ApplyModifiedProperties();
        }
        #region Internal And EditorOnly
#if UNITY_EDITOR
        //避免使用Reflection在OnGUI每帧获取，节约性能，写缓存记录该节点的Attribute信息
        static Dictionary<Type, Dictionary<Type, Attribute>> attributeCache = new Dictionary<Type, Dictionary<Type, Attribute>>();//节点Type套着Attribute Type的字典
        internal T GetAttributeCache<T>() where T : Attribute
        {
            Type nodeType = this.GetType(), attributeType = typeof(T);
            if (!attributeCache.TryGetValue(nodeType, out var nodeAttri))
            {
                nodeAttri = new Dictionary<Type, Attribute>();
                attributeCache.Add(nodeType, nodeAttri);
            }
            if (nodeAttri.TryGetValue(attributeType, out var newAttribute1))
                return newAttribute1 as T;
            else
            {
                var newAttribute2 = nodeType.GetCustomAttribute(attributeType);
                attributeCache[nodeType].Add(attributeType, newAttribute2);
                return newAttribute2 as T;
            }
        }
        //------------------------------------------------------
        internal string GetNodeTitle()
        {
            var attribute = GetAttributeCache<NodeTitleAttribute>();
            if (attribute != null)//有Attribute，则返回自定义的Attribute的name
                return attribute.name;
            else//没有Attribute就看是不是Node结尾的类名，是就去掉Node四个字母返回
            {
                string typeName = this.GetType().Name;
                if (typeName.ToLower().EndsWith("node"))
                    return typeName[..(typeName.Length - 4)];
                else return typeName;
            }
        }
        internal uint GetNodeWidthInGridSpace()
        {
            var attribute = GetAttributeCache<NodeWidthAttribute>();
            return attribute?.width ?? DefaultNodeWidth;
        }
        static Dictionary<Type, float> nodeHightCache = new Dictionary<Type, float>();
        internal float GetNodeHeightInGridSpace()
        {
            Type type = this.GetType();
            if (nodeHightCache.TryGetValue(type, out var ret))
                return ret;
            else
            {
                SerializedObject serializedObject = new SerializedObject(this);
                var serializedProperty = serializedObject.GetIterator();
                ret = 0;
                while (serializedProperty.NextVisible(true))
                {
                    ret++;
                }
                ret = .5f + .1f * ret;//TODO:此处有问题，需要把字段数量和图片72像素的尺寸转换到网格空间
                nodeHightCache.Add(type, ret);
                return nodeHightCache[type];
            }
        }
        internal Rect GetNodeRectInGridSpace()
        {
            var width = GetNodeWidthInGridSpace();
            var height = GetNodeHeightInGridSpace();
            return new Rect(position, new Vector2(width, height));
        }
#endif
        #endregion
    }
}
