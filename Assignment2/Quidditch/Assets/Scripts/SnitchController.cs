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
    public int speed;
    public int acceleration;
    public float distanceToDest;

    private Vector3 target;

    // Instance Variables
    private bool runningFromWall;
    private const int TOP = 100;
    private const int BOUNDARIES = 100;

    private void Awake()
    {
        runningFromWall = false;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        float dist = Vector3.Distance(rb.position, target);
        if (dist < distanceToDest)
        {
            Debug.Log("Reached Position");
            GetNewRandomPosition();
        }

        Vector3 dir = target - transform.position;
        dir += urgeAwayFromBoundaries();
        dir = dir.normalized;
        rb.AddForce(dir * acceleration);

        urgeAwayFromBoundaries();

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, speed);
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

    void Respawn()
    {
        float x = Random.Range(-BOUNDARIES, BOUNDARIES);
        float y = Random.Range(1, TOP);
        float z = Random.Range(-BOUNDARIES, BOUNDARIES);

        transform.position = new Vector3(x, y, z);
    }

    // AI Functions
    void GetNewRandomPosition()
    {
        float x = Random.Range(-BOUNDARIES, BOUNDARIES);
        float y = Random.Range(1, TOP);
        float z = Random.Range(-BOUNDARIES, BOUNDARIES);

        target = new Vector3(x, y, z);
    }

    Vector3 urgeAwayFromBoundaries()
    {
        float x = transform.position.x;
        float y = transform.position.y;
        float z = transform.position.z;

        // Apply urges away from Boundaries
        Vector3 totalVector = new Vector3(0, 0, 0);
        if (y > (TOP - 25))
        {
            totalVector += new Vector3(0, -1, 0);
        }
        if (y < 25)
        {
            totalVector += new Vector3(0, 1, 0);
        }
        if (x > (BOUNDARIES - 25))
        {
            totalVector += new Vector3(-1, 0, 0);
        }
        if (x < (-BOUNDARIES + 25))
        {
            totalVector += new Vector3(1, 0, 0);
        }
        if (z > (BOUNDARIES - 25))
        {
            totalVector += new Vector3(0, 0, -1);
        }
        if (z < (-BOUNDARIES + 25))
        {
            totalVector += new Vector3(0, 0, 1);
        }

        return totalVector;
    }
}
