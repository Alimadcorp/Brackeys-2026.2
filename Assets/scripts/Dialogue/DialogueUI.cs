using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public GameObject dialoguePanel;
    public GameObject choicesPanel;
    public TMP_Text characterNameText;
    public TMP_Text messageText;
    public Button nextButton;
    public Transform choiceButtonContainer;
    public Button choiceButtonPrefab;
    [SerializeField] private float typingSpeed = 0.03f;
    public TextMeshProUGUI prompt;

    private List<Button> activeChoiceButtons = new List<Button>();
    public static DialogueUI instance;
    private Coroutine typingCoroutine;
    private bool isTyping = false;

    private void Awake() => instance = this;

    private void OnEnable()
    {
        DialogueEvents.onDialogueProgress += OnDialogueProgress;
        DialogueEvents.onDialogueEnd += OnDialogueEnd;
        if (nextButton) nextButton.onClick.AddListener(OnNextButtonClicked);
    }

    private void OnDisable()
    {
        DialogueEvents.onDialogueProgress -= OnDialogueProgress;
        DialogueEvents.onDialogueEnd -= OnDialogueEnd;
        if (nextButton) nextButton.onClick.RemoveListener(OnNextButtonClicked);
    }

    private void Start()
    {
        if (dialoguePanel) dialoguePanel.SetActive(false);
        if (choicesPanel) choicesPanel.SetActive(false);
    }

    private void Update()
    {
        if (dialoguePanel && dialoguePanel.activeSelf && (!choicesPanel || !choicesPanel.activeSelf) && !isTyping)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
                OnNextButtonClicked();
        }
    }

    private void OnDialogueProgress(int dialogueId, int messageIndex)
    {
        Dialogue current = DialogueManager.Instance.GetCurrentDialogue();
        if (current == null) return;

        dialoguePanel.SetActive(true);
        choicesPanel.SetActive(false);
        ClearChoices();

        if (messageIndex < current.characters.Count)
            characterNameText.text = current.characters[messageIndex].ToString().Replace("_", " ");

        if (messageIndex < current.messages.Count)
        {
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            typingCoroutine = StartCoroutine(TypeText(current.messages[messageIndex]));
        }

        if (nextButton) nextButton.gameObject.SetActive(true);
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

    public void OnNextButtonClicked()
    {
        if (isTyping) return;

        Dialogue current = DialogueManager.Instance.GetCurrentDialogue();
        if (current == null) return;

        int currentIndex = DialogueManager.Instance.GetCurrentMessageIndex();
        if (currentIndex >= current.messages.Count - 1 && current.messages.Count > 0)
            RenderReplies(current);
        else
            DialogueManager.Instance.NextMessage();
    }

    private void RenderReplies(Dialogue dialogue)
    {
        if (nextButton) nextButton.gameObject.SetActive(false);
        choicesPanel.SetActive(true);
        ClearChoices();

        for (int i = 0; i < dialogue.replies.Length; i++)
        {
            int index = i;
            Button btn = Instantiate(choiceButtonPrefab, choiceButtonContainer);
            TMP_Text btnText = btn.GetComponentInChildren<TMP_Text>();
            if (btnText) btnText.text = dialogue.replies[i];

            btn.onClick.AddListener(() => DialogueManager.Instance.SelectReply(index));
            activeChoiceButtons.Add(btn);
        }
    }

    private void ClearChoices()
    {
        foreach (Button btn in activeChoiceButtons)
            if (btn) Destroy(btn.gameObject);
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