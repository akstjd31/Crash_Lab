using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class QuestRabbit : MonoBehaviour
{
    float waitTime = 0.0f; // 이동시간, 대기시간
    float lookAtObsTime = 0.0f;
    //float moveSpeed = 6f;  // 움직이는 속도
    //float rotateSpeed = 10.0f; // 회전속도
    bool waitForSecond = false; // 대기상태인지 아닌지 체크
    bool rabbitFollowMe = false;

    public static bool isSafePos = false;
    public static bool getRadish = false;
    
    Animator rabbitAnim;

    public NavMeshAgent rabbitAgent;
    public GameObject radishPrefab;
    GameObject target; // player 좌표

    public static Vector3 randPos;
    Vector3  rabbitPos;
    // Start is called before the first frame update
    void Awake() // 초기화 및 생성
    {
        rabbitAnim = GetComponent<Animator>();
        rabbitAgent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player");
        SpawnRadish();
        Settings();
    }

    void SpawnRadish()
    {
        Instantiate(radishPrefab, new Vector3(Random.Range(-10, 10), 1.5f, Random.Range(-10, 10)), Quaternion.identity);
    }

    void Settings() // 리셋 작업
    {
        lookAtObsTime = 0.0f;
        waitTime = 0.0f;
        //randPos = new Vector3(Random.Range(-10, 10), 1, Random.Range(-10, 10)); 
        randPos = new Vector3(Random.Range(-55, 80), transform.position.y, Random.Range(-70, 110));
        waitForSecond = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(rabbitFollowMe);
        if (!waitForSecond)
        {
            Movement();
            if (!rabbitFollowMe) CheckGoalPos();
        }

        /* 랜덤으로 정해진 시간보다 작거나 x, z 좌표의 차이가 목표 랜덤좌표와
        가까이 있으면서 만약 장애물과 맞닿은 시간이 1.5초 이상이 되면 토끼가 멈추고 새로운 좌표를 가져온다. */
        if (!rabbitFollowMe)
        {
            if (Mathf.Abs(transform.position.x - randPos.x) <= 1.5f
                && Mathf.Abs(transform.position.z - randPos.z) <= 1.5f
                || lookAtObsTime > 1.5f)
            {
                Wait();
                if (waitTime > 1.0f) Settings();
            }
        }
        if (getRadish) InvitationRabbit();
    }

    void LateUpdate()
    {
        isSafePos = true;
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
        rabbitAnim.SetBool("isRun", true);

        //this.transform.LookAt(randPos);
        if (rabbitFollowMe) rabbitAgent.SetDestination(target.transform.position);
        else rabbitAgent.SetDestination(randPos);
    }

    /* 현재 토끼가 장애물에 맞닿는 시간을 측정 */
    void CheckGoalPos()
    {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(rabbitPos, fwd, out hit, 2) != false
                            && hit.collider.gameObject.tag == "Obstacle")
        {
            lookAtObsTime += Time.deltaTime;
        }
    }

    /* 토끼랑 먹이가 근접했을 때 실행 */
    void InvitationRabbit()
    {
        Vector3 vDir = this.transform.position - target.transform.position;

        if (Mathf.Abs(vDir.x) < 7f && Mathf.Abs(vDir.z) < 7f)
        {
            rabbitFollowMe = true;
        }
        else rabbitFollowMe = false;
    }

    /* 충돌 체크 및 현재 퀘스트 완료를 전달하기 위한 메소드 */
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "NPC")
        {
            FindNPC.isGetRabbit = true;
            Destroy(gameObject);
        } 
    }

    private void OnCollisionStay(Collision col)
    {
        /* 토끼의 생성위치가 장애물이 없는 안전한 위치인지 확인 */
        if (col.gameObject.tag == "Obstacle" && !isSafePos)
        {
            //gameObject.transform.position = new Vector3(Random.Range(-10, 10), 1, Random.Range(-10, 10));
            gameObject.transform.position = new Vector3(Random.Range(-110, 105), 4, Random.Range(-68, 100));
        }
        else isSafePos = true;
    }
}
