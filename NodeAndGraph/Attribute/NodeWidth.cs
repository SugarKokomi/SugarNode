using System;
namespace SugarNode
{
    /// <summary><para> 用于修改节点的宽度 </para>
    /// <para> 定义1网格单位是1000的GUI长度 </para>
    /// <para> 实际修改PPI位置：NodeEditor.PPI </para>
    /// </summary>
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