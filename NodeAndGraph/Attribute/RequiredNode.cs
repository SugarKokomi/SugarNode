using System;
namespace SugarNode
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class RequiredNodeAttribute : Attribute
    {
        public Type nodeType;
        public uint minNum = 1;
        public RequiredNodeAttribute(Type nodeType, uint minNum = 1)
        {
            this.nodeType = nodeType;
            this.minNum = minNum;
        }
    }
}
