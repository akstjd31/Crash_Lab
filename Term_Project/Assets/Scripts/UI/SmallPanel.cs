using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmallPanel : MonoBehaviour
{
    [SerializeField] private Text smallText;
    public Transform startPosition;
    public Transform endPosition;

    public float currentTime = 0f;
    float lerpTime = 1.0f;

    private static SmallPanel instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public static SmallPanel Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
       smallText.alignment = TextAnchor.MiddleCenter;
    }

    // Update is called once per frame
    void Update()
    {
        OnText();
        MoveImage();
    }

    void OnText()
    {
        if (!Player.isRiding)
        {
            smallText.text = "F 눌러서 탑승하기";
        }
        else
        {
            smallText.text = "F 눌러서 하차하기";
        }
    }

    void MoveImage()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= lerpTime)
        {
            currentTime = lerpTime;
        }

        // 스무스 스텝 계산
        float t = currentTime / lerpTime;
        t = Mathf.Sin(t * Mathf.PI * 0.5f);
        this.transform.position = Vector3.Lerp(startPosition.position, endPosition.position, t);
    }
}
