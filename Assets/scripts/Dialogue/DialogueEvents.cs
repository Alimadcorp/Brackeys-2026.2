using System;

public static class DialogueEvents
{
    public static event Action<int, int> onDialogueProgress;
    public static event Action<int, int> onDialogueEnd;
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