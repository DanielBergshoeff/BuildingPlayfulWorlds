using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Text creation menu")]
public class TextDialogueContainer : DialogueContainer
{
    public string[] sentences;
    public DialogueContainer nextDialogueContainer;

    public override DialogueNodeType dialogueNodeType
    {
        get
        {
            return DialogueNodeType.TEXT;
        }
    }

    public override void Setup(DialogueContainer dialogueContainer)
    {
        if (dialogueContainer == null)
        {
            if (nextDialogueContainer != null)
            {
                nextDialogueContainer.Setup(this);
            }
        }
        else
        {
            if (dialogueContainer == nextDialogueContainer)
            {
                return;
            }
            if (nextDialogueContainer != null)
            {
                nextDialogueContainer.Setup(dialogueContainer);
            }
        }
    }
}
