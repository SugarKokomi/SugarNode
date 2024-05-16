using System;
namespace SugarNode
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class CreateMenuAttribute : Attribute
    {
        public string menuPath;
        public CreateMenuAttribute(string menuPath)
        {
            this.menuPath = menuPath;
        }
    }
}