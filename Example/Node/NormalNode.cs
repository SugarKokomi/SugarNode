namespace SugarNode.Example
{
    [CreateMenu("对话系统/普通对话节点")]
    [NodeTitle("普通对话节点")]
    public class NormalNode : DialogueBaseNode
    {
        public InputPort input = new InputPort();
        public OutputPort<string> output = new OutputPort<string>(string.Empty);
        public string text = "我说话了！";

        public UnityEngine.AudioSource audio;
    }
}