using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Object components
    public Rigidbody rb;

    // Magic numbers
    public float speed;

    private bool falling;
    private bool alive;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        falling = false;
        alive = true;
    }

    void FixedUpdate()
    {
        Vector3 target = LocateGoldenSnitch();
        if (falling == false)
        {
            rb.position = Vector3.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
            transform.LookAt(target);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // If they bump into another player
        if (collision.gameObject.tag == "Gryffindor" || collision.gameObject.tag == "Slytherin")
        {
            if (collision.gameObject.tag != this.tag)
            {
                collision.gameObject.GetComponent<PlayerController>().Tackled();
            }
        }

        // If they bump onto the ground
        if (collision.gameObject.name == "Ground")
        {
            HitGround();
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
        this.rb.position = Vector3.MoveTowards(rb.position, rb.position, speed * Time.fixedDeltaTime);
        this.rb.useGravity = true;
        this.alive = false;
        this.falling = true;
    }

    public void HitGround()
    {
        // this line is for users who accidently hit the ground
        this.Tackled();

        // Kills the game object in 2 seconds after loading the object
        Destroy(this.gameObject, 2);
    }
}
