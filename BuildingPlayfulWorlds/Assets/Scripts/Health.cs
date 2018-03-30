using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    public int health;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DoDamage(GameObject g, int damage)
    {
        Debug.Log("Player was hurt!");
        //PLAY SOUND OF HURT
        health -= damage;
    }
}
