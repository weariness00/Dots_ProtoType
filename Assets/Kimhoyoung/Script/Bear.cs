using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bear : MonoBehaviour
{
    public Transform target;
    public int speed;
    public static Bear Instance;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(target.position, Vector3.down, speed * Time.deltaTime);
    }
}
