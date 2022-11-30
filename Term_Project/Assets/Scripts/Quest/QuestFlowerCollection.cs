using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestFlowerCollection : MonoBehaviour
{
    float gatherTime = 0.0f;                    // 채집 시간(애니메이션에 맞춰서 없얘기 위함)  
    public static bool gatherArea = false;      // 채집 가능한 지역 판별
    public static bool getFlower = false;       // 채집 유무
    public static bool diggingFlower = false;   // 채집중(상태)인지 유무

    // Update is called once per frame
    void Update()
    {
        // 채집
        if (diggingFlower)
        {
            gatherTime += Time.deltaTime;
            if (gatherTime > 1.5f)
            {
                diggingFlower = false;
                gatherArea = false;
                getFlower = true;
                SoundManager.Instance.PlayOnRadishSound();
                Destroy(gameObject);
            }
        }
    }

    // 채집가능한 장소 여부 확인 및 소환 위치 결정
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            gatherArea = true;
        }

        if (other.gameObject.tag == "Obstacle")
        {
            //transform.position = new Vector3(Random.Range(-15, 15), 1, Random.Range(-15, 15));
            this.gameObject.transform.position = new Vector3(Random.Range(-110, 105), 2, Random.Range(-68, 100));
        }
    }
}
