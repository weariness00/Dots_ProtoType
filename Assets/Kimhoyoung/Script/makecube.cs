using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class makecube : MonoBehaviour
{
    public GameObject prefab;
    public int count;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < count; ++i)
        {
            Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.Euler(0f, 0f, 0f));
        }
        
    }
}
