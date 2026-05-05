using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public struct DialogueLine
{
    public string speaker;
    [TextArea(2, 5)]
    public string text;
}
 
[CreateAssetMenu(menuName = "Quests/Cat Dialogue")]
public class CatDialogue : ScriptableObject
{
    [FormerlySerializedAs("lines")] public DialogueLine[] startLines;
    public DialogueLine[] endLines;
    public Color catColor;
}