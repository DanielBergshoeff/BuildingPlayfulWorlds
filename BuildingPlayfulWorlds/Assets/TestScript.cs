using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour {

    private Vector3[] spawnGrounds;
    private int currentLevel = 0;

    // Use this for initialization
    void Start () {
        currentLevel = 0;
        spawnGrounds = new Vector3[] {
                new Vector3(3, 11, 36),
                new Vector3(94, 3, -36),
                new Vector3(194, 16, -35),
            };
        Respawn();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse pressed");
            if (currentLevel != spawnGrounds.Length - 1)
            {
                currentLevel++;
                Respawn();
            }
        }
    }


    public void Respawn()
    {
        Debug.Log("Respawn");
        Debug.Log(currentLevel);
        Debug.Log(spawnGrounds[currentLevel].ToString());

        transform.position = spawnGrounds[currentLevel];

        Debug.Log(transform.name);
        Debug.Log(transform.position);
    }
}
