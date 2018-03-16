using UnityEngine;
using System.Collections;

public class RaycastShootComplete : MonoBehaviour
{

    public int gunDamage = 1;                                           // Set the number of hitpoints that this gun will take away from shot objects with a health script
    public float fireRate = 0.25f;                                      // Number in seconds which controls how often the player can fire
    public float weaponRange = 50f;                                     // Distance in Unity units over which the player can fire
    public float hitForce = 100f;                                       // Amount of force which will be added to objects with a rigidbody shot by the player
    public Transform gunEnd;                                            // Holds a reference to the gun end object, marking the muzzle location of the gun

    private Camera fpsCam;                                              // Holds a reference to the first person camera
    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);    // WaitForSeconds object used by our ShotEffect coroutine, determines time laser line will remain visible
    private AudioSource gunAudio;                                       // Reference to the audio source which will play our shooting sound effect
    private LineRenderer laserLine;                                     // Reference to the LineRenderer component which will display our laserline
    private float nextFire;                                             // Float to store the time the player will be allowed to fire again, after firing
    public GameObject prefab;

    private Color[] colors;
    public GameObject colorGun;


    void Start()
    {
        // Get and store a reference to our LineRenderer component
        laserLine = GetComponent<LineRenderer>();

        // Get and store a reference to our AudioSource component
        gunAudio = GetComponent<AudioSource>();

        // Get and store a reference to our Camera by searching this GameObject and its parents
        fpsCam = GetComponentInParent<Camera>();

        string[] enumNames = System.Enum.GetNames(typeof(ColorManager.ColorNames));
        colors = new Color[enumNames.Length];
        int i = 0;
        foreach(ColorManager.ColorNames name in System.Enum.GetValues(typeof(ColorManager.ColorNames)))
        {
            colors[i] = ColorManager.GetColorValue(name);
            i++;            
        }

        
    }


    void Update()
    {
        // Check if the player has pressed the fire button and if enough time has elapsed since they last fired
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
        {
            // Update the time when our player can fire next
            nextFire = Time.time + fireRate;

            // Start our ShotEffect coroutine to turn our laser line on and off
            StartCoroutine(ShotEffect());

            // Create a vector at the center of our camera's viewport
            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

            // Declare a raycast hit to store information about what our raycast has hit
            RaycastHit hit;

            // Set the start position for our visual effect for our laser to the position of gunEnd
            laserLine.SetPosition(0, gunEnd.position);

            int layer_mask = LayerMask.GetMask("BounceLaser");

            // Check if our raycast has hit anything
            if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, weaponRange, layer_mask))
            {
                // Set the end position for our laser line 
                laserLine.SetPosition(1, hit.point);

                // If there was a health script attached
                if (hit.collider.gameObject.tag == "Enemy")
                {
                    /*Debug.Log(colorGun.GetComponent<Renderer>().materials[0].GetColor("_EmissionColor"));
                    Debug.Log(hit.collider.GetComponent<Renderer>().materials[0].GetColor("_EmissionColor"));*/
                    if (colorGun.GetComponent<Renderer>().materials[0].GetColor("_EmissionColor") == hit.collider.GetComponent<Renderer>().materials[0].GetColor("_EmissionColor"))
                    {

                        for (int i = 0; i < colors.Length; i++)
                        {
                            if (hit.collider.GetComponent<Renderer>().materials[0].GetColor("_EmissionColor") == (colors[i]))
                            {
                                if (i + 1 != colors.Length)
                                {
                                    hit.collider.GetComponent<Renderer>().materials[0].SetColor("_EmissionColor", colors[i + 1]);
                                }
                                else
                                {
                                    hit.collider.GetComponent<Renderer>().materials[0].SetColor("_EmissionColor", colors[0]);
                                }

                                break;
                            }
                        }
                    }


                    /* Vector3 tempPosition = hit.collider.transform.position;
                    Quaternion tempRotation = hit.collider.transform.rotation;
                    Destroy(hit.collider.gameObject);
                    Instantiate(prefab, tempPosition, tempRotation); */
                }
                else if(hit.collider.gameObject.tag == "Bounce")
                {
                    if (colorGun.GetComponent<Renderer>().materials[0].GetColor("_EmissionColor") == hit.collider.GetComponentInParent<Renderer>().materials[0].GetColor("_EmissionColor"))
                    {

                        for (int i = 0; i < colors.Length; i++)
                        {
                            if (hit.collider.GetComponentInParent<Renderer>().materials[0].GetColor("_EmissionColor") == (colors[i]))
                            {
                                if (i + 1 != colors.Length)
                                {
                                    hit.collider.GetComponentInParent<Renderer>().materials[0].SetColor("_EmissionColor", colors[i + 1]);
                                }
                                else
                                {
                                    hit.collider.GetComponentInParent<Renderer>().materials[0].SetColor("_EmissionColor", colors[0]);
                                }

                                break;
                            }
                        }
                    }
                }
                else if (hit.collider.gameObject.tag == "SmallEnemy")
                {
                    Destroy(hit.collider.gameObject);
                }

                // Check if the object we hit has a rigidbody attached
                if (hit.rigidbody != null)
                {
                    // Add force to the rigidbody we hit, in the direction from which it was hit
                    hit.rigidbody.AddForce(-hit.normal * hitForce);
                }
            }
            else
            {
                // If we did not hit anything, set the end of the line to a position directly in front of the camera at the distance of weaponRange
                laserLine.SetPosition(1, rayOrigin + (fpsCam.transform.forward * weaponRange));
            }
        }

        if(Input.GetButtonDown("Fire2")) {
            for (int i = 0; i < colors.Length; i++)
            {
                /*Debug.Log(colors[i]);*/
                if (colorGun.GetComponent<Renderer>().materials[0].GetColor("_EmissionColor") == (colors[i]))
                {
                    /*Debug.Log("Change material");*/
                    if (i + 1 != colors.Length)
                    {
                        colorGun.GetComponent<Renderer>().materials[0].SetColor("_EmissionColor", colors[i + 1]);
                        GetComponent<LineRenderer>().material.SetColor("_EmissionColor", colors[i + 1]);
                    }
                    else
                    {
                        colorGun.GetComponent<Renderer>().materials[0].SetColor("_EmissionColor", colors[0]);
                        GetComponent<LineRenderer>().material.SetColor("_EmissionColor", colors[0]);
                    }

                    break;
                }
            }
        }
    }


    private IEnumerator ShotEffect()
    {
        // Play the shooting sound effect
        gunAudio.Play();

        // Turn on our line renderer
        laserLine.enabled = true;

        //Wait for .07 seconds
        yield return shotDuration;

        // Deactivate our line renderer after waiting
        laserLine.enabled = false;
    }
}