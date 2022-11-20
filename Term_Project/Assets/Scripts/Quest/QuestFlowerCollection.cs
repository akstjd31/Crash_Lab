using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestFlowerCollection : MonoBehaviour
{
    GameObject player;
    float gatherTime = 0.0f;
    public static bool getFlower = false; // 채집 중인지 확인
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // 채집
        if (getFlower)
        {
            gatherTime += Time.deltaTime;
            if (gatherTime > 1.5f)
            {
                getFlower = false;
                QuestManager.flowerCnt++;
                Destroy(gameObject);
            }
        }
    }

    // 채집가능한 장소 여부 확인 및
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player)
        {
            QuestManager.gatherArea = true;
        }

        if (other.gameObject.tag == "Obstacle")
        {
            //transform.position = new Vector3(Random.Range(-15, 15), 1, Random.Range(-15, 15));
            this.gameObject.transform.position = new Vector3(Random.Range(-110, 105), 4, Random.Range(-68, 100));
        }
    }
}
