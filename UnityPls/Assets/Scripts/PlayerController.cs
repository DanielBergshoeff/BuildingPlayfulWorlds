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

    public Dialogue[] dialogues;
    public GameObject dialogueMenu;

    private GameObject closestInteractable;
    private GameObject interactableMenu;
    private GameObject[] allInteractables;

    private GameObject panel;

    // Use this for initialization
    void Start() {
        text.enabled = false;
        interactableText.enabled = false;
        LoadInteractables();
        panel = GameObject.Find("startDialoguePanel");
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
            }
            if (Input.GetKey(KeyCode.A))
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(-moveSpeed, GetComponent<Rigidbody2D>().velocity.y);
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
                        for (int i = 0; i < closestInteractable.transform.childCount; i++)
                        {
                            closestInteractable.transform.GetChild(i).gameObject.SetActive(true);
                        }
                        interactableMenu = closestInteractable;


                        if(interactableMenu.name == "i_waterbak")
                        {
                            interactableText.text = "Er zit geen water meer in. Lucky zal wel dorst hebben!";
                        }
                        else if(interactableMenu.name == "i_voerbak")
                        {
                            interactableText.text = "De voerbak zit vol.";
                        }
                        else if (interactableMenu.name == "i_hondenmand")
                        {
                            interactableText.text = "Lucky ligt niet in de hondenmand.";
                        }

                        interactableText.alignment = TextAnchor.LowerCenter;
                        interactableText.enabled = true;
                    }
                    else if (closestInteractable.name.Substring(0, 2) == "c_")
                    {
                        dialogueMenu.SetActive(true);
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

    public void StartDialogue(int choice)
    {
        dialogueMenu.SetActive(false);
        foreach (Dialogue dia in dialogues) {
            if(closestInteractable == dia.character)
            {
                FindObjectOfType<DialogueManager>().StartDialogue(dia, choice);
            }
        }
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
}
