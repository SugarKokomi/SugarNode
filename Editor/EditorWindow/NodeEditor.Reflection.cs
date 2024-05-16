using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
namespace SugarNode.Editor
{
    partial class NodeEditorWindow
    {
        private static Dictionary<Type, HashSet<Type>> allowNodeTypeCache = new Dictionary<Type, HashSet<Type>>();
        /// <summary> 获取一个类的所有可以实例化的子类 </summary>
        public static HashSet<Type> GetSubclasses(Type baseType)
        {
            if (allowNodeTypeCache.TryGetValue(baseType, out var ret))
                return ret;
            else
            {
                Assembly assembly = baseType.Assembly;
                var cache = assembly.GetTypes()
                    .Where(t => baseType.IsAssignableFrom(t) && t.IsPublic && !t.IsAbstract)//必须继承或者就是这个类，且必须public(private类我找他来显示出来干嘛)，必须是非抽象类
                    .ToHashSet();
                allowNodeTypeCache.Add(baseType,cache);
                return cache;
            }
        }
    }
}