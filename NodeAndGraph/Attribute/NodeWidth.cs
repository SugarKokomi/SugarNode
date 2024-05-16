using System;
namespace SugarNode
{
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