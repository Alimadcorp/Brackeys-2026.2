using System;

public static class DialogueEvents
{
    // Fires when a dialogue message updates: (Dialogue ID, Message Index)
    public static event Action<int, int> onDialogueProgress;

    // Fires when dialogue ends: (Dialogue ID, Selected Reply Index or -1)
    public static event Action<int, int> onDialogueEnd;

    // Call this to kick off a dialogue sequence by ID
    public static event Action<int> onTriggerTriggered;

    public static void TriggerDialogue(int dialogueId)
    {
        onTriggerTriggered?.Invoke(dialogueId);
    }

    public static void ProgressDialogue(int dialogueId, int messageIndex)
    {
        onDialogueProgress?.Invoke(dialogueId, messageIndex);
    }

    public static void EndDialogue(int dialogueId, int replyIndex = -1)
    {
        onDialogueEnd?.Invoke(dialogueId, replyIndex);
    }
}