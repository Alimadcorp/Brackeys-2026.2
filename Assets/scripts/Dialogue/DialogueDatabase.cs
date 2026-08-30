using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Character 
{
    Zach,
    Ryan,
    Narrator,
    Mother,
    Poor_Soul,
    Overseer_4,
    Overseer_9,
    Random_Kid,
    Lesli,
    Bobby,
    Amy,
    Bluy,
    // add new characters below - 0
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
    public List<UnityEvent> onDialogueAction; 
}

[System.Serializable]
public class DialogueDatabase
{ 
    public Dialogue[] dialogues; 
}