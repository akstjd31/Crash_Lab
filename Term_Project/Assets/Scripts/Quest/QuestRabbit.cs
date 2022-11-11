using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestRabbit : MonoBehaviour
{
    float runTime, waitTime; // 이동시간, 대기시간
    int randRunTime; // 랜덤 이동시간
    float moveSpeed = 6f;  // 움직이는 속도
    float rotateSpeed = 10.0f; // 회전속도
    bool waitForSecond = false; // 대기상태인지 아닌지 체크
    Animator rabbitAnim;
    GameObject player;

    Vector3 randPos, rabbitPos;
    // Start is called before the first frame update
    void Awake() // 초기화
    {
        rabbitAnim = gameObject.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        Settings();
    }

    void Settings() // 리셋 작업
    {
        runTime = 0.0f; waitTime = 0.0f;
        randRunTime = Random.Range(3, 6);
        randPos = new Vector3(Random.Range(-14, 14), 0.5f, Random.Range(-14, 14));
        waitForSecond = false;
    }

    // Update is called once per frame
    void Update()
    {
        runTime += Time.deltaTime;
        if (!waitForSecond) Movement();

        /* 랜덤으로 정해진 시간보다 작거나 x, z 좌표의 차이가 목표 랜덤좌표와 1정도 나면 토끼가 멈춘다. */
        if ((runTime > randRunTime || Mathf.Abs(transform.position.x - randPos.x) <= 1 && Mathf.Abs(transform.position.z - randPos.z) <= 1))
        {
            Wait();
            if(waitTime > 1) Settings();
        } 
    }
    /* 토끼가 멈춰선다. */
    void Wait()
    {
        waitTime += Time.deltaTime;
        waitForSecond = true;
        rabbitAnim.SetBool("isRun", false);
        Debug.Log(waitTime);
    }
    /* 토끼의 이동 및 회전 */
    void Movement()
    {
        rabbitPos = transform.position;
        Vector3 vDist = randPos - rabbitPos;
        Vector3 vDir = vDist.normalized;

        rabbitAnim.SetBool("isRun", true);

        transform.position += vDir * moveSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(vDir), Time.deltaTime * rotateSpeed);
    }

    /* 충돌 체크 및 현재 퀘스트 완료를 전달하기 위한 메소드 */
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject == player)
        {
            QuestManager.questClear = true;
            Destroy(gameObject);
        }
    }
}
