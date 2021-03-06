﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    public float moveSpeed;
    public float jumpHeight;

    public float distanceToCheck;
    public Text text;
    public Text interactableText;
    public int currentLevel;

    public Dialogue[] currentDialogues;
    public Dialogue[] dialogues1;
    public Dialogue[] dialogues2;
    public Dialogue[] dialogues3;

    public Dialogue startDialogue;

    public GameObject dialogueMenu;
    public GameObject closeupMenu;

    public GameObject panelPhone;
    public GameObject panelSmoelenboek;
    public GameObject panelKwetter;
    public GameObject btnPhone;

    public GameObject[] interactablesLevel1;
    public GameObject[] interactablesLevel2;

    public DialogueManager dialogueManager;

    public Sprite background1;
    public Sprite background2;

    public Sprite spriteSmoelenboek2;
    public Sprite spriteKwetter2;

    public Sprite spriteSmoelenboek3;
    public Sprite spriteKwetter3;

    public GameObject closeUpPanel;
    public Sprite spriteVoerEnWaterBak;
    public Sprite spriteHondenMand;

    public GameObject pressButtonHint;
    public GameObject congratulationsHint;
    public Text congratulationsHintText;

    public GameObject phoneHint;

    public bool blockMovement;

    public GameObject animHolder;

    public GameObject dogSprite;

    public AudioClip clipVictory;

    public GameObject finalScreen;
    public Text finalScore;

    private Animator animCanvas;

    private GameObject closestInteractable;
    private GameObject interactableMenu;
    private GameObject[] allInteractables;

    private GameObject bg;

    private GameObject panel;

    private Animator anim;

    private bool phoneOpened;

    private AudioClip music;

    // Use this for initialization
    void Start() {
        music = GameObject.Find("Audio").GetComponent<AudioSource>().clip;
        bg = GameObject.Find("Background");
        NextLevel();
        text.enabled = false;
        closeupMenu.SetActive(false);
        panel = GameObject.Find("startDialoguePanel");
        anim = GetComponent<Animator>();
        panelPhone.SetActive(false);
        panelSmoelenboek.SetActive(false);
        panelKwetter.SetActive(false);
        phoneOpened = false;
        btnPhone.SetActive(false);
        blockMovement = false;
        congratulationsHint.SetActive(false);
        phoneHint.SetActive(false);
        animCanvas = animHolder.GetComponent<Animator>();
        dogSprite.SetActive(false);
        finalScreen.SetActive(false);
        pressButtonHint.SetActive(true);

        foreach(Dialogue dialogue in currentDialogues)
        {
            if (dialogue.dialogueContainer != null)
            {
                dialogue.dialogueContainer.Setup(null);
            }
        }

        for (int i = 0; i < allInteractables.Length; i++)
        {
            allInteractables[i].SetActive(false);
        }

        dialogueManager.StartDialogue(startDialogue.dialogueContainer);
    }
	
	// Update is called once per frame
	void Update () {
        
                // Jumping currently disabled
                /*if (Input.GetKey(KeyCode.Space) && GetComponent<Rigidbody2D>().velocity.y == 0) 
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpHeight);
                }*/
                

                if (GetComponent<Rigidbody2D>().velocity.x != 0)
                {
                    anim.SetBool("Moving", true);
                }
                else
                {
                    anim.SetBool("Moving", false);
                }

        if (!blockMovement) //If the user is inspecting an interactable or in a conversation, movement is blocked
        {
            {
                if (Input.GetKey(KeyCode.D))
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed, GetComponent<Rigidbody2D>().velocity.y);
                    GetComponent<SpriteRenderer>().flipX = true;
                }
                if (Input.GetKey(KeyCode.A))
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(-moveSpeed, GetComponent<Rigidbody2D>().velocity.y);
                    GetComponent<SpriteRenderer>().flipX = false;
                }


                for (int i = 0; i < allInteractables.Length; i++)
                {
                    allInteractables[i].SetActive(false);
                }

                closestInteractable = GetClosestInteractable();
                if (closestInteractable != null)
                {                    
                    if (closestInteractable.name.Substring(0, 2) == "i_")
                    {
                        text.text = "Inspecteer " + closestInteractable.name.Substring(2);
                        text.enabled = true;
                        closestInteractable.SetActive(true);
                    }
                    else if (closestInteractable.name.Substring(0, 2) == "c_")
                    {
                        text.text = "Praat met " + closestInteractable.name.Substring(2);
                        text.enabled = true;
                        closestInteractable.SetActive(true);
                    }
                }
                else
                {
                    text.enabled = false;
                }

                if (Input.GetKey(KeyCode.E))
                {
                    if(pressButtonHint.activeSelf)
                    {
                        pressButtonHint.SetActive(false);
                    }

                    if (closestInteractable != null)
                    {
                        if (closestInteractable.name.Substring(0, 2) == "i_")
                        {
                            interactableMenu = closestInteractable;

                            if (interactableMenu.name == "i_voer en waterbak")
                            {
                                closeUpPanel.GetComponent<Image>().sprite = spriteVoerEnWaterBak;
                                interactableText.text = "De voerbak zit vol, maar er is geen water meer.. Lucky zal wel dorst hebben!";
                                closeupMenu.SetActive(true);
                            }
                            else if (interactableMenu.name == "i_hondenmand")
                            {
                                closeUpPanel.GetComponent<Image>().sprite = spriteHondenMand;
                                interactableText.text = "Lucky ligt niet in de hondenmand.";
                                closeupMenu.SetActive(true);
                            }
                        }
                        else if (closestInteractable.name.Substring(0, 2) == "c_")
                        {
                            foreach (Dialogue dia in currentDialogues)
                            {
                                if (closestInteractable == dia.character)
                                {
                                    dialogueManager.StartDialogue(dia.dialogueContainer);
                                    closestInteractable.GetComponent<AudioSource>().Play();
                                }
                            }

                        }
                    }

                }
            }

        }
    }
    

    GameObject GetClosestInteractable()
    {
        GameObject tempClosestInteractable = null;

        for (int i = 0; i < allInteractables.Length; i++)
        {
            if (tempClosestInteractable != null)
            {
                if (Vector2.Distance(new Vector2(allInteractables[i].transform.position.x, allInteractables[i].transform.position.y), new Vector2(transform.position.x, transform.position.y)) < Vector2.Distance(new Vector2(tempClosestInteractable.transform.position.x, tempClosestInteractable.transform.position.y), new Vector2(transform.position.x, transform.position.y)))
                {
                    tempClosestInteractable = allInteractables[i];
                }
            }
            else
            {
                if (Vector2.Distance(new Vector2(allInteractables[i].transform.position.x, allInteractables[i].transform.position.y), new Vector2(transform.position.x, transform.position.y)) < distanceToCheck)
                {
                    tempClosestInteractable = allInteractables[i];
                }
            }
        }

        return tempClosestInteractable;
    }


    public void MakeChoice(int choice)
    {
        if(choice == 3)
        {
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            if (SceneManager.sceneCount > nextSceneIndex)
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
        }
    }

    public void ExitCloseUpMenu()
    {
        interactableMenu = null;
        closeupMenu.SetActive(false);
    }

    public void NextLevel()
    {
        StartCoroutine(ShowCongratulations(currentLevel));

        StartCoroutine(NewLevelSound());

        currentLevel++;       

        if(currentLevel == 1)
        {
            currentDialogues = dialogues1;
            allInteractables = interactablesLevel1;
        }
        else if(currentLevel == 2)
        {
            StartCoroutine(ShowPhoneHint());
            dogSprite.SetActive(true);
            btnPhone.SetActive(true);
            currentDialogues = dialogues2;
            allInteractables = interactablesLevel2;
            panelSmoelenboek.GetComponentInChildren<Image>().sprite = spriteSmoelenboek2;
            panelKwetter.GetComponentInChildren<Image>().sprite = spriteKwetter2;
        }
        else if (currentLevel == 3)
        {
            currentDialogues = dialogues3;
            allInteractables = interactablesLevel2;
            panelSmoelenboek.GetComponentInChildren<Image>().sprite = spriteSmoelenboek3;
            panelKwetter.GetComponentInChildren<Image>().sprite = spriteKwetter3;
        }

        foreach (Dialogue dialogue in currentDialogues)
        {
            if (dialogue.dialogueContainer != null)
            {
                dialogue.dialogueContainer.Setup(null);
            }
        }
    }

    public void OpenPhone()
    {
        Debug.Log("Open phone");
        phoneOpened = !phoneOpened;
        if(phoneOpened)
        {            
            panelPhone.SetActive(true);
            animCanvas.SetBool("phoneIsOpen", true);
        }
        else
        {
            animCanvas.SetBool("phoneIsOpen", false);
            panelPhone.SetActive(false);
            panelKwetter.SetActive(false);
            panelSmoelenboek.SetActive(false);
        }
    }

    public void OpenKwetter()
    {
        panelPhone.SetActive(false);
        panelKwetter.SetActive(true);
    }

    public void OpenSmoelenboek()
    {
        panelPhone.SetActive(false);
        panelSmoelenboek.SetActive(true);
    }

    public void ExitKwetter()
    {
        panelKwetter.SetActive(false);
        panelPhone.SetActive(true);
    }

    public void ExitSmoelenboek()
    {
        panelSmoelenboek.SetActive(false);
        panelPhone.SetActive(true);
    }

    IEnumerator ShowCongratulations(int level)
    {
        congratulationsHintText.text = level.ToString();
        congratulationsHint.SetActive(true);
        yield return new WaitForSeconds(10);
        congratulationsHint.SetActive(false);
    }

    IEnumerator ShowPhoneHint()
    {
        phoneHint.SetActive(true);
        yield return new WaitForSeconds(15);
        phoneHint.SetActive(false);
    }

    IEnumerator NewLevelSound()
    {
        AudioSource a = GameObject.Find("Audio").GetComponent<AudioSource>();
        a.clip = clipVictory;
        a.Play();
        yield return new WaitForSeconds(a.clip.length);
        a.clip = music;
        a.Play();
    }

    public void FinalScreen(int score)
    {
        finalScore.text = score.ToString();
        finalScreen.SetActive(true);
    }
}
