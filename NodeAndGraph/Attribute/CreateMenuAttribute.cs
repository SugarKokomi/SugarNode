using System;
namespace SugarNode
{
    /// <summary> 该类用于指定节点编辑时的右键创建节点的菜单 </summary>
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