using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Object components
    public Rigidbody rb;

    // Magic numbers
    public float speed;
    public float distanceToDest;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 target = LocateGoldenSnitch();
        rb.position = Vector3.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        transform.LookAt(target);
    }

    // AI Functions
    Vector3 LocateGoldenSnitch()
    {
        var pos = GameObject.Find("Golden-Snitch").transform.position;

        return pos;
    }
}
