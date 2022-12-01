using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestFlowerCollection : MonoBehaviour
{
    float gatherTime = 0.0f;                    // 채집 시간(애니메이션에 맞춰서 없얘기 위함)  
    public static bool gatherArea = false;      // 채집 가능한 지역 판별
    public static bool getFlower = false;       // 채집 유무
    public static bool diggingFlower = false;   // 채집중(상태)인지 유무
    bool flag = false;

    GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    // Update is called once per frame
    void Update()
    {
        CheckDir();
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

    /* 꽃과 플레이어 거리 계산해서 해당 지역에서 채집이 가능한지 여부 판단 */
    void CheckDir()
    {
        float absXDir = Mathf.Abs(this.gameObject.transform.position.x - player.transform.position.x);
        float absZDir = Mathf.Abs(this.gameObject.transform.position.z - player.transform.position.z);

        if (absXDir < 3 && absZDir < 3)
        {
            gatherArea = true;
            if (!flag)
            {
                CanvasManager.Instance.MiniPanelOn();
                SoundManager.Instance.PlayOnSidePanelSound();
                flag = true;
            }
        }
        else
        {
            gatherArea = false;
            CanvasManager.Instance.MiniPanelOff();
            CanvasManager.Instance.GetMiniPanel().transform.position = CanvasManager.Instance.GetMiniPanel().GetComponent<MovePanel>().startPosition.position;
            CanvasManager.Instance.GetMiniPanel().GetComponent<MovePanel>().currentTime = 0.0f;
            flag = false;
        }
    }

    // 채집가능한 장소 여부 확인 및 소환 위치 결정
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            //transform.position = new Vector3(Random.Range(-15, 15), 1, Random.Range(-15, 15));
            this.gameObject.transform.position = new Vector3(Random.Range(-110, 105), 2, Random.Range(-68, 100));
        }
    }
}
