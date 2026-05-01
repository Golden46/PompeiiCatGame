using UnityEngine;
 
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
    public DialogueLine[] lines;
    public Color catColor;
}