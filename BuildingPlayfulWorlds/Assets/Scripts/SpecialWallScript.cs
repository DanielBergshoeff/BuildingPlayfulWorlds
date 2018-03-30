using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialWallScript : MonoBehaviour {
    public int rangeWallCheck = 10;

    public Material transparentMat;

    private Material startMat;

	// Use this for initialization
	void Start () {
        startMat = GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
		if(CheckEnemyProximity())
        {
            if (GetComponent<Renderer>().material.name.StartsWith(startMat.name))
            {
                GetComponent<Renderer>().material = transparentMat;
                GetComponent<Collider>().enabled = false;
            }
        }
        else
        {
            if(GetComponent<Renderer>().material.name.StartsWith(transparentMat.name))
            {
                GetComponent<Renderer>().material = startMat;
                GetComponent<Collider>().enabled = true;
            }
        }
	}

    bool CheckEnemyProximity()
    {
        foreach (GameObject e in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (Vector3.Distance(transform.position, e.transform.position) < rangeWallCheck && e.GetComponent<Renderer>().material.GetColor("_EmissionColor") == ColorManager.GetColorValue(ColorManager.ColorNames.BLUE))
            {
                return true;
            }
        }

        return false;
    }
}
