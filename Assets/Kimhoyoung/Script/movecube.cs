using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class movecube : MonoBehaviour
{
    private float angle = 0f;

    public float Speed;
    
    private float x;
    private float y;
    private float z;

    // Start is called before the first frame update
    void Start()
    {
        x = transform.position.x;
        z = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        // gameObject.transform.position.x = new Vector3(math.cos(angle) * Radian, 0, math.sin(angle) * Radian);
        transform.position += new Vector3(x * Time.deltaTime * Speed, 0, z * Time.deltaTime * Speed);
    }
}