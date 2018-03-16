using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class DialogueContainer : ScriptableObject {
    public string characterName;
    public abstract DialogueNodeType dialogueNodeType { get; }

    public abstract void Setup(DialogueContainer dialogueContainer);
}






