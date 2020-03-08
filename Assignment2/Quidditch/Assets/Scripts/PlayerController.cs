using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

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
    }

    void FixedUpdate()
    {
        Vector3 target = LocateGoldenSnitch();
        transform.LookAt(target);
        // Only do this when the character has not been tackled
        if (falling == false)
        {
            // Collect all urges
            Vector3 urges = new Vector3(0, 0, 0);
            Vector3 urgeToSnitch = GetUrgeToSnitch();
            Vector3 urgeToSwarm = GetUrgeToSwarm();

            float distToSnitch = Vector3.Distance(target, transform.position);
            // If this player is close to the snitch, only worry about catching it
            if (distToSnitch < 20)
            {
                urges += urgeToSnitch;
                rb.AddForce(urges * maxAcceleration * 2);
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity * 4);
            }
            // When the player is not close to the snitch, move towards snitch and also perform swarm urges
            else 
            {
                urges += urgeToSnitch;
                urges += urgeToSwarm;
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

    Vector3 GetUrgeToSwarm()
    {
        // Urge to swarm
        GameObject[] allSlytherinPlayers = GameObject.FindGameObjectsWithTag("Slytherin"); //Get Slytherin
        GameObject[] allGryffindorPlayers = GameObject.FindGameObjectsWithTag("Gryffindor"); //Get Slytherin
        GameObject[] allPlayers = allSlytherinPlayers.Concat(allGryffindorPlayers).ToArray();

        // Get urge to swarm
        GameObject closestPlayer = GetClosestPlayer(allPlayers);
        Vector3 urgeToSwarm = (closestPlayer.transform.position - transform.position).normalized;

        // Urge away from player if too close to avoid frequent collisions
        float dist = Vector3.Distance(closestPlayer.transform.position, transform.position);
        if (dist < 10)
        {
            urgeToSwarm *= -1f;
        }

        return urgeToSwarm;
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
