using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject Target;

    public float offsetX = 0.0f;
    public float offsetY = 10.0f;
    public float offsetZ = -10.0f;

    public float CameraSpeed = 10.0f;
    Vector3 TargetPos;
    void FixedUpdate()
    {
        TargetPos = new Vector3( // 해당 플레이어의 좌표를 기준으로 (0, 10, -10)을 한 카메라 좌표를 설정
            Target.transform.position.x + offsetX,
            Target.transform.position.y + offsetY,
            Target.transform.position.z + offsetZ
            );

        transform.position = Vector3.Lerp(transform.position, TargetPos, Time.deltaTime * CameraSpeed); // Lerp = 움직임을 부드럽게 나타내기.
    }
}
