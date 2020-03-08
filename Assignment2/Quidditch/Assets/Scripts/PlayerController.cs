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
    public int speed;
    public int acceleration;
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
        if (falling == false)
        {
            Vector3 urges = new Vector3(0, 0, 0);
            // Urge towards Golden Snitch
            Vector3 urgeToSnitch = (LocateGoldenSnitch() - transform.position).normalized;

            // Urge to swarm
            GameObject[] allSlytherinPlayers = GameObject.FindGameObjectsWithTag("Slytherin"); //Get Slytherin
            GameObject[] allGryffindorPlayers = GameObject.FindGameObjectsWithTag("Gryffindor"); //Get Slytherin
            GameObject[] allPlayers = allSlytherinPlayers.Concat(allGryffindorPlayers).ToArray();

            // Get urge to swarm
            GameObject closestPlayer = GetClosestPlayer(allPlayers);
            Vector3 urgeToSwarm = (closestPlayer.transform.position - transform.position).normalized;

            // Urge away if too close
            float dist = Vector3.Distance(closestPlayer.transform.position, transform.position);
            if (dist < 10)
            {
                urgeToSwarm *= -1f;
            }

            float distToSnitch = Vector3.Distance(LocateGoldenSnitch(), transform.position);
            if (distToSnitch < 10)
            {
                urges += urgeToSnitch;

                // Apply the force
                rb.AddForce(urges * acceleration * 2);

                // Can get up to 2 times the speed when close to the snitch
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, speed * 4);
            }
            // When the player is not close to the snitch
            else 
            {
                urges += urgeToSnitch;
                urges += urgeToSwarm;

                // Apply the force
                rb.AddForce(urges * acceleration * 2);

                rb.velocity = Vector3.ClampMagnitude(rb.velocity, speed * 2);
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
                    collision.gameObject.GetComponent<PlayerController>().Tackled();
                }
            }
        }

        // If they bump onto the ground
        if (collision.gameObject.name == "Ground")
        {
            Destroy(this.gameObject);
        }
    }

    // AI Functions
    Vector3 LocateGoldenSnitch()
    {
        var pos = GameObject.Find("Golden-Snitch").transform.position;

        return pos;
    }

    public void Tackled()
    {
        this.rb.velocity = Vector3.zero;
        this.rb.angularVelocity = Vector3.zero;
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
