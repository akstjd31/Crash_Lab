using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindNPC : MonoBehaviour
{
    GameObject target; // �÷��̾�
    bool onlyOneUse = false; // �� �״�� �ѹ��� ����ϱ� ����
    public static bool findNPC = false;
    public static bool isGetRabbit = false;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // �÷��̾�� ��ȣ�ۿ��� ������ ��ġ����?
        if (Mathf.Abs(transform.position.x - target.transform.position.x) < 4f
            && Mathf.Abs(transform.position.z - target.transform.position.z) < 4f)
        {
            // NPC �߰� ����Ʈ
            if (!onlyOneUse)
            {
                findNPC = true;
                onlyOneUse = true; 
            }
            else if (QuestManager.coinCnt == 10) findNPC = true; // ���� ����Ʈ
            else if (QuestManager.flowerCnt == 1) findNPC = true; // �عٶ�� ����Ʈ
            else if (isGetRabbit) findNPC = true;
            this.transform.LookAt(target.transform.position); // �÷��̾�� ����� ��ġ���� �׻� �÷��̾ �ٶ󺻴�.
        }
    }

    // ��ֹ��� ��ġ�� �ʴ� ��ҿ��� ��ȯ
    private void OnCollisionStay(Collision col)
    {
        if (col.gameObject.tag == "Obstacle")
        {
            this.transform.position = new Vector3(Random.Range(-10, 10), transform.position.y, Random.Range(-10, 10));
            //this.gameObject.transform.position = new Vector3(Random.Range(-110, 105), 4, Random.Range(-68, 100));
        }
    }
}
