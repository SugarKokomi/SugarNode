using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
namespace SugarNode.Editor
{
    partial class NodeEditorWindow
    {
        /// <summary> 获取一个类的所有子类 </summary>
        public static HashSet<Type> GetSubclasses(Type baseType)
        {
            Assembly assembly = baseType.Assembly;
            return assembly.GetTypes().Where(t => t.IsSubclassOf(baseType)).ToHashSet();
        }
    }
}