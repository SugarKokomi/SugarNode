using System;
namespace SugarNode
{
    /// <summary> 用于修改节点的宽度 </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class NodeWidthAttribute : Attribute
    {
        public uint width;
        public NodeWidthAttribute(uint width)
        {
            this.width = width;
        }
    }
}