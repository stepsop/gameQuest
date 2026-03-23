using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;

    [Header("UI")]
    [SerializeField] private GameObject window;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Transform optionsContainer;
    [SerializeField] private Button optionPrefab;

  
    private void Awake()
    {
        Instance = this;
        window.SetActive(false);
    }

    /// <summary>
    /// Очистка всех кнопок (чтобы не было клонов)
    /// </summary>
    private void ClearOptions()
    {
        foreach (Transform child in optionsContainer)
        {
            Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// Открытие диалога
    /// </summary>
    public void OpenDialogue(DialogueData dialogue)
    {
        GameState.IsDialogueOpen = true;

        window.SetActive(true);

        dialogueText.text = dialogue.text;

        ClearOptions();

        foreach (var option in dialogue.options)
        {
            Button btn = Instantiate(optionPrefab, optionsContainer);

            btn.GetComponentInChildren<TMP_Text>().text = option.text;

            btn.onClick.AddListener(() =>
            {
                // 👉 Переход к следующему диалогу
                if (option.nextDialogue != null)
                {
                    OpenDialogue(option.nextDialogue);
                }
                else
                {
                    CloseDialogue();
                }
            });
        }
    }

    /// <summary>
    /// Закрытие диалога
    /// </summary>
    public void CloseDialogue()
    {
        GameState.IsDialogueOpen = false;
        ClearOptions();
        window.SetActive(false);
    }
}