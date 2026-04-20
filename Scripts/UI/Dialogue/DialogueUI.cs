using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;

    [Header("Главное окно")]
    [SerializeField] private GameObject dialogueCanvas;      // DialogueCanvas
    [SerializeField] private GameObject optionsCanvas;       // OptionsDialogueCanvas ← новое

    [Header("Игрок — левая сторона")]
    [SerializeField] private Image playerPortraitImage;
    [SerializeField] private TMP_Text playerNameText;

    [Header("НПС — правая сторона")]
    [SerializeField] private Image npcPortraitImage;
    [SerializeField] private TMP_Text npcNameText;

    [Header("Диалог")]
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Transform optionsContainer;     // OptionsContainer внутри OptionsCanvas
    [SerializeField] private Button optionPrefab;

    [Header("Скорость печати")]
    [SerializeField] private float typewriterSpeed = 0.03f;

    private Coroutine typewriterCoroutine;
    private bool isTyping = false;
    private DialogueData currentDialogue;

    private void Awake()
    {
        Instance = this;
        dialogueCanvas.SetActive(false);
        optionsCanvas.SetActive(false); // Оба канваса скрыты по умолчанию
    }

    public void OpenDialogue(DialogueData dialogue)
    {
        currentDialogue = dialogue;
        GameState.IsDialogueOpen = true;

        dialogueCanvas.SetActive(true);
        optionsCanvas.SetActive(true); // Кнопки скрыты пока текст печатается

        // Портрет и имя игрока
        if (playerNameText != null)
            playerNameText.text = dialogue.playerName;
        if (playerPortraitImage != null)
        {
            playerPortraitImage.sprite = dialogue.playerPortrait;
            playerPortraitImage.gameObject.SetActive(dialogue.playerPortrait != null);
        }

        // Портрет и имя NPC
        if (npcNameText != null)
            npcNameText.text = dialogue.npcName;
        if (npcPortraitImage != null)
        {
            npcPortraitImage.sprite = dialogue.npcPortrait;
            npcPortraitImage.gameObject.SetActive(dialogue.npcPortrait != null);
        }

        // Запускаем печать текста
        ClearOptions();
        if (typewriterCoroutine != null) StopCoroutine(typewriterCoroutine);
        typewriterCoroutine = StartCoroutine(TypeText(dialogue));
    }

    private IEnumerator TypeText(DialogueData dialogue)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in dialogue.text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typewriterSpeed);
        }

        isTyping = false;
        SpawnOptions(dialogue); // Текст допечатан — показываем кнопки
    }

    private void Update()
    {
        // Пропустить анимацию печати по нажатию E или Space
        if (GameState.IsDialogueOpen && isTyping)
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
            {
                StopCoroutine(typewriterCoroutine);
                isTyping = false;
                dialogueText.text = currentDialogue.text;
                SpawnOptions(currentDialogue);
            }
        }
    }

    private void SpawnOptions(DialogueData dialogue)
    {
        ClearOptions();
        optionsCanvas.SetActive(true); // Показываем канвас с кнопками

        foreach (var option in dialogue.options)
        {
            Button btn = Instantiate(optionPrefab, optionsContainer);
            btn.GetComponentInChildren<TMP_Text>().text = option.text;

            var localOption = option;
            btn.onClick.AddListener(() =>
            {
                if (localOption.nextDialogue != null)
                    OpenDialogue(localOption.nextDialogue);
                else
                    CloseDialogue();
            });
        }
    }

    private void ClearOptions()
    {
        foreach (Transform child in optionsContainer)
            Destroy(child.gameObject);
    }

    public void CloseDialogue()
    {
        if (typewriterCoroutine != null) StopCoroutine(typewriterCoroutine);
        isTyping = false;
        GameState.IsDialogueOpen = false;
        ClearOptions();
        dialogueCanvas.SetActive(false);
        optionsCanvas.SetActive(false); // Прячем оба канваса
    }
}