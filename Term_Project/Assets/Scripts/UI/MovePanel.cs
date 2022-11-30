using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MovePanel : MonoBehaviour
{
    public Transform startPosition, endPosition;

    public float currentTime = 0f;  // 현재 시간
    float lerpTime = 1.0f;          // 판넬 내려오는 시간

    bool flag = false;              // 한번만 사용될 변수
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveImage();
    }

    void MoveImage()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= lerpTime)
        {
            currentTime = lerpTime;
        }

        // lerpTime의 절반 시간에 사운드 재생
        if (currentTime >= lerpTime / 2 && !flag)
        {
            SoundManager.Instance.PlayOnMainPanelSound();
            flag = true;
        }

        // 스무스 스텝 계산
        float t = currentTime / lerpTime;
        t = Mathf.Sin(t * Mathf.PI * 0.5f);
        this.transform.position = Vector3.Lerp(startPosition.position, endPosition.position, t);
    }
}
