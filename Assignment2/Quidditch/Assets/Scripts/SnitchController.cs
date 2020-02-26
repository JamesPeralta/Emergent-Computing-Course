using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnitchController : MonoBehaviour
{
    private CharacterController controller;
    private float baseSpeed = 10.0f;
    private float rotSpeedX = 3.0f;
    private float rotSpeedY = 1.5f;

    public float speed;
    public Rigidbody rb;
    float moveHorizontal;
    float moveVertical;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.AddForce(movement * speed);
    }

    //Add logic to make it collide on walls
    //void OnTriggerExit(Collider other)
    //{
    //    this.transform.Translate(Vector3.right * -moveHorizontal);
    //    this.transform.Translate(Vector3.forward * -moveVertical);
    //}
}
