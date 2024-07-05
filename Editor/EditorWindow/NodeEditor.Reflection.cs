using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
namespace SugarNode.Editor
{
    partial class NodeEditorWindow
    {
        /// <summary>
        /// <para> 对于一个Type（NodeGraph被标记上OnlyAllowNode的Type）</para>
        /// <para> 缓存其所有的可实例化的子类 </para>
        /// </summary>
        private static Dictionary<Type, HashSet<Type>> allowNodeTypeCache = new Dictionary<Type, HashSet<Type>>();
        /// <summary> 获取一个类的所有可以实例化的子类 </summary>
        private static HashSet<Type> GetSubclasses(Type baseType)
        {
            if (allowNodeTypeCache.TryGetValue(baseType, out var ret))
                return ret;
            else
            {
                Assembly assembly = baseType.Assembly;
                var cache = assembly.GetTypes()
                    .Where(t => baseType.IsAssignableFrom(t) && t.IsPublic && !t.IsAbstract)//必须继承或者就是这个类，且必须public(private类我找他来显示出来干嘛)，必须是非抽象类
                    .ToHashSet();
                allowNodeTypeCache.Add(baseType, cache);
                return cache;
            }
        }
        /// <summary>
        /// 反射读取CreateMenuAttribute,返回右键创建菜单的路径
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static string GetRightMenuPath(Type type)
        {
            return
                type.GetCustomAttribute(typeof(CreateMenuAttribute)) is CreateMenuAttribute createMenu ?
                createMenu.menuPath :
                type.Name;
        }
    }
}