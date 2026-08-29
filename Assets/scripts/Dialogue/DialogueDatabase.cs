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

[System.Serializable]
public class Dialogue
{
    public int id;
    public List<string> messages = new List<string>();
    public List<Character> characters = new List<Character>();
    
    [Header("Replies")]
    public string[] replies; 
    public int[] nextDialogueIds;

    [Header("On-end-triggers")]
    public UnityEvent onDialogueAction; 
}

[System.Serializable]
public class DialogueDatabase
{ 
    public Dialogue[] dialogues; 
}