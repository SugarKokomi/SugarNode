using System;
using UnityEngine;
namespace SugarNode
{
    /// <summary> 用于修改节点的默认颜色 </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class NodeColorAttribute : Attribute
    {
        public Color color = Color.white;
        //public NodeColorAttribute(Color color) => this.color = color;//这个没法用，因为UnityEngine.Color.red不是常量，而是静态变量
        public NodeColorAttribute(float r, float g, float b) => this.color = new Color(r, g, b);
        public NodeColorAttribute(uint r, uint g, uint b) => this.color = new Color(r / 255, g / 255, b / 255);
    }
}