using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePanel : MonoBehaviour
{
    public Transform startPosition;
    public Transform endPosition;

    public static float currentTime = 0f; 
    float lerpTime = 1.0f; // 내려오는 시간
    bool flag = false;

    QuestManager qManager;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 퀘스트 클리어 = 이전 퀘스트 종료 시 다시 위로 올림
        MoveImage();
    }

    void MoveImage()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= lerpTime)
        {
            currentTime = lerpTime;
        }

        // 스무스스텝 계산
        float t = currentTime / lerpTime;
        t = Mathf.Sin(t * Mathf.PI * 0.5f);
        this.transform.position = Vector3.Lerp(startPosition.position, endPosition.position, t);
    }
}
