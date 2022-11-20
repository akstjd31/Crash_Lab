using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radish : MonoBehaviour
{
    float rotSpeed = 30f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime); // 회전
    }

    // 플레이어가 획득하면 스폰되어있던 무 삭제
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            QuestRabbit.getRadish = true;
            Destroy(gameObject);
        }
            
    }

    // 장애물과 겹치지 않는 장소에 소환
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            gameObject.transform.position = new Vector3(Random.Range(-10, 10), 1.5f, Random.Range(-10, 10));
            //this.gameObject.transform.position = new Vector3(Random.Range(-110, 105), 4, Random.Range(-68, 100));
        }
    }
}
