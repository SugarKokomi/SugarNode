using System;
namespace SugarNode
{
    /// <summary> 该类用于指定一个NodePort最多能连几根线 </summary>
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