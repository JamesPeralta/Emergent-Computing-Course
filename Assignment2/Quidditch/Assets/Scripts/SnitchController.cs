using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SnitchController : MonoBehaviour
{
    // Object components
    public Rigidbody rb;
    public SphereCollider sphereCollider;
    public NavMeshAgent agent;
    public ScoreUpdate score;

    // Magic numbers
    public float speed;
    public float distanceToDest;

    private Vector3 target;

    // Instance Variables
    private bool runningFromWall;

    private void Awake()
    {
        runningFromWall = false;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // If it's not running from anything
        if (runningFromWall == false)
        {
            float dist = Vector3.Distance(rb.position, target);
            if (dist < distanceToDest)
            {
                GetNewRandomPosition();
            }
            rb.position = Vector3.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        }
        else
        {
            GetNewRandomPosition();
            rb.position = Vector3.MoveTowards(rb.position, target, 1.0f * speed * Time.fixedDeltaTime);
        }
    }

    // Event function when interacting with other game objects
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Slytherin")
        {
            Respawn();
            score.SlytherinPoint();
        }
        else if (collision.gameObject.tag == "Gryffindor")
        {
            Respawn();
            score.GryffindorPoint();
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        // Prioritize runing from wall
        if (collision.gameObject.tag != "Gryffindor" && collision.gameObject.tag != "Slytherin")
        {
            runningFromWall = true;
        }
        // If it is out in space and close to an object, move away from it
        else
        {
            if (runningFromWall == false)
            {
                rb.position = Vector3.MoveTowards(rb.position, collision.transform.position, -1.0f * speed * Time.fixedDeltaTime);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        runningFromWall = false;
    }

    void Respawn()
    {
        float x = Random.Range(-49, 49);
        float y = Random.Range(1, 49);
        float z = Random.Range(-49, 49);

        transform.position = new Vector3(x, y, z);
    }

    // AI Functions
    void GetNewRandomPosition()
    {
        float x = Random.Range(-49, 49);
        float y = Random.Range(1, 49);
        float z = Random.Range(-49, 49);

        target = new Vector3(x, y, z);
    }
}
