using System;
namespace SugarNode
{
    /// <summary>
    /// <para> 强制某个NodeGraph必须拥有某些节点 </para>
    /// <para> 仅编辑器时生效，用户写脚本创建的NodeGraph无法保证生效 </para>
    /// </summary>
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