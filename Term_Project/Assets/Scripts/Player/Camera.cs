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
        TargetPos = new Vector3( // �ش� �÷��̾��� ��ǥ�� �������� (0, 10, -10)�� �� ī�޶� ��ǥ�� ����
            Target.transform.position.x + offsetX,
            Target.transform.position.y + offsetY,
            Target.transform.position.z + offsetZ
            );

        transform.position = Vector3.Lerp(transform.position, TargetPos, Time.deltaTime * CameraSpeed); // Lerp = �������� �ε巴�� ��Ÿ����.
    }
}
