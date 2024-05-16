namespace SugarNode.Example
{
    [CreateMenu("对话系统/普通对话节点")]
    public class NormalNode : DialogueBaseNode
    {
        public InputPort input = new InputPort();
        public OutputPort<string> output = new OutputPort<string>(string.Empty);
    }
}