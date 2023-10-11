using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SommonBear : MonoBehaviour
{
    public GameObject prefab;

    public int summonNum;
    public int minRad;
    public int maxRad;

    void Update()
    {
        for (int i = 0; i < summonNum; i++)
        {
            GameObject pref = Instantiate(prefab, Vector3.zero, Quaternion.Euler(0f, 0f, 0f));
            pref.GetComponent<Bear>().Radian = Random.Range(minRad, maxRad);
        }
    }
}