using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [Header("Panels")]
    public GameObject dialoguePanel;
    public GameObject choicesPanel;
    public TextMeshProUGUI prompt;

    [Header("Text Fields")]
    public TMP_Text characterNameText;
    public TMP_Text messageText;

    [Header("Buttons & Layout")]
    public Button nextButton;
    public Transform choiceButtonContainer;
    public Button choiceButtonPrefab;

    [Header("Typewriter Settings")]
    [SerializeField] private float typingSpeed = 0.03f;

    private List<Button> activeChoiceButtons = new List<Button>();
    public static DialogueUI instance;

    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private string currentFullMessage = "";

    private void OnEnable()
    {
        DialogueEvents.onDialogueProgress += OnDialogueProgress;
        DialogueEvents.onDialogueEnd += OnDialogueEnd;

        if (nextButton != null)
        {
            nextButton.onClick.AddListener(OnNextButtonClicked);
        }
    }

    private void OnDisable()
    {
        DialogueEvents.onDialogueProgress -= OnDialogueProgress;
        DialogueEvents.onDialogueEnd -= OnDialogueEnd;

        if (nextButton != null)
        {
            nextButton.onClick.RemoveListener(OnNextButtonClicked);
        }
    }

    private void Awake() { instance = this; }

    private void Start()
    {
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (choicesPanel != null) choicesPanel.SetActive(false);
    }

    private void OnDialogueProgress(int dialogueId, int messageIndex)
    {
        Dialogue current = DialogueManager.Instance.GetCurrentDialogue();
        if (current == null) return;

        dialoguePanel.SetActive(true);
        choicesPanel.SetActive(false);
        ClearChoices();

        if (messageIndex < current.characters.Count)
        {
            characterNameText.text = current.characters[messageIndex].ToString().Replace("_", " ");
        }

        if (messageIndex < current.messages.Count)
        {
            currentFullMessage = current.messages[messageIndex];
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            typingCoroutine = StartCoroutine(TypeText(currentFullMessage));
        }

        if (nextButton != null) nextButton.gameObject.SetActive(true);
    }

    private IEnumerator TypeText(string targetText)
    {
        isTyping = true;
        messageText.text = "";

        foreach (char letter in targetText.ToCharArray())
        {
            messageText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    private void CompleteTextImmediately()
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        messageText.text = currentFullMessage;
        isTyping = false;
    }

    private void OnNextButtonClicked()
    {
        Dialogue current = DialogueManager.Instance.GetCurrentDialogue();
        if (current == null) return;

        // Undertale Mechanics:
        // 1. If currently typing -> Rush text completion
        if (isTyping)
        {
            CompleteTextImmediately();
            return;
        }

        // 2. If finished typing -> Proceed normally
        int currentIndex = DialogueManager.Instance.GetCurrentMessageIndex();

        if (currentIndex >= current.messages.Count - 1 && current.messages.Count > 0)
        {
            RenderReplies(current);
        }
        else
        {
            DialogueManager.Instance.NextMessage();
        }
    }

    private void RenderReplies(Dialogue dialogue)
    {
        if (nextButton != null) nextButton.gameObject.SetActive(false);
        choicesPanel.SetActive(true);
        ClearChoices();

        for (int i = 0; i < dialogue.replies.Length; i++)
        {
            int index = i;
            Button btn = Instantiate(choiceButtonPrefab, choiceButtonContainer);

            TMP_Text btnText = btn.GetComponentInChildren<TMP_Text>();
            if (btnText != null) btnText.text = dialogue.replies[i];

            btn.onClick.AddListener(() =>
            {
                DialogueManager.Instance.SelectReply(index);
            });

            activeChoiceButtons.Add(btn);
        }
    }

    private void ClearChoices()
    {
        foreach (Button btn in activeChoiceButtons)
        {
            if (btn != null) Destroy(btn.gameObject);
        }
        activeChoiceButtons.Clear();
    }

    private void OnDialogueEnd(int dialogueId, int replyIndex)
    {
        if (!DialogueManager.Instance.IsDialogueActive)
        {
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            isTyping = false;

            dialoguePanel.SetActive(false);
            choicesPanel.SetActive(false);
            ClearChoices();
        }
    }
}