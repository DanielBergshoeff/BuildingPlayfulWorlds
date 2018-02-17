using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour {

    public GameObject prefab;

    public Texture2D crosshairImage;

    public int range;



    private Camera fpsCam;
    

    private void OnGUI()
    {
        float xMin = (Screen.width / 2) - (crosshairImage.width / 2);
        float yMin = (Screen.height / 2) - (crosshairImage.height / 2);
        GUI.DrawTexture(new Rect(xMin, yMin, crosshairImage.width, crosshairImage.height), crosshairImage);
    }

    // Use this for initialization
    void Start () {
        fpsCam = GameObject.Find("FirstPersonCharacter").GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1")){
            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(.5f, .5f, 0));
            RaycastHit hit;
            if(Physics.Raycast (rayOrigin, fpsCam.transform.forward, out hit, range))
            {
                if(hit.collider.gameObject.tag == "Enemy")
                {
                    Vector3 tempPosition = hit.collider.transform.position; 
                    Quaternion tempRotation = hit.collider.transform.rotation;                    
                    Destroy(hit.collider.gameObject);
                    Instantiate(prefab, tempPosition, tempRotation);
                }
                else if(hit.collider.gameObject.tag == "SmallEnemy")
                {
                    Destroy(hit.collider.gameObject);
                }
            }
        }
	}
}
