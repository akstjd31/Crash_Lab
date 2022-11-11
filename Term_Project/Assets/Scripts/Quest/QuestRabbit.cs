using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestRabbit : MonoBehaviour
{
    float waitTime = 0.0f; // �̵��ð�, ���ð�
    float lookAtObsTime = 0.0f;
    //float moveSpeed = 6f;  // �����̴� �ӵ�
    float rotateSpeed = 10.0f; // ȸ���ӵ�
    bool waitForSecond = false; // ���������� �ƴ��� üũ
    public static bool isSafePos = false;
    Animator rabbitAnim;

    public static Vector3 randPos;
    Vector3 rabbitPos;
    // Start is called before the first frame update
    void Awake() // �ʱ�ȭ
    {
        rabbitAnim = gameObject.GetComponent<Animator>();
        Settings();
    }

    void Settings() // ���� �۾�
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
        /* �������� ������ �ð����� �۰ų� x, z ��ǥ�� ���̰� ��ǥ ������ǥ��
        ������ �����鼭 ���� ��ֹ��� �´��� �ð��� 1.5�� �̻��� �Ǹ� �䳢�� ���߰� ���ο� ��ǥ�� �����´�. */
        if (Mathf.Abs(transform.position.x - randPos.x) <= 1
        && Mathf.Abs(transform.position.z - randPos.z) <= 1
        || lookAtObsTime > 1.5f)
        {
            Wait();
            if(waitTime > 1.5f) Settings();
        }
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
        Vector3 vDist = randPos - rabbitPos;
        Vector3 vDir = vDist.normalized;

        rabbitAnim.SetBool("isRun", true);

        if (!(Mathf.Abs(transform.position.x - randPos.x) <= 1 && Mathf.Abs(transform.position.z - randPos.z) <= 1))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(vDir), Time.deltaTime * rotateSpeed);
        }  
    }

    /* ���� �䳢�� ��ֹ��� �´�� �ð��� ���� */
    private void CheckGoalPos()
    {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(rabbitPos, fwd, out hit, 2) != false
                            && hit.collider.gameObject.tag == "Obstacle") {
                lookAtObsTime += Time.deltaTime;
        }
    }

    /* �浹 üũ �� ���� ����Ʈ �ϷḦ �����ϱ� ���� �޼ҵ� */
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            QuestManager.questClear = true;
            Destroy(gameObject);
        } 

        /* �䳢�� ������ġ�� ��ֹ��� ���� ������ ��ġ���� Ȯ�� */
        if (col.gameObject.tag == "Obstacle" && !isSafePos)
        {
            this.gameObject.transform.position = new Vector3(Random.Range(-15, 15), 1, Random.Range(-15, 15));
        } else isSafePos = true;
    }
}
