using System;
using System.Linq;
using System.Collections.Generic;

namespace SugarNode
{
    /// <summary>
    /// 节点端口基类
    /// </summary>
    [Serializable]
    public abstract class NodePort
    {
        public Node Owner { get; internal set; }
        public HashSet<NodePort> Connections { get; internal set; } = new HashSet<NodePort>();
        public NodePort DefaultConnection => Connections.FirstOrDefault();
        public void ConnectWith(NodePort nodePort)
        {
            Connections.Add(nodePort);
            nodePort.Connections.Add(this);
        }
        public void SetOwner(Node node) => this.Owner = node;
        public static void CheckNodePort<T>(ref NodePort nodePort , Node Owner) where T : NodePort, new()
        {
            nodePort ??= new T();
            nodePort.Owner = Owner;
        }
    }
    /// <summary>
    /// 不传递值，只用于传递节点关系的NodePort
    /// </summary>
    public class InputPort : NodePort { }
    public class OutputPort : NodePort { }
    public class InputPort<T> : InputPort
    {
        public T value;
        public InputPort(T defaultValue = default) : base()
        {
            value = defaultValue;
        }
    }
    public class OutputPort<T> : OutputPort
    {
        public T value;
        public OutputPort(T defaultValue = default) : base()
        {
            value = defaultValue;
        }
    }
}