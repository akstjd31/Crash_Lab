using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindNPC : MonoBehaviour
{
    GameObject target; // 플레이어
    bool onlyOneUse = false; // 말 그대로 한번만 사용하기 위함
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
        // 플레이어와 상호작용이 가능한 위치인지?
        if (Mathf.Abs(transform.position.x - target.transform.position.x) < 4f
            && Mathf.Abs(transform.position.z - target.transform.position.z) < 4f)
        {
            // NPC 발견 퀘스트
            if (!onlyOneUse)
            {
                findNPC = true;
                onlyOneUse = true; 
            }
            else if (QuestManager.coinCnt == 10) findNPC = true; // 코인 퀘스트
            else if (QuestManager.flowerCnt == 1) findNPC = true; // 해바라기 퀘스트
            else if (isGetRabbit) findNPC = true;
            this.transform.LookAt(target.transform.position); // 플레이어와 가까운 위치에서 항상 플레이어를 바라본다.
        }
    }

    // 장애물과 겹치지 않는 장소에서 소환
    private void OnCollisionStay(Collision col)
    {
        if (col.gameObject.tag == "Obstacle")
        {
            this.transform.position = new Vector3(Random.Range(-10, 10), transform.position.y, Random.Range(-10, 10));
            //this.gameObject.transform.position = new Vector3(Random.Range(-110, 105), 4, Random.Range(-68, 100));
        }
    }
}
