using System;
using UnityEngine;
namespace SugarNode.Example
{
    [CreateAssetMenu(fileName = "对话文件",menuName = "创建对话文件")]
    [OnlyAllowNode(typeof(DialogueBaseNode))]
    public class DialogueGraph : NodeGraph
    {
    }
}