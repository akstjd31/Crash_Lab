using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static float moveSpeed = 10.0f;
    public float rotateSpeed = 10.0f;
    public const float MAX_SPEED = 10.0f;
    public Animator playerAnim;
    public static bool isRun = false;
    public static bool isRiding = false;

    float h, v;
    [SerializeField] private GameObject carPrefab;
    [SerializeField] private GameObject radishOnHand;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "City") CarInteraction();
        else
        {
            FlowerInteraction();
            GetRadish();
        }

        GameOver();
    }

    void FixedUpdate()
    {
        if (!QuestFlowerCollection.diggingFlower)
            Movement();

    }

    /* 플레이어 이동 */
    private void Movement()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        if (h != 0 || v != 0)
        {
            isRun = true;
            playerAnim.SetBool("isRun", true);
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

    // 채집하기
    void FlowerInteraction()
    {
        if (QuestFlowerCollection.gatherArea && Input.GetMouseButtonDown(0) && !isRun)
        {
            playerAnim.SetTrigger("isGather");
            QuestFlowerCollection.diggingFlower = true;
        }
    }

    void GetRadish()
    {
        if (QuestRabbit.getRadish) radishOnHand.SetActive(true);
        else if (QuestManager.Instance.GetQuestID() > 2) radishOnHand.SetActive(false);
    }

    // 차 모든 상호작용
    void CarInteraction()
    {
        float xDir = this.transform.position.x - carPrefab.transform.position.x;
        float zDir = this.transform.position.z - carPrefab.transform.position.z;
        if (Mathf.Abs(xDir) < 5f && Mathf.Abs(zDir) < 5f && !isRiding && !CarController.coolTimeStart)
        {
            CanvasManager.Instance.LeftSidePanelOn();
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
        }
    }
}
