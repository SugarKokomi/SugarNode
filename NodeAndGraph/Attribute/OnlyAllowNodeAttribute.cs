using System;
namespace SugarNode
{
    /// <summary> 让一个NodeGraph只能创建某些类型（包括其子类）的节点 </summary>
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