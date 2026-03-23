using UnityEngine;

[System.Serializable]
public class DialogueOption
{
    public string text;                // Текст кнопки
    public DialogueData nextDialogue; // Куда ведёт выбор
}