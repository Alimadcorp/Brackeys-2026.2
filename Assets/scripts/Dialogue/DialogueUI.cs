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

    private List<Button> activeChoiceButtons = new List<Button>();
    public static DialogueUI instance;

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
            characterNameText.text = current.characters[messageIndex].ToString();
        }

        if (messageIndex < current.messages.Count)
        {
            messageText.text = current.messages[messageIndex];
        }

        if (nextButton != null) nextButton.gameObject.SetActive(true);
    }

    private void OnNextButtonClicked()
    {
        Dialogue current = DialogueManager.Instance.GetCurrentDialogue();
        if (current == null) return;

        int currentIndex = DialogueManager.Instance.GetCurrentMessageIndex();

        // If on the final message and end action is a reply, render choice buttons
        if (currentIndex >= current.messages.Count - 1 && current.endAction == EndAction.Reply)
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
            dialoguePanel.SetActive(false);
            choicesPanel.SetActive(false);
            ClearChoices();
        }
    }
}