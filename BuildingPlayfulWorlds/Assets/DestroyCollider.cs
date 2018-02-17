using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCollider : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Invoke("DestroyScript", 3);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void DestroyScript()
    {
        Destroy(GetComponent<Collider>());
    }
}
