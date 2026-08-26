using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Character 
{
    Player,
    Joe,
    Meowmad,
    Niggu
    // add new characters below - ali
}

public enum EndAction
{ 
    End, 
    Reply, 
    Action 
}

[System.Serializable]
public class Dialogue
{
    public int id;
    public List<string> messages = new List<string>();
    public List<Character> characters = new List<Character>();
    
    [Space(10)]
    public EndAction endAction;
    
    [Header("Options (if endAction is Reply)")]
    public string[] replies; 
    public int[] nextDialogueIds;

    [Header("Event to trigger (if endAction is Action)")]
    public UnityEvent onDialogueAction; 
}

[System.Serializable]
public class DialogueDatabase
{ 
    public Dialogue[] dialogues; 
}