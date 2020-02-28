﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnitchController : MonoBehaviour
{
    private CharacterController controller;
    private float baseSpeed = 1000.0f;
    private float rotSpeedX = 3.0f;
    private float rotSpeedY = 1.5f;

    public float speed;
    public Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Middle is y force
        Vector3 movement = new Vector3(moveHorizontal, 1.0f, moveVertical);
        rb.AddForce(movement * speed);
    }
}
