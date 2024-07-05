namespace SugarNode.Example
{
    public abstract class DialogueBaseNode : SugarNode.Node
    {
    }
    [NodeTitle("分支节点")]
    public class BranchNode : DialogueBaseNode
    {
        public InputPort input = new InputPort();
        public OutputPort<string> output = new OutputPort<string>(string.Empty);
        public int id;
        public string text = "hello world";
    }
}