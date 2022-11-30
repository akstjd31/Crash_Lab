using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPath : MonoBehaviour
{
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Car");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /* 충돌한 물체를 확인하고 경로의 개수, 시간, 다음 경로 액티브 활성화 등 작업 */
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Car" && Player.isRiding)
        {
            SoundManager.Instance.PlayOnPathSound();
            PathManager.Instance.pathCnt--;
            PathManager.Instance.CalPathTime();
            PathManager.Instance.NextPath();
            if (PathManager.Instance.pathCnt == 0) QuestManager.Instance.questClear = true;
            Destroy(gameObject);
        }
    }
}
