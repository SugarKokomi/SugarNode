namespace SugarNode.Example
{
    [NodeColor(1,0,0)]
    [NodeWidth(1)]
    [CreateMenu("特殊节点/开始对话")]
    public class StartNode : DialogueBaseNode
    {
        public OutputPort input = new OutputPort();
    }
}