using System;
using UnityEngine;
namespace SugarNode
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class NodeTitleAttribute : Attribute
    {
        public string name;
        public NodeTitleAttribute(string name = "")
        {
            this.name = name;
        }
    }
}