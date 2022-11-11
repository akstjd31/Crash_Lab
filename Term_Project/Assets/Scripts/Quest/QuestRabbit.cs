using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestRabbit : MonoBehaviour
{
    float waitTime = 0.0f; // 이동시간, 대기시간
    float lookAtObsTime = 0.0f;
    //float moveSpeed = 6f;  // 움직이는 속도
    float rotateSpeed = 10.0f; // 회전속도
    bool waitForSecond = false; // 대기상태인지 아닌지 체크
    public static bool isSafePos = false;
    Animator rabbitAnim;

    public static Vector3 randPos;
    Vector3 rabbitPos;
    // Start is called before the first frame update
    void Awake() // 초기화
    {
        rabbitAnim = gameObject.GetComponent<Animator>();
        Settings();
    }

    void Settings() // 리셋 작업
    {
        lookAtObsTime = 0.0f;
        waitTime = 0.0f;
        randPos = new Vector3(Random.Range(-15, 15), transform.position.y, Random.Range(-15, 15));
        waitForSecond = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!waitForSecond) Movement();

        CheckGoalPos();
        /* 랜덤으로 정해진 시간보다 작거나 x, z 좌표의 차이가 목표 랜덤좌표와
        가까이 있으면서 만약 장애물과 맞닿은 시간이 1.5초 이상이 되면 토끼가 멈추고 새로운 좌표를 가져온다. */
        if (Mathf.Abs(transform.position.x - randPos.x) <= 1
        && Mathf.Abs(transform.position.z - randPos.z) <= 1
        || lookAtObsTime > 1.5f)
        {
            Wait();
            if(waitTime > 1.5f) Settings();
        }
    }

    /* 토끼가 멈춰선다. */
    void Wait()
    {
        waitTime += Time.deltaTime;
        waitForSecond = true;
        rabbitAnim.SetBool("isRun", false);
    }

    /* 토끼의 이동 및 회전 */
    void Movement()
    {
        rabbitPos = transform.position;
        Vector3 vDist = randPos - rabbitPos;
        Vector3 vDir = vDist.normalized;

        rabbitAnim.SetBool("isRun", true);

        if (!(Mathf.Abs(transform.position.x - randPos.x) <= 1 && Mathf.Abs(transform.position.z - randPos.z) <= 1))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(vDir), Time.deltaTime * rotateSpeed);
        }  
    }

    /* 현재 토끼가 장애물에 맞닿는 시간을 측정 */
    private void CheckGoalPos()
    {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(rabbitPos, fwd, out hit, 2) != false
                            && hit.collider.gameObject.tag == "Obstacle") {
                lookAtObsTime += Time.deltaTime;
        }
    }

    /* 충돌 체크 및 현재 퀘스트 완료를 전달하기 위한 메소드 */
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            QuestManager.questClear = true;
            Destroy(gameObject);
        } 

        /* 토끼의 생성위치가 장애물이 없는 안전한 위치인지 확인 */
        if (col.gameObject.tag == "Obstacle" && !isSafePos)
        {
            this.gameObject.transform.position = new Vector3(Random.Range(-15, 15), 1, Random.Range(-15, 15));
        } else isSafePos = true;
    }
}
