using System;
namespace SugarNode
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class MultiplyPortAttribute : Attribute
    {
        public uint maxCount;
        public MultiplyPortAttribute(uint maxCount)
        {
            this.maxCount = maxCount;
        }
    }
}