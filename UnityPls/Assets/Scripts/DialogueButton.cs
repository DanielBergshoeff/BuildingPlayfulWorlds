using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueButton
{
    public bool questionAsked;
    public string text;
    public DialogueContainer nextDialogueContainer;
}
