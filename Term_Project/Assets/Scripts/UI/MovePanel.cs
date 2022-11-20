using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePanel : MonoBehaviour
{
    public Transform startPosition;
    public Transform endPosition;

    public static float currentTime = 0f; 
    float lerpTime = 1.0f; // �������� �ð�
    bool flag = false;

    QuestManager qManager;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // ����Ʈ Ŭ���� = ���� ����Ʈ ���� �� �ٽ� ���� �ø�
        MoveImage();
    }

    void MoveImage()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= lerpTime)
        {
            currentTime = lerpTime;
        }

        // ���������� ���
        float t = currentTime / lerpTime;
        t = Mathf.Sin(t * Mathf.PI * 0.5f);
        this.transform.position = Vector3.Lerp(startPosition.position, endPosition.position, t);
    }
}
