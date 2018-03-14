using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue {

    public string name;
    public GameObject character;

    [TextArea(3, 10)]
    public string[] questions;

    [TextArea(3, 10)]
    public string[] answers;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
