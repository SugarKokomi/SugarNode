using System;
namespace SugarNode
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public sealed class OnlyAllowNodeAttribute : Attribute
    {
        public Type nodeType;
        public OnlyAllowNodeAttribute(Type nodeType)
        {
            this.nodeType = nodeType;
        }
    }
}