using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;

public class makecube : MonoBehaviour
{
    public GameObject prefab;
    public int count;

    private float deltaTime;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;

        if (fps > 60)
        {
            for (int i = 0; i < count; ++i)
            {
                var random = UnityEngine.Random.Range(-360, 360);

                Instantiate(prefab, new Vector3(math.cos(random), 0, math.sin(random)), Quaternion.Euler(0f, 0f, 0f));
            }
        }

    }
}
