namespace SugarNode.Example
{
    public class NormalNode : DialogueBaseNode
    {
        public InputPort input = new InputPort();
        public OutputPort<string> output = new OutputPort<string>(string.Empty);
    }
}