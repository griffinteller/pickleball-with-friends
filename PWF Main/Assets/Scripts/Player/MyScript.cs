using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyScript : MonoBehaviour
{
    public float force = -9.8f;

    private Rigidbody _rigidbody;

    public void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        transform.Translate(new Vector3(Mathf.Cos(Time.time), 0, Mathf.Sin(Time.time)) * force);
    }
}
