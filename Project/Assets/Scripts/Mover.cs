using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 0.0f;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }
}
