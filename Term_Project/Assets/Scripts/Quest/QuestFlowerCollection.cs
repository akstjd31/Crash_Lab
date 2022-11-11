using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestFlowerCollection : MonoBehaviour
{
    GameObject player;
    float gatherTime = 0.0f;
    public static bool getFlower = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (getFlower)
        {
            gatherTime += Time.deltaTime;
            if (gatherTime > 2.5f)
            {
                getFlower = false;
                QuestManager.flowerCnt++;
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player)
        {
            QuestManager.gatherArea = true;
        }
    }
}
