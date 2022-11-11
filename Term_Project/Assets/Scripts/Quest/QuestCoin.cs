using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestCoin : MonoBehaviour
{
    GameObject player;
    float rotSpeed = 20f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        RotCoin();
    }

    private void OnCollisionEnter(Collision col)
    {
        if (player == col.gameObject)
        {
            Destroy(this.gameObject);
            QuestManager.coinCnt++;
        }
    }

    void RotCoin()
    {
        transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime);
    }
}
