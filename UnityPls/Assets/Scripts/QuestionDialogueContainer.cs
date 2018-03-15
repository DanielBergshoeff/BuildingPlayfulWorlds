using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class QuestionDialogueContainer : DialogueContainer
{
    public DialogueButton[] questionButtons;

    public override DialogueNodeType dialogueNodeType
    {
        get
        {
            return DialogueNodeType.QUESTION;
        }
    }

    public override void Setup(DialogueContainer dialogueContainer)
    {
        foreach (DialogueButton btn in questionButtons)
        {
            btn.questionAsked = false;
            if (dialogueContainer == null)
            {
                if (btn.nextDialogueContainer != null)
                {
                    btn.nextDialogueContainer.Setup(this);
                }
            }
            else
            {
                if (dialogueContainer == btn.nextDialogueContainer)
                {
                    break;
                }
                if (btn.nextDialogueContainer != null)
                {
                    btn.nextDialogueContainer.Setup(dialogueContainer);
                }
            }
        }
    }
}
