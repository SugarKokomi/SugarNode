using System;
namespace SugarNode
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class NodeWidthAttribute : Attribute
    {
        public int width;
        public NodeWidthAttribute(int width)
        {
            this.width = width;
        }
    }
}