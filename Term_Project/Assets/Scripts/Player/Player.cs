using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static float moveSpeed = 10.0f;              // 플레이어 이동 속도
    public float rotateSpeed = 10.0f;                   // 플레이어 회전 속도
    public const float MAX_SPEED = 10.0f;               // 플레이어 최대 속도
    public Animator playerAnim;                         // 플레이어 애니메이션
    public static bool isRun = false;                   // 이동 여부
    public static bool isRiding = false;                // 차 탑승 여부
    public static bool getRadish = false;
    bool flag = false;                                  // 1번 실행하기 위한 변수

    float h, v;                                         // horizontal, vertical
    [SerializeField] private GameObject carPrefab;      // 차 오브젝트
    [SerializeField] private GameObject radishOnHand;   // 채소(무) : 현재 손에 들고 있으며, 액티브를 비활성화 해둔 상태
    [SerializeField] private GameObject flowerPrefab;   // 해바라기(꽃)

    // Update is called once per frame
    void Update()
    {
        GameOver();

        if (SceneManager.GetActiveScene().name == "City") CarInteraction();
        else
        {
            FlowerInteraction();
            GetRadish();
        }
    }

    void FixedUpdate()
    {
        // 해바라기 캐는 중이 아니면 이동 가능
        if (!QuestFlowerCollection.diggingFlower)
            Movement();
    }

    /* 플레이어 이동 */
    private void Movement()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        // 이동 중
        if (!(h == 0 && v == 0))
        {
            isRun = true;
            playerAnim.SetBool("isRun", true);

            SoundManager.Instance.PlayOnWalkSound();
        }
        else
        {
            isRun = false;
            playerAnim.SetBool("isRun", false);
        }

        Vector3 dir = new Vector3(h, 0, v);

        if (!(h == 0 && v == 0))
        {
            transform.position += dir * moveSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotateSpeed);
        }
    }

    void GameOver()
    {
        if (this.gameObject.transform.position.y <= -8.0f) SceneManager.LoadScene("Gameover");
    }

    /* 현재 위치가 채집가능한 위치이며, 플레이어가 걷는 중이거나 이미 꽃을 캐는 중이 아닌 상태에서 마우스 왼쪽 버튼누르면 채집 시작 */
    void FlowerInteraction()
    {
        if (QuestFlowerCollection.gatherArea && !QuestFlowerCollection.diggingFlower
            && Input.GetMouseButtonDown(0) && !isRun)
        {
            playerAnim.SetTrigger("isGather");
            SoundManager.Instance.PlayOnFlowerSound();

            QuestFlowerCollection.diggingFlower = true;
            CanvasManager.Instance.MiniPanelOff();
        }
    }

    /* 무를 획득하면 액티브 활성화 */
    void GetRadish()
    {
        if (getRadish)
        {
            radishOnHand.SetActive(true);
        } 

        if (FindNPC.NPCGetRabbit)
        {
            radishOnHand.SetActive(false);
        }
    }

    /* 차 상호작용 */
    void CarInteraction()
    {
        float xDir = this.transform.position.x - carPrefab.transform.position.x;
        float zDir = this.transform.position.z - carPrefab.transform.position.z;

        // 현재 위치에서 차와의 거리가 가깝고, 차에 탑승하지 않은 상태이며 쿨타임이 아니라면
        if (Mathf.Abs(xDir) < 5f && Mathf.Abs(zDir) < 5f && !isRiding && !CarController.coolTimeStart)
        {
            CanvasManager.Instance.LeftSidePanelOn();
            if (!flag)
            {
                SoundManager.Instance.PlayOnSidePanelSound();
                flag = true;
            }

            // F 키 누르면 탑승
            if (Input.GetKeyDown(KeyCode.F))
            {
                this.gameObject.SetActive(false);
                this.gameObject.transform.parent = carPrefab.transform;
                isRiding = true;
            }

            if (QuestManager.Instance.GetQuestID() == 4 && isRiding)
            {
                QuestManager.Instance.questClear = true;
            }
        }
        else
        {
            CanvasManager.Instance.LeftSidePanelOff();
            CanvasManager.Instance.GetLeftSidePanel().transform.position = CanvasManager.Instance.GetLeftSidePanel().GetComponent<MovePanel>().startPosition.position;
            CanvasManager.Instance.GetLeftSidePanel().GetComponent<MovePanel>().currentTime = 0.0f;
            flag = false;
        }
    }
}
