namespace SugarNode.Example
{
    [NodeColor(1,0,0)]
    [NodeWidth(1)]
    [CreateMenu("特殊节点/结束对话")]
    public class EndNode : DialogueBaseNode
    {
        public InputPort input = new InputPort();
    }
}