using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Object components
    public Rigidbody rb;

    // Magic numbers
    public int maxVelocity;
    public int maxAcceleration;
    public float tacklingProb;

    private bool falling;
    private System.Random rnd;
    private string sceneName;

    private void Awake()
    {
        falling = false;
        rnd = new System.Random();
    }

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        this.rb.velocity = Vector3.zero;
        this.rb.angularVelocity = Vector3.zero;
        rb.freezeRotation = true;
        sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "Custom_Urges_2")
        {
            maxAcceleration = 16;
            maxVelocity = 16;
        }
    }

    void FixedUpdate()
    {
        Vector3 target = LocateGoldenSnitch();
        transform.LookAt(target);
        // Only do this when the character has not been tackled
        if (falling == false)
        {
            Vector3 urges = new Vector3(0, 0, 0);
            // Collect all normal urges
            if (sceneName == "Normal_Urges")
            {
                // Collect all urges
                Vector3 urgeToSnitch = GetUrgeToSnitch();
                Vector3 urgeToAvoidPlayers = GetUrgeToAvoidPlayerCollision();

                // Apply urges
                urges += urgeToAvoidPlayers;
                urges += urgeToSnitch;

                rb.AddForce(urges * maxAcceleration * 2);
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity * 2);
            }
            else if (sceneName == "Custom_Urges_1")
            {
                // Collect all urges
                Vector3 urgeToSnitch = GetUrgeToSnitch();
                Vector3 urgeToAvoidPlayers = GetUrgeToAvoidPlayerCollision();
                Vector3 urgeTowardCrowd = GetUrgeTowardCrowd();

                float distToSnitch = Vector3.Distance(target, transform.position);
                // If this player is close to the snitch, only worry about catching it
                if (distToSnitch < 20)
                {
                    urges += urgeToSnitch;
                    rb.AddForce(urges * maxAcceleration * 4);
                    rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity * 4);
                }
                else
                { 
                    // Apply urge to swarm or avoid based on proximity
                    GameObject closestPlayer = GetClosestPlayer();
                    float dist = Vector3.Distance(closestPlayer.transform.position, transform.position);
                    if (dist < 10)
                    {
                        urges += urgeToAvoidPlayers;
                    }
                    else
                    {
                        urges += urgeTowardCrowd;
                    }
                    urges += urgeToSnitch;

                    rb.AddForce(urges * maxAcceleration * 2);
                    rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity * 2);
                }
            }
            else
            {
                // Collect all urges
                Vector3 urgeToSnitch = GetUrgeToSnitch();
                Vector3 urgeToAvoidPlayers = GetUrgeToAvoidPlayerCollision();
                Vector3 urgeTowardCrowd = GetUrgeTowardCrowd();


                float distToSnitch = Vector3.Distance(target, transform.position);
                // If this player is close to the snitch, only worry about catching it
                if (distToSnitch < 30)
                {
                    urges += urgeToSnitch;
                    if (this.tag == "Slytherin")
                    {
                        urges += GetUrgeToTackleGryffindorPlayer();
                    }            
                }
                else
                {
                    urges += urgeToSnitch;
                    // Apply urge to swarm or avoid based on proximity
                    GameObject closestPlayer = GetClosestPlayer();
                    float dist = Vector3.Distance(closestPlayer.transform.position, transform.position);
                    if (dist < 10)
                    {
                        urges += urgeToAvoidPlayers;
                    }
                    else
                    {
                        urges += urgeTowardCrowd;
                    }

                }

                rb.AddForce(urges * maxAcceleration * 2);
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity * 2);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // If they bump into another player
        if (collision.gameObject.tag == "Gryffindor" || collision.gameObject.tag == "Slytherin")
        {
            if (collision.gameObject.tag != this.tag)
            {
                if (rnd.NextDouble() < tacklingProb)
                {
                    GameObject otherPlayer = collision.gameObject;

                    // Tackle other player
                    otherPlayer.GetComponent<PlayerController>().Tackled();

                    // Simulate pushing player away from you
                    Vector3 pushForce = (transform.position - otherPlayer.transform.position).normalized * -1.0f;
                    otherPlayer.GetComponent<Rigidbody>().AddForce(pushForce * 3);

                    // Make sure the object is deleted if it is pushed off of the game map
                    Destroy(this.gameObject, 10);
                }
            }
        }

        // If they bump onto the ground
        if (collision.gameObject.name == "Ground")
        {
            Destroy(this.gameObject);
        }
    }

    // Urges
    Vector3 GetUrgeToSnitch()
    {
        var pos = GameObject.Find("Golden-Snitch").transform.position;

        Vector3 urgeToSnitch = (pos - transform.position).normalized;

        return urgeToSnitch;
    }

    Vector3 GetUrgeTowardCrowd()
    {
        GameObject closestPlayer = GetClosestPlayer();
        Vector3 urgeToCrowd = (closestPlayer.transform.position - transform.position).normalized;

        return urgeToCrowd;
    }

    Vector3 GetUrgeToAvoidPlayerCollision()
    {
        // Get urge to swarm
        GameObject closestPlayer = GetClosestPlayer();

        // Urge away from player if too close to avoid frequent collisions
        Vector3 urgeToAvoidCollision = new Vector3(0, 0, 0);
        float dist = Vector3.Distance(closestPlayer.transform.position, transform.position);
        if (dist < 10)
        {
            urgeToAvoidCollision = (closestPlayer.transform.position - transform.position).normalized * -1f;
        }

        return urgeToAvoidCollision;
    }

    Vector3 GetUrgeToTackleGryffindorPlayer()
    {
        GameObject closestGryffindorPlayer = GetClosestGryffindorPlayer();
        Vector3 urgeToTackle = (closestGryffindorPlayer.transform.position - transform.position).normalized * 3f;

        return urgeToTackle;
    }

    // Helper functions
    Vector3 LocateGoldenSnitch()
    {
        var pos = GameObject.Find("Golden-Snitch").transform.position;

        return pos;
    }

    public void Tackled()
    {
        this.rb.useGravity = true;
        this.falling = true;
    }

    public GameObject GetClosestPlayer()
    {
        // Urge to swarm
        GameObject[] allSlytherinPlayers = GameObject.FindGameObjectsWithTag("Slytherin"); //Get Slytherin
        GameObject[] allGryffindorPlayers = GameObject.FindGameObjectsWithTag("Gryffindor"); //Get Slytherin
        GameObject[] allPlayers = allSlytherinPlayers.Concat(allGryffindorPlayers).ToArray();

        GameObject tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (GameObject t in allPlayers)
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

    public GameObject GetClosestGryffindorPlayer()
    {
        GameObject[] allGryffindorPlayers = GameObject.FindGameObjectsWithTag("Gryffindor"); //Get Slytherin

        GameObject tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (GameObject t in allGryffindorPlayers)
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
