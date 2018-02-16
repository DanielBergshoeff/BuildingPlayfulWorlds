using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    public int moveSpeed = 3;
    public int rotationSpeed = 3;
    public int range = 10;
    public int range2 = 10;


    private Transform myTransform;
    private Transform target;

    private void Awake()
    {
        myTransform = transform;
    }

    // Use this for initialization
    void Start () {
        target = GameObject.FindWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
        var distance = Vector3.Distance(myTransform.position, target.position);
        if (distance < range)
        {
            myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(target.position - myTransform.position), rotationSpeed * Time.deltaTime);
            myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
        }
    }
}
