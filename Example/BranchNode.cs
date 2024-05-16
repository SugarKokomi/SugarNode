namespace SugarNode.Example
{
    public abstract class DialogueBaseNode : SugarNode.Node
    {
    }
    public class BranchNode : DialogueBaseNode
    {
        public InputPort input = new InputPort();
        public OutputPort<string> output = new OutputPort<string>(string.Empty);
    }
}