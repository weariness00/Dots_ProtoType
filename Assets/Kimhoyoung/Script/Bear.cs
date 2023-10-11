using System;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bear : MonoBehaviour
{
    private float angle = 0f;
    public float Speed;
    public float Radian;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        angle += Time.deltaTime * Speed;
        gameObject.transform.position = new Vector3(math.cos(angle) * Radian, 0, math.sin(angle) * Radian);
    }
}
