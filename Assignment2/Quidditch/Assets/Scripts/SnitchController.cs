using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SnitchController : MonoBehaviour
{
    // Object components
    public Rigidbody rb;
    public NavMeshAgent agent;

    // Magic numbers
    public float speed;
    public float distanceToDest;

    private Vector3 target;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float dist = Vector3.Distance(rb.position, target);
        if (dist < distanceToDest)
        {
            GetNewRandomPosition();
        }

        rb.position = Vector3.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
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
