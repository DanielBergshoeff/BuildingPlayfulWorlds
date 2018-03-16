using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class BouncingLaser : MonoBehaviour
{
    public float updateFrequency = 0.1f;
    public int laserDistance;
    public string bounceTag;
    public int maxBounce;
    public float sizeLaser = 0.1f;
    private float timer = 0;
    private LineRenderer mLineRenderer;

    // Use this for initialization
    void Start()
    {
        timer = 0;
        mLineRenderer = gameObject.GetComponent<LineRenderer>();
        StartCoroutine(RedrawLaser());
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= updateFrequency)
        {
            timer = 0;
            //Debug.Log("Redrawing laser");
            /* foreach (GameObject laserSplit in GameObject.FindGameObjectsWithTag(spawnedBeamTag))
                Destroy(laserSplit); */

            StartCoroutine(RedrawLaser());
        }
        timer += Time.deltaTime;
        
    }

    IEnumerator RedrawLaser()
    {
        //Debug.Log("Running");
        int laserReflected = 1; //How many times it got reflected
        int vertexCounter = 1; //How many line segments are there
        bool loopActive = true; //Is the reflecting loop active?

        Vector3 laserDirection = transform.forward; //direction of the next laser
        Vector3 lastLaserPosition = transform.localPosition; //origin of the next laser

        mLineRenderer.positionCount = 1;
        mLineRenderer.SetPosition(0, transform.position);
        mLineRenderer.startWidth = sizeLaser;
        mLineRenderer.endWidth = sizeLaser;
        RaycastHit hit;

        while (loopActive)
        {
            int layer_mask = LayerMask.GetMask("BounceLaser");
            //Debug.Log("Physics.Raycast(" + lastLaserPosition + ", " + laserDirection + ", out hit , " + laserDistance + ")");
            if (Physics.Raycast(lastLaserPosition, laserDirection, out hit, laserDistance, layer_mask) && hit.collider.gameObject.GetComponent<Renderer>().material.GetColor("_EmissionColor") == ColorManager.GetColorValue(ColorManager.ColorNames.RED))
            {
                /*Debug.Log("Bounce");*/
                laserReflected++;
                vertexCounter += 2;
                mLineRenderer.positionCount = vertexCounter;
                mLineRenderer.SetPosition(vertexCounter - 2, Vector3.MoveTowards(hit.point, lastLaserPosition, sizeLaser));
                mLineRenderer.SetPosition(vertexCounter - 1, hit.point);
                mLineRenderer.startWidth = sizeLaser;
                mLineRenderer.endWidth = sizeLaser;
                lastLaserPosition = hit.point;
                Vector3 prevDirection = laserDirection;
                laserDirection = Vector3.Reflect(laserDirection, hit.normal);
            }
            else
            {
                /*Debug.Log("No Bounce");*/
                laserReflected++;
                vertexCounter++;
                mLineRenderer.positionCount = vertexCounter;
                Vector3 lastPos = lastLaserPosition + (laserDirection.normalized * laserDistance);
                //Debug.Log("InitialPos " + lastLaserPosition + " Last Pos" + lastPos);
                mLineRenderer.SetPosition(vertexCounter - 1, lastLaserPosition + (laserDirection.normalized * laserDistance));

                loopActive = false;
            }
            if (laserReflected > maxBounce)
                loopActive = false;
        }

        yield return new WaitForEndOfFrame();
    }
}