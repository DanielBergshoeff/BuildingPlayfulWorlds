using System.Collections;
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

    public Dialogue startDialogue;

    public GameObject dialogueMenu;
    public GameObject closeupMenu;

    public GameObject panelPhone;
    public GameObject panelSmoelenboek;
    public GameObject panelKwetter;
    public GameObject btnPhone;

    public DialogueManager dialogueManager;

    public Sprite background1;
    public Sprite background2;

    public GameObject closeUpPanel;
    public Sprite spriteVoerEnWaterBak;
    public Sprite spriteHondenMand;

    public GameObject animHolder;

    private Animator animCanvas;

    private GameObject closestInteractable;
    private GameObject interactableMenu;
    private GameObject[] allInteractables;

    private GameObject bg;

    private GameObject panel;

    private Animator anim;

    private bool phoneOpened;

    // Use this for initialization
    void Start() {
        bg = GameObject.Find("Background");
        NextLevel();
        text.enabled = false;
        closeupMenu.SetActive(false);
        LoadInteractables();
        panel = GameObject.Find("startDialoguePanel");
        anim = GetComponent<Animator>();
        panelPhone.SetActive(false);
        panelSmoelenboek.SetActive(false);
        panelKwetter.SetActive(false);
        phoneOpened = false;
        btnPhone.SetActive(false);
        animCanvas = animHolder.GetComponent<Animator>();

        foreach(Dialogue dialogue in currentDialogues)
        {
            if (dialogue.dialogueContainer != null)
            {
                dialogue.dialogueContainer.Setup(null);
            }
        }

        dialogueManager.StartDialogue(startDialogue.dialogueContainer);
    }
	
	// Update is called once per frame
	void Update () {
        if (interactableMenu != null) //If the user is inspecting an interactable, movement is restricted until the user presses C
        {
            if (Input.GetKey(KeyCode.C))
            {
                for (int i = 0; i < interactableMenu.transform.childCount; i++)
                {
                    interactableMenu.transform.GetChild(i).gameObject.SetActive(false);
                }
                interactableMenu = null;
                interactableText.enabled = false;
            }
        }
        else
        {
            // Jumping currently disabled
            /*if (Input.GetKey(KeyCode.Space) && GetComponent<Rigidbody2D>().velocity.y == 0) 
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpHeight);
            }*/
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
           
            if(GetComponent<Rigidbody2D>().velocity.x != 0)
            {
                anim.SetBool("Moving", true);
            }  
            else
            {
                anim.SetBool("Moving", false);
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
                else if(closestInteractable.name.Substring(0, 2) == "c_")
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
                if (closestInteractable != null)
                {
                    if (closestInteractable.name.Substring(0, 2) == "i_")
                    {
                        interactableMenu = closestInteractable;

                        if(interactableMenu.name == "i_voer en waterbak")
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
                            }
                        }
                        
                    }
                }
                
            }

        }


    }

    void LoadInteractables()
    {
        allInteractables = GameObject.FindGameObjectsWithTag("Interactable");
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
        currentLevel++;

        if(currentLevel == 1)
        {
            /*bg.GetComponent<SpriteRenderer>().sprite = background1; */
            currentDialogues = dialogues1;
        }
        else if(currentLevel == 2)
        {
            /*bg.GetComponent<SpriteRenderer>().sprite = background2; */
            btnPhone.SetActive(true);
            currentDialogues = dialogues2;
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
}
