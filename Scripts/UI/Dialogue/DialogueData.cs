using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Dialogue")]
public class DialogueData : ScriptableObject
{
    public string text;                    // Текст NPC
    public List<DialogueOption> options;   // Варианты ответа
}