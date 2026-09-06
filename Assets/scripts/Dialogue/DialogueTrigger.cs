using TMPro;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DialogueTrigger : MonoBehaviour
{
    [Header("Sequence of dialogues")]
    public int[] dialogueIds;

    [Header("Which ones should trigger only once?")]
    public bool[] triggerOnces;

    [Header("State Pointer")]
    [SerializeField] private int nextIndex = 0;

    private bool[] hasTriggered;
    private bool playerHere = false;

    private void Awake()
    {
        if (dialogueIds != null)
        {
            hasTriggered = new bool[dialogueIds.Length];
        }
    }

    private void Update()
    {
        DialogueUI.instance.prompt.gameObject.SetActive(playerHere);

        if (playerHere && Input.GetKeyDown(KeyCode.E))
        {
            TriggerCurrentDialogue();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) playerHere = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) playerHere = false;
    }

    private void TriggerCurrentDialogue()
    {
        if (dialogueIds == null || dialogueIds.Length == 0) return;

        if (nextIndex < 0 || nextIndex >= dialogueIds.Length)
        {
            Debug.LogWarning($"[DialogueTrigger on {gameObject.name}] nextIndex {nextIndex} is out of bounds!");
            return;
        }

        bool shouldTriggerOnce = (nextIndex < triggerOnces.Length) && triggerOnces[nextIndex];

        // Skip if marked triggerOnce and already triggered
        if (shouldTriggerOnce && hasTriggered[nextIndex]) return;

        if (shouldTriggerOnce) hasTriggered[nextIndex] = true;

        DialogueEvents.TriggerDialogue(dialogueIds[nextIndex]);
    }

    public void Increment()
    {
        nextIndex++;
    }

    public void SetNext(int index)
    {
        nextIndex = index;
    }
}