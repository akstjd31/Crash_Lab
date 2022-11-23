using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    float rotateSpeed = 30f;
    void Start()
    {
    }

    void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }

    /* 플레이어가 아이템을 획득하면 bool값과 이름을 전달하고 다른 곳에서 다시 소환 */
    private void OnTriggerEnter(Collider other) 
    {
       if (other.gameObject.tag == "Player")
        {
            ItemManager.UsingItem = true;
            ItemManager.itemName = gameObject.name.Substring(0, gameObject.name.Length - 7); 

            if (gameObject.tag == "Food") ItemManager.foodCnt--;
            else ItemManager.itemCnt--;
            Destroy(gameObject);
        }
    }

    /* 아이템 소환 위치 결정 */
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            if (gameObject.tag == "Food") ItemManager.foodCnt--;
            else ItemManager.itemCnt--;
            Destroy(gameObject);
        }
    }
}