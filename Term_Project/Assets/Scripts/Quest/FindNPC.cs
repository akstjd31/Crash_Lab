using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindNPC : MonoBehaviour
{
    private GameObject target; // �÷��̾�
    private bool findNPC = false; // �� �״�� �ѹ��� ����ϱ� ����
    public static bool NPCGetRabbit = false;
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
            if (!findNPC)
            {
                QuestManager.questClear = true;
                findNPC = true; 
            }

            if (QuestManager.coinCnt == 10) QuestManager.questClear = true; // ���� ����Ʈ
            if (QuestManager.flowerCnt == 1) QuestManager.questClear = true; // �عٶ�� ����Ʈ
            if (NPCGetRabbit) QuestManager.questClear = true;
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
