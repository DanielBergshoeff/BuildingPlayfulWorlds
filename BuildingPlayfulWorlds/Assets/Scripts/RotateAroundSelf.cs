﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundSelf : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(3, 3, 3);
    }
}
