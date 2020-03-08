using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using UnityEngine.SceneManagement;

public class SnitchController : MonoBehaviour
{
    // Object components
    public Rigidbody rb;
    public SphereCollider sphereCollider;
    public ScoreUpdate score;

    // Instance Variables
    public int maxVelocity;
    public int maxAcceleration;
    public float distanceToDest;
    private Vector3 target;

    // Magic numbers
    private const int TOP = 150;
    private const int BOUNDARIES = 100;
    private string sceneName;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        GetNewRandomPosition();
        sceneName = SceneManager.GetActiveScene().name;
    }

    void FixedUpdate()
    {
        Vector3 urges = new Vector3(0, 0, 0);

        // Collect all normal urges
        if (sceneName == "Normal_Urges")
        {
            Vector3 urgeToMoveRandom = UrgeToMoveRandom();
            Vector3 urgeFromBoundaries = UrgeAwayFromBoundaries();

            // Add all urges
            urges += urgeToMoveRandom;
            urges += urgeFromBoundaries;
        }
        // In my custom urges I have an extra urge to avoid players
        else
        {
            Vector3 urgeToMoveRandom = UrgeToMoveRandom();
            Vector3 urgeFromBoundaries = UrgeAwayFromBoundaries();
            Vector3 urgeToRun = UrgeToMoveAwayFromPlayers();

            // Add all urges
            urges += urgeToMoveRandom;
            urges += urgeFromBoundaries;
            urges += urgeToRun;
        } 

        // Apply forces
        rb.AddForce(urges * maxAcceleration * 2);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity * 2);
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

    // Urges
    Vector3 UrgeToMoveAwayFromPlayers()
    {
        GameObject[] allSlytherinPlayers = GameObject.FindGameObjectsWithTag("Slytherin"); //Get Slytherin
        GameObject[] allGryffindorPlayers = GameObject.FindGameObjectsWithTag("Gryffindor"); //Get Slytherin
        GameObject[] allPlayers = allSlytherinPlayers.Concat(allGryffindorPlayers).ToArray();

        Vector3 urgeToRun = new Vector3(0, 0, 0);
        if (allPlayers.Length > 0)
        {
            GameObject closestPlayer = GetClosestPlayer(allPlayers);

            urgeToRun = (closestPlayer.transform.position - transform.position).normalized * -1;
        }

        return urgeToRun;
    }

    Vector3 UrgeToMoveRandom()
    {
        float dist = Vector3.Distance(rb.position, target);
        if (dist < distanceToDest)
        {
            Debug.Log("Reached Position");
            GetNewRandomPosition();
        }

        // Urge towards the Golden Snitch
        Vector3 urgeToMoveRandom = (target - transform.position).normalized;

        return urgeToMoveRandom;
    }
    
    Vector3 UrgeAwayFromBoundaries()
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
