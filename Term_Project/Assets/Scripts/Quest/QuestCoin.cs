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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
            QuestManager.coinCnt++;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            this.gameObject.transform.position = new Vector3(Random.Range(-15, 15), 2, Random.Range(-15, 15));
        }
    }
    void RotCoin()
    {
        transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime);
    }
}
