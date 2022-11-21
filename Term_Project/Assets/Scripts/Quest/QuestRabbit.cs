using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class QuestRabbit : MonoBehaviour
{
    float waitTime = 0.0f; // �̵��ð�, ���ð�
    float lookAtObsTime = 0.0f;
    //float moveSpeed = 6f;  // �����̴� �ӵ�
    //float rotateSpeed = 10.0f; // ȸ���ӵ�
    bool waitForSecond = false; // ���������� �ƴ��� üũ
    bool rabbitFollowMe = false;

    public static bool isSafePos = false;
    public static bool getRadish = false;
    
    Animator rabbitAnim;

    public NavMeshAgent rabbitAgent;
    public GameObject radishPrefab;
    GameObject target; // player ��ǥ

    public static Vector3 randPos;
    Vector3  rabbitPos;
    // Start is called before the first frame update
    void Awake() // �ʱ�ȭ �� ����
    {
        rabbitAnim = GetComponent<Animator>();
        rabbitAgent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player");
        SpawnRadish();
        Settings();
    }

    void SpawnRadish()
    {
        Instantiate(radishPrefab, new Vector3(Random.Range(-110, 105), 4f, Random.Range(-68, 100)), Quaternion.identity);
        //Instantiate(radishPrefab, new Vector3(Random.Range(-10, 10), 1.5f, Random.Range(-10, 10)), Quaternion.identity);
    }

    void Settings() // ���� �۾�
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

        /* �������� ������ �ð����� �۰ų� x, z ��ǥ�� ���̰� ��ǥ ������ǥ��
        ������ �����鼭 ���� ��ֹ��� �´��� �ð��� 1.5�� �̻��� �Ǹ� �䳢�� ���߰� ���ο� ��ǥ�� �����´�. */
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

    /* �䳢�� ���缱��. */
    void Wait()
    {
        waitTime += Time.deltaTime;
        waitForSecond = true;
        rabbitAnim.SetBool("isRun", false);
    }

    /* �䳢�� �̵� �� ȸ�� */
    void Movement()
    {
        rabbitPos = transform.position;
        rabbitAnim.SetBool("isRun", true);

        //this.transform.LookAt(randPos);
        if (rabbitFollowMe) rabbitAgent.SetDestination(target.transform.position);
        else rabbitAgent.SetDestination(randPos);
    }

    /* ���� �䳢�� ��ֹ��� �´�� �ð��� ���� */
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

    /* �䳢�� ���̰� �������� �� ���� */
    void InvitationRabbit()
    {
        Vector3 vDir = this.transform.position - target.transform.position;

        if (Mathf.Abs(vDir.x) < 7f && Mathf.Abs(vDir.z) < 7f)
        {
            rabbitFollowMe = true;
        }
        else rabbitFollowMe = false;
    }

    /* �浹 üũ �� ���� ����Ʈ �ϷḦ �����ϱ� ���� �޼ҵ� */
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
        /* �䳢�� ������ġ�� ��ֹ��� ���� ������ ��ġ���� Ȯ�� */
        if (col.gameObject.tag == "Obstacle" && !isSafePos)
        {
            //gameObject.transform.position = new Vector3(Random.Range(-10, 10), 1, Random.Range(-10, 10));
            gameObject.transform.position = new Vector3(Random.Range(-110, 105), 4, Random.Range(-68, 100));
        }
        else isSafePos = true;
    }
}
