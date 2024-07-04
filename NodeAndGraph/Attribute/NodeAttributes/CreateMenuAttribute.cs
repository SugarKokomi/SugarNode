using System;
namespace SugarNode
{
    /// <summary> 右键创建Node节点的菜单 </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class CreateMenuAttribute : Attribute
    {
        public string menuPath;
        public CreateMenuAttribute(string menuPath)
        {
            this.menuPath = menuPath;
        }
    }
}