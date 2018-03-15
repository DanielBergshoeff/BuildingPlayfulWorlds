using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public Text nameText;
    public Text dialogueText;
    
    public Animator animator;
    public Button[] buttons;

    private DialogueContainer currentDialogueContainer;

    private Queue<string> sentences;

	// Use this for initialization
	void Start () {
        sentences = new Queue<string>();
	}

    public void StartDialogue(DialogueContainer dialogueContainer)
    {
        currentDialogueContainer = dialogueContainer;        

        switch(dialogueContainer.dialogueNodeType)
        {
            case DialogueNodeType.TEXT:
                DisplayText((TextDialogueContainer)dialogueContainer);
                break;
            case DialogueNodeType.QUESTION:
                DisplayQuestion((QuestionDialogueContainer)dialogueContainer);
                break;
            default:
                throw new System.NotImplementedException();
        }

        
    }

    public void DisplayQuestion(QuestionDialogueContainer questionDialogue)
    {
        if(questionDialogue.questionButtons.Length > buttons.Length)
        {
            throw new System.ArgumentException("The number of questions exceed the number of buttons available.");
        }

        float yOffSet = 30 * (questionDialogue.questionButtons.Length - 1);

        for(int i = 0; i < questionDialogue.questionButtons.Length; i++)
        {
            if(questionDialogue.questionButtons[i].questionAsked == true)
            {
                buttons[i].GetComponent<Image>().color = Color.gray;
            }
            else
            {
                buttons[i].GetComponent<Image>().color = Color.white;
            }
            buttons[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, yOffSet);
            buttons[i].GetComponentInChildren<Text>().text = questionDialogue.questionButtons[i].text;
            buttons[i].gameObject.SetActive(true);
            yOffSet -= 60; 
        }
    }

    public void DisplayText(TextDialogueContainer textDialogue)
    {
        sentences.Clear();
        nameText.text = textDialogue.characterName;
        animator.SetBool("IsOpen", true);
        foreach(string s in textDialogue.sentences)
        {
            sentences.Enqueue(s);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
   { 
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
    }

    void EndDialogue()
    {
        TextDialogueContainer textDialogueContainer = (TextDialogueContainer)currentDialogueContainer;
        if(textDialogueContainer.nextDialogueContainer != null)
        {
            StartDialogue(textDialogueContainer.nextDialogueContainer);
        }
        Debug.Log("End of conversation.");
        animator.SetBool("IsOpen", false);
    }

    public void PickedQuestion(int questionChoice)
    {
        foreach(Button b in buttons)
        {
            b.gameObject.SetActive(false);
        }
        QuestionDialogueContainer questionDialogueContainer = (QuestionDialogueContainer)currentDialogueContainer;

        questionDialogueContainer.questionButtons[questionChoice].questionAsked = true;

        if(questionDialogueContainer.questionButtons[questionChoice].nextDialogueContainer != null)
        {
            StartDialogue(questionDialogueContainer.questionButtons[questionChoice].nextDialogueContainer);
        }
    }
}
