using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
namespace SugarNode
{
    [Serializable]
    public abstract class NodeGraph : ScriptableObject
    {
        public List<Node> nodes = new List<Node>();
        public T AddNode<T>() where T : Node => AddNode(typeof(T)) as T;
        public virtual Node AddNode(Type newNodeType)
        {
            if (CheckNodeCanAddIn(newNodeType))//输入类型必须同时满足用户自定义Type，以及必须继承或者就是Node
            {
                Node newNode = CreateInstance(newNodeType) as Node;
                newNode.name = newNodeType.Name;
                newNode.Owner = this;
                nodes.Add(newNode);
                return newNode;
            }
            else throw new InvalidCastException($"{newNodeType.Name}必须继承Node类,且是[OnlyAllowNode]允许的类型，才能添加到该节点图！");
        }
        public virtual void RemoveNode(Node node)
        {
            nodes.Remove(node);
        }
        public bool CheckNodeCanAddIn(Type nodeType)
        {
            if (typeof(Node).IsAssignableFrom(nodeType))
            {
                var cache = GetAllowNodeTypeCache(this.GetType());
                if (cache == null) return true;
                else foreach (var type in cache)
                        if (nodeType.IsSubclassOf(type) || nodeType.Equals(type))
                            return true;
                return false;
            }
            else return false;
        }
        #region Cache
        private static readonly Dictionary<Type, HashSet<Type>> allowNodeTypeCache = new Dictionary<Type, HashSet<Type>>();
        public static HashSet<Type> GetAllowNodeTypeCache(Type type)
        {
            if (allowNodeTypeCache.TryGetValue(type, out var value))
                return value;
            else
            {
                OnlyAllowNodeAttribute[] attributes = type.GetCustomAttributes(typeof(OnlyAllowNodeAttribute), true) as OnlyAllowNodeAttribute[];
                if (attributes != null && attributes.Length > 0)//如果写了[OnlyAllowNode]
                {
                    var output = new HashSet<Type>();
                    foreach (var attri in attributes)
                        output.Add(attri.nodeType);
                    allowNodeTypeCache.Add(type, output);
                    return output;
                }
                else//否则就是没有写[OnlyAllowNode]的Attribute
                {
                    allowNodeTypeCache.Add(type, null);
                    return null;
                }
            }
        }
        #endregion
    }
}