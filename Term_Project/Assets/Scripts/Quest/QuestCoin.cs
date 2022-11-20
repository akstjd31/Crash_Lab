using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestCoin : MonoBehaviour
{
    float rotSpeed = 20f;
    // Update is called once per frame
    void Update()
    {
        RotCoin();
    }

    // 플레이어의 코인 획득
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            QuestManager.coinCnt++;
            Destroy(this.gameObject);
        }
    }

    // 장애물과 겹치지 않는 장소에 소환
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            this.gameObject.transform.position = new Vector3(Random.Range(-15, 15), 2, Random.Range(-15, 15));
            //this.gameObject.transform.position = new Vector3(Random.Range(-110, 105), 4, Random.Range(-68, 100));
        }
    }
    void RotCoin()
    {
        transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime);
    }
}
