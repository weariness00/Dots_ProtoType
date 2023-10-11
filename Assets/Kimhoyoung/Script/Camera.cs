using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform target; // 카메라가 따라다닐 대상(플레이어)의 Transform 컴포넌트

    public Vector3 offset; // 카메라와 대상 간의 상대적인 위치 오프셋

    void Update()
    {
        if (target != null)
        {
            // 플레이어의 위치에 오프셋을 더한 값을 카메라의 위치로 설정
            transform.position = target.position + offset;

            // 플레이어를 항상 바라보게 하려면 LookAt 메서드 사용
            transform.LookAt(target.position);
        }
    }
}
