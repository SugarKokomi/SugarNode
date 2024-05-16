using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

namespace SugarNode
{
    [Serializable]
    public abstract class Node : ScriptableObject
    {
        [SerializeField]
        internal Vector2 position = Vector2.zero;
        public HashSet<InputPort> Inputs { get; private set; } = new HashSet<InputPort>();
        public HashSet<OutputPort> Outputs { get; private set; } = new HashSet<OutputPort>();
        public NodeGraph Owner { get; internal set; }
        public virtual object GetPortValue(NodePort nodePort) => default;
        public virtual T GetOutputValue<T>(OutputPort<T> outputPort) => outputPort.value;
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
#endif
        #endregion
    }
}