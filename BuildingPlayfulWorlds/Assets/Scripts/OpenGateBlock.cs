﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGateBlock : MonoBehaviour {

    public GameObject gate;
    public bool gateOpen;
    public bool laserTouched;
    public bool upOrSide;
    public float distanceUp;
    public float speedUp;

    private Vector3 target;
    private Vector3 originalPosition;

    void Start ()
    {
        originalPosition = gate.transform.position;
        if (!upOrSide)
        {
            target = gate.transform.position + gate.transform.up * distanceUp;
        }
        else
        {
            target = gate.transform.position + gate.transform.right * distanceUp;
        }
    }
	
	// Update is called once per frame
	void Update () {
		if(laserTouched)
        {
            gate.transform.position = Vector3.Lerp(gate.transform.position, target, Time.deltaTime * speedUp);
            if(gate.transform.position == target)
            {
                gateOpen = true;
            }
            //IF FIRST TIME, PUT CAMERA ON GATE
        }
        else if(!laserTouched)
        {
            gate.transform.position = Vector3.Lerp(gate.transform.position, originalPosition, Time.deltaTime * speedUp);
            if(gate.transform.position == originalPosition)
            {
                gateOpen = false;
            }
        }

        laserTouched = false;
	}
}
