using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radish : MonoBehaviour
{
    float rotSpeed = 30f;   // 회전 속도

    // Update is called once per frame
    void Update()
    {
        // 회전
        transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime); 
    }

    // 플레이어의 무 획득
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            QuestRabbit.getRadish = true;
            SoundManager.Instance.PlayOnRadishSound();
            Destroy(gameObject);
        }
            
    }

    // 무 소환 위치 결정
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            //gameObject.transform.position = new Vector3(Random.Range(-10, 10), 1.5f, Random.Range(-10, 10));
            this.gameObject.transform.position = new Vector3(Random.Range(-110, 105), 4, Random.Range(-68, 100));
        }
    }
}
