using System;
using UnityEngine;
namespace SugarNode
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class NodeColorAttribute : Attribute
    {
        public Color color = Color.white;
        public NodeColorAttribute(Color color) => this.color = color;
        public NodeColorAttribute(float r, float g, float b) => this.color = new Color(r, g, b);
        public NodeColorAttribute(uint r, uint g, uint b) => this.color = new Color(r / 255, g / 255, b / 255);
    }
}