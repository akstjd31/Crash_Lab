using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestRabbit : MonoBehaviour
{
    float runTime, waitTime; // �̵��ð�, ���ð�
    int randRunTime; // ���� �̵��ð�
    float moveSpeed = 6f;  // �����̴� �ӵ�
    float rotateSpeed = 10.0f; // ȸ���ӵ�
    bool waitForSecond = false; // ���������� �ƴ��� üũ
    Animator rabbitAnim;
    GameObject player;

    Vector3 randPos, rabbitPos;
    // Start is called before the first frame update
    void Awake() // �ʱ�ȭ
    {
        rabbitAnim = gameObject.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        Settings();
    }

    void Settings() // ���� �۾�
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

        /* �������� ������ �ð����� �۰ų� x, z ��ǥ�� ���̰� ��ǥ ������ǥ�� 1���� ���� �䳢�� �����. */
        if ((runTime > randRunTime || Mathf.Abs(transform.position.x - randPos.x) <= 1 && Mathf.Abs(transform.position.z - randPos.z) <= 1))
        {
            Wait();
            if(waitTime > 1) Settings();
        } 
    }
    /* �䳢�� ���缱��. */
    void Wait()
    {
        waitTime += Time.deltaTime;
        waitForSecond = true;
        rabbitAnim.SetBool("isRun", false);
        Debug.Log(waitTime);
    }
    /* �䳢�� �̵� �� ȸ�� */
    void Movement()
    {
        rabbitPos = transform.position;
        Vector3 vDist = randPos - rabbitPos;
        Vector3 vDir = vDist.normalized;

        rabbitAnim.SetBool("isRun", true);

        transform.position += vDir * moveSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(vDir), Time.deltaTime * rotateSpeed);
    }

    /* �浹 üũ �� ���� ����Ʈ �ϷḦ �����ϱ� ���� �޼ҵ� */
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject == player)
        {
            QuestManager.questClear = true;
            Destroy(gameObject);
        }
    }
}
