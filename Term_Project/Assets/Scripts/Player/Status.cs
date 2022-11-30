using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Status : MonoBehaviour
{
    public static int MAX_HP = 100;         // 최대 HP
    public static int HP = 100;             // HP
    public static bool useItem = false;     // 아이템 사용여부

    private float decreaseHPTime = 0.0f;    // 체력이 천천히 줄어드는 속도를 계산하는 시간 (아이템 기준시간)
    private float decreaseTime;             // 체력이 줄어드는 속도를 계산하는 시간
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        decreaseHPTime += Time.deltaTime;

        // Forest(아이템이 존재하는 맵)에서 함수 실행
        if (SceneManager.GetActiveScene().name == "Forest") calculateHP();
        Death();
    }

    /* 기본 0.5초마다 감소, 아이템 효과는 1초마다 감소 */
    void calculateHP() // 1초마다 1의 체력을 소모
    {
        if (!useItem) decreaseTime = 0.5f;
        else decreaseTime = 1f;

        if (decreaseHPTime > decreaseTime)
        {
            HP -= 1;
            decreaseHPTime = .0f;
        }
    }

    /* 죽음 */
    void Death()
    {
        if (HP <= 0)
        {
            SceneManager.LoadScene("Gameover");
        }
    }
}
