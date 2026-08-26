using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("Database Reference")]
    public DialogueDatabase database;

    private Dictionary<int, Dialogue> dialogueMap = new Dictionary<int, Dialogue>();
    private Dialogue currentDialogue;
    private int currentMessageIndex = 0;

    public bool IsDialogueActive { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        BuildDictionary();
    }

    private void OnEnable()
    {
        DialogueEvents.onTriggerTriggered += StartDialogue;
    }

    private void OnDisable()
    {
        DialogueEvents.onTriggerTriggered -= StartDialogue;
    }

    private void BuildDictionary()
    {
        dialogueMap.Clear();
        if (database == null || database.dialogues == null) return;

        foreach (var d in database.dialogues)
        {
            if (!dialogueMap.ContainsKey(d.id))
            {
                dialogueMap.Add(d.id, d);
            }
            else
            {
                Debug.LogWarning($"[DialogueManager] Duplicate Dialogue ID found: {d.id}");
            }
        }
    }

    public void StartDialogue(int id)
    {
        if (!dialogueMap.TryGetValue(id, out currentDialogue))
        {
            Debug.LogError($"[DialogueManager] Dialogue with ID {id} not found!");
            return;
        }

        IsDialogueActive = true;
        currentMessageIndex = 0;

        DialogueEvents.ProgressDialogue(currentDialogue.id, currentMessageIndex);
    }

    public void NextMessage()
    {
        if (!IsDialogueActive || currentDialogue == null) return;

        currentMessageIndex++;

        if (currentMessageIndex < currentDialogue.messages.Count)
        {
            DialogueEvents.ProgressDialogue(currentDialogue.id, currentMessageIndex);
        }
        else
        {
            HandleEndAction();
        }
    }

    private void HandleEndAction()
    {
        switch (currentDialogue.endAction)
        {
            case EndAction.End:
                FinishDialogue(-1);
                break;

            case EndAction.Action:
                // Run inspector-assigned UnityEvent callbacks
                currentDialogue.onDialogueAction?.Invoke();
                FinishDialogue(-1);
                break;

            case EndAction.Reply:
                // Let UI script display reply buttons; do not auto-close
                break;
        }
    }

    public void SelectReply(int replyIndex)
    {
        if (currentDialogue == null || currentDialogue.endAction != EndAction.Reply) return;

        int nextId = -1;
        if (replyIndex >= 0 && replyIndex < currentDialogue.nextDialogueIds.Length)
        {
            nextId = currentDialogue.nextDialogueIds[replyIndex];
        }

        int previousId = currentDialogue.id;
        DialogueEvents.EndDialogue(previousId, replyIndex);

        if (nextId != -1)
        {
            StartDialogue(nextId);
        }
        else
        {
            IsDialogueActive = false;
            currentDialogue = null;
        }
    }

    public void FinishDialogue(int replyIndex = -1)
    {
        int finishedId = currentDialogue != null ? currentDialogue.id : -1;
        IsDialogueActive = false;
        currentDialogue = null;

        DialogueEvents.EndDialogue(finishedId, replyIndex);
    }

    public Dialogue GetCurrentDialogue() => currentDialogue;
    public int GetCurrentMessageIndex() => currentMessageIndex;
}