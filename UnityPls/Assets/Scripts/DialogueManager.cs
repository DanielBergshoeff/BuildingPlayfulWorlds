using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public Text nameText;
    public Text dialogueText;
    
    public Animator animator;
    public Button[] buttons;

    public int livesLeft;
    public GameObject player;

    public TextDialogueContainer rightChoiceLevel1;
    public TextDialogueContainer wrongChoiceLevel1;
    public TextDialogueContainer wrongChoiceLevel1_final;

    public GameObject panelKevin;
    public GameObject panelPapa;
    public GameObject panelMama;
    public GameObject panelRobin;
    public GameObject panelLucky;

    public GameObject finalScreen;
    public Text textNrOfLines;

    private DialogueContainer currentDialogueContainer;

    private Queue<string> sentences;

	// Use this for initialization
	void Start () {
        sentences = new Queue<string>();
        finalScreen.SetActive(false);
        livesLeft = 6;
	}

    public void StartDialogue(DialogueContainer dialogueContainer)
    {
        player.GetComponent<PlayerController>().blockMovement = true;

        currentDialogueContainer = dialogueContainer;        

        switch(dialogueContainer.dialogueNodeType)
        {
            case DialogueNodeType.TEXT:
                DisplayText((TextDialogueContainer)dialogueContainer);
                break;
            case DialogueNodeType.QUESTION:
                DisplayQuestion((QuestionDialogueContainer)dialogueContainer);
                break;
            case DialogueNodeType.SPECIAL:
                DisplaySpecial((SpecialQuestionDialogueContainer)dialogueContainer);
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
            buttons[i].GetComponent<Button>().onClick.RemoveAllListeners();
            buttons[i].gameObject.SetActive(true);
            yOffSet -= 60; 
        }
    }

    public void DisplayText(TextDialogueContainer textDialogue)
    {
        sentences.Clear();
        nameText.text = textDialogue.characterName;
        if(textDialogue.characterName == "Robin")
        {
            panelRobin.SetActive(true);
        }
        else if (textDialogue.characterName == "Kevin")
        {
            panelKevin.SetActive(true);
        }
        else if (textDialogue.characterName == "Papa")
        {
            panelPapa.SetActive(true);
        }
        else if (textDialogue.characterName == "Mama")
        {
            panelMama.SetActive(true);
        }
        else if (textDialogue.characterName == "Lucky")
        {
            panelLucky.SetActive(true);
        }
        animator.SetBool("IsOpen", true);
        foreach(string s in textDialogue.sentences)
        {
            sentences.Enqueue(s);
        }
        DisplayNextSentence();
    }

    public void DisplaySpecial(SpecialQuestionDialogueContainer specDialogue)
    {
        if (specDialogue.questionButtons.Length > buttons.Length)
        {
            throw new System.ArgumentException("The number of questions exceed the number of buttons available.");
        }

        float yOffSet = 30 * (specDialogue.questionButtons.Length - 1);

        for (int i = 0; i < specDialogue.questionButtons.Length; i++)
        {
            if (specDialogue.questionButtons[i].questionAsked == true)
            {
                buttons[i].GetComponent<Image>().color = Color.gray;
            }
            else
            {
                buttons[i].GetComponent<Image>().color = Color.white;
            }
            buttons[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, yOffSet);
            buttons[i].GetComponentInChildren<Text>().text = specDialogue.questionButtons[i].text;
            buttons[i].GetComponent<Button>().onClick.RemoveAllListeners();
            
        switch (i)
            {
                case 0:
                    buttons[i].onClick.AddListener(() => { SpecialQuestionAnswer(0); });
                    break;
                case 1:
                    buttons[i].onClick.AddListener(() => { SpecialQuestionAnswer(1); });
                    break;
                case 2:
                    buttons[i].onClick.AddListener(() => { SpecialQuestionAnswer(2); });
                    break;
                case 3:
                    buttons[i].onClick.AddListener(() => { SpecialQuestionAnswer(3); });
                    break;
                case 4:
                    buttons[i].onClick.AddListener(() => { SpecialQuestionAnswer(4); });
                    break;
                default:
                    buttons[i].onClick.AddListener(() => { SpecialQuestionAnswer(10); });
                    break;
            } 
            buttons[i].gameObject.SetActive(true);
            yOffSet -= 60;
        }
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
        player.GetComponent<PlayerController>().blockMovement = false;
        TextDialogueContainer textDialogueContainer = (TextDialogueContainer)currentDialogueContainer;

        panelKevin.SetActive(false);
        panelLucky.SetActive(false);
        panelMama.SetActive(false);
        panelPapa.SetActive(false);
        panelRobin.SetActive(false);

        if (textDialogueContainer.nextDialogueContainer != null)
        {
            StartDialogue(textDialogueContainer.nextDialogueContainer);
            if(textDialogueContainer.nextDialogueContainer.dialogueNodeType != DialogueNodeType.TEXT)
            {
                animator.SetBool("IsOpen", false);
            }
        }
        else
        {
            animator.SetBool("IsOpen", false);            
        }        
        Debug.Log("End of conversation.");
        
    }

    void SpecialQuestionAnswer(int nrChosen)
    {
        Debug.Log(nrChosen);
        if (player.GetComponent<PlayerController>().currentLevel == 1)
        {            
            if (nrChosen == 1)
            {
                StartDialogue(rightChoiceLevel1);
                player.GetComponent<PlayerController>().NextLevel();
                //ADD YOU WON DIALOGUE
                Debug.Log("Good choice!");
            }
            else
            {
                livesLeft--;
                if (livesLeft == 4)
                {
                    StartDialogue(wrongChoiceLevel1_final);
                    player.GetComponent<PlayerController>().NextLevel();
                    //ADD YOU FAILED DIALOGUE
                }
                else
                {
                    StartDialogue(wrongChoiceLevel1);
                }

                Debug.Log("Bad choice!");
            }
        } else if(player.GetComponent<PlayerController>().currentLevel == 2)
        {
            if (nrChosen == 4)
            {
                player.GetComponent<PlayerController>().NextLevel();
                livesLeft++;
                //ADD YOU WON DIALOGUE
                Debug.Log("Good choice!");
                textNrOfLines.text = livesLeft.ToString();
                finalScreen.SetActive(true);
            }
            else
            {
                livesLeft--;        
                Debug.Log("Bad choice!");
            }
        }
    }

    public void PickedQuestion(int questionChoice)
    {
        player.GetComponent<PlayerController>().blockMovement = false;

        foreach (Button b in buttons)
        {
            b.gameObject.SetActive(false);
        }
        if (currentDialogueContainer is QuestionDialogueContainer)
        {

            QuestionDialogueContainer questionDialogueContainer = (QuestionDialogueContainer)currentDialogueContainer;
            questionDialogueContainer.questionButtons[questionChoice].questionAsked = true;
            if (questionDialogueContainer.questionButtons[questionChoice].nextDialogueContainer != null)
            {
                StartDialogue(questionDialogueContainer.questionButtons[questionChoice].nextDialogueContainer);
            }

        }
        else
        {
            SpecialQuestionDialogueContainer specDialogueContainer = (SpecialQuestionDialogueContainer)currentDialogueContainer;
            specDialogueContainer.questionButtons[questionChoice].questionAsked = true;
            if (specDialogueContainer.questionButtons[questionChoice].nextDialogueContainer != null)
            {
                StartDialogue(specDialogueContainer.questionButtons[questionChoice].nextDialogueContainer);
            }
        }       

        
    }
}
