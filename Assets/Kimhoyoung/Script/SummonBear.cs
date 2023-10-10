using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SommonBear : MonoBehaviour
{
    public Bear bear;
    public Transform startPos;
    public Transform targetPos;
    public int summonNum;
    void Start()
    {
        
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            for (int i = 0; i < summonNum; i++)
            {
                var prefab = Instantiate(bear.gameObject, startPos.position, Quaternion.Euler(0f, 0f, 0f));
                prefab.GetComponent<Bear>().target = targetPos;
            }
        }
    }
}
