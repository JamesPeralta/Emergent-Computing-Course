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
        if (collision.gameObject.tag == "Gryffindor" || collision.gameObject.tag == "Slytherin")
        {
            if (collision.gameObject.tag != this.tag)
            {
                collision.gameObject.GetComponent<PlayerController>().tackled();
            }
        }
    }

    // AI Functions
    Vector3 LocateGoldenSnitch()
    {
        var pos = GameObject.Find("Golden-Snitch").transform.position;

        return pos;
    }

    public void tackled()
    {
        this.rb.position = Vector3.MoveTowards(rb.position, rb.position, speed * Time.fixedDeltaTime);
        this.rb.useGravity = true;
        this.alive = false;
        this.falling = true;
    }
}
