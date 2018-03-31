using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum State { Idle, Move, Attack, Fly, Descend, Silence }

public class FollowPlayer : MonoBehaviour {

    public int moveSpeed = 3;
    public int rotationSpeed = 3;
    public int range = 10;
    public int attackRange = 1;

    public int rangeWall = 10;
    public Material transparantMat;
    public Material blueMat;


    private Transform myTransform;
    private Transform target;
    public ColorManager.ColorNames color;
    public State currentState;
    private Vector3 targetIdle;
    private bool targetIdleSet;

    

    private void Awake()
    {
        GetComponent<Renderer>().material.SetColor("_EmissionColor", ColorManager.GetColorValue(color));
        myTransform = transform;
    }

    // Use this for initialization
    void Start () {
        target = GameObject.FindWithTag("Player").transform;
        currentState = State.Idle;
	}
	
	// Update is called once per frame
	void Update () {
        var distance = Vector3.Distance(myTransform.position, target.position);
        switch (currentState)
        {
            case State.Idle:
                if (GetComponent<Renderer>().material.GetColor("_EmissionColor") == ColorManager.GetColorValue(ColorManager.ColorNames.RED))
                {
                    currentState = State.Silence;
                    break;
                }

                if (targetIdleSet == false)
                {
                    targetIdle = transform.position + new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
                    targetIdleSet = true;
                }
                else
                {
                    myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(targetIdle - myTransform.position), rotationSpeed * Time.deltaTime);
                    if (Physics.Raycast(myTransform.position, myTransform.forward - myTransform.up, 3) && !Physics.Raycast(myTransform.position, myTransform.forward, 2))
                    {
                        myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
                        if(Vector3.Distance(myTransform.position, targetIdle) < 1)
                        {
                            targetIdleSet = false;
                        }
                    }
                    else
                    {
                        targetIdleSet = false;                        
                    }
                }

                
                if (GetComponent<Renderer>().material.GetColor("_EmissionColor") != ColorManager.GetColorValue(ColorManager.ColorNames.RED))
                {
                    if (distance < range)
                    {
                        currentState = State.Move;
                    }
                }

                break;
            case State.Move:
                if (GetComponent<Renderer>().material.GetColor("_EmissionColor") == ColorManager.GetColorValue(ColorManager.ColorNames.RED))
                {
                    currentState = State.Silence;
                    break;
                }

                Vector3 vectorTarget = new Vector3(target.position.x, myTransform.position.y, target.position.z);
                myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(vectorTarget - myTransform.position), rotationSpeed * Time.deltaTime);
                RaycastHit hit;
                if (Physics.Raycast(myTransform.position, myTransform.forward - myTransform.up, out hit, 3))
                {
                    myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
                }
                if (distance >= range)
                {
                    currentState = State.Idle;
                }
                
                break;
            case State.Attack:
                break;
            case State.Fly:
                break;
            case State.Descend:
                break;
            case State.Silence:
                if (GetComponent<Renderer>().material.GetColor("_EmissionColor") != ColorManager.GetColorValue(ColorManager.ColorNames.RED))
                {
                    currentState = State.Idle;
                    break;
                }



                break;
            default:
                break;
        } 


    } 


    /*
    public int rangeWall = 10;

    private Transform myTransform;
    public ColorManager.ColorNames color;

    public State currentState;
    public int damage = 20;
    public float attackRange;
    public float maxCooldown = 1;
    public float senseRange = 10;

    private NavMeshAgent agent;
    private Health target;
    private float coolDown;
    private float distanceToTarget;
    private float distanceToGround;
    // Use this for initialization
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        distanceToGround = GetComponentInChildren<Collider>().bounds.extents.y;
        GetComponentInChildren<Renderer>().material.SetColor("_EmissionColor", ColorManager.GetColorValue(color));
        myTransform = transform;
        //player = GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {

        CheckState();

    }

    void CheckState()
    {
        if (GetComponentInChildren<Renderer>().material.GetColor("_EmissionColor") != ColorManager.GetColorValue(ColorManager.ColorNames.RED)) {
            //Sensing
            if (target == null)
            {
                distanceToTarget = float.MaxValue;
                Collider[] cols = Physics.OverlapSphere(transform.position, senseRange);
                foreach (Collider c in cols)
                {
                    if (c.gameObject == gameObject) { continue; }
                    Health hp = c.gameObject.GetComponent<Health>();
                    if (hp != null)
                    {
                        Debug.Log("Health found!");
                        float distToHealthScript = Vector3.Distance(transform.position, hp.transform.position);
                        if (distToHealthScript < distanceToTarget)
                        {
                            target = hp;
                            distanceToTarget = distToHealthScript;
                        }
                    }
                }
                if (target == null)
                {
                    currentState = State.Idle;
                }

            }
            else
            {
                distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
                if (distanceToTarget > senseRange)
                {
                    target = null;
                }
            }

        }
        else
        {
            currentState = State.Silence;
        }

        //States
        switch (currentState)
        {
            case State.Attack:
                //Action
                if (coolDown > 0)
                {
                    coolDown -= Time.deltaTime;
                }

                //Do Damage
                if (distanceToTarget < attackRange && coolDown <= 0)
                {
                    Debug.Log("Do attack!");
                    target.DoDamage(gameObject, damage);
                    coolDown = maxCooldown;
                }

                //Transition
                if (distanceToTarget > 2 * attackRange)
                {
                    currentState = State.Move;
                }

                break;
            case State.Idle:
                if (GetComponentInChildren<Renderer>().material.GetColor("_EmissionColor") == ColorManager.GetColorValue(ColorManager.ColorNames.BLUE) && IsGrounded())
                {
                    Debug.Log("Going up!");
                    if (GetComponentInChildren<Rigidbody>().useGravity)
                        GetComponentInChildren<Rigidbody>().useGravity = false;
                    GetComponentInChildren<Transform>().rotation = Quaternion.identity;
                    GetComponentInChildren<Rigidbody>().freezeRotation = true;
                    agent.GetComponentInChildren<Transform>().position = Vector3.Lerp(agent.GetComponentInChildren<Transform>().position, agent.GetComponentInChildren<Transform>().up, Time.deltaTime * 1);
                    break;
                }
                else if (GetComponentInChildren<Renderer>().material.GetColor("_EmissionColor") != ColorManager.GetColorValue(ColorManager.ColorNames.BLUE) && !IsGrounded())
                {
                    Debug.Log("Going down!");
                    if (!GetComponentInChildren<Rigidbody>().useGravity)
                    {
                        GetComponentInChildren<Rigidbody>().useGravity = true;
                        agent.GetComponentInChildren<Transform>().position = Vector3.Lerp(agent.GetComponentInChildren<Transform>().position, -agent.GetComponentInChildren<Transform>().up, Time.deltaTime * (float)0.01);
                    }
                    break;
                }

                //if we are close pick a new position to walk to
                if (agent.remainingDistance > agent.stoppingDistance)
                {
                    break;
                }
                else
                {
                    agent.SetDestination(transform.position + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5)));
                }
                if (distanceToTarget < senseRange)
                {
                    currentState = State.Move;
                }


                break;
            case State.Move:
                //Move to the target
                
                if (target != null)
                {
                    agent.SetDestination(target.transform.position);
                }


                if (distanceToTarget < attackRange)
                {
                    currentState = State.Attack;
                }

                else
                {
                    
                }

                break;

            case State.Fly:
                //Go up into the sky                

                break;

        }

    }

    public bool IsGrounded()
    {
        return Physics.Raycast(GetComponentInChildren<Transform>().position, -Vector3.up, distanceToGround + 1);
    } */
}
