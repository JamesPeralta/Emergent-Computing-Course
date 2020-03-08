using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

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
    private const int TOP = 150;
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
        Vector3 urges = new Vector3(0, 0, 0);

        float dist = Vector3.Distance(rb.position, target);
        if (dist < distanceToDest)
        {
            Debug.Log("Reached Position");
            GetNewRandomPosition();
        }

        // Urge towards the Golden Snitch
        Vector3 urgeToMoveRandom = (target - transform.position).normalized;

        // Urge away from the boundaries
        Vector3 urgeFromBoundaries = urgeAwayFromBoundaries();

        // Urge to run from the closest player
        GameObject[] allSlytherinPlayers = GameObject.FindGameObjectsWithTag("Slytherin"); //Get Slytherin
        GameObject[] allGryffindorPlayers = GameObject.FindGameObjectsWithTag("Gryffindor"); //Get Slytherin
        GameObject[] allPlayers = allSlytherinPlayers.Concat(allGryffindorPlayers).ToArray();
        GameObject closestPlayer = GetClosestPlayer(allPlayers);
        Vector3 urgeToRun = (closestPlayer.transform.position - transform.position).normalized * -1;

        // Add all urges
        urges += urgeToMoveRandom;
        urges += urgeFromBoundaries;
        urges += urgeToRun;

        rb.AddForce(urges * acceleration * 2);

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, speed * 2);
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

    public GameObject GetClosestPlayer(GameObject[] players)
    {
        GameObject tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (GameObject t in players)
        {
            float dist = Vector3.Distance(t.transform.position, currentPos);
            if (dist < minDist && t != this.gameObject)
            {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin;
    }
}
