using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    float rotateSpeed = 30f;

    GameObject player, obstacle;
    ItemManager iManager;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        obstacle = GameObject.FindGameObjectWithTag("Obstacle");

    }

    void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision col)
    {
       if (col.gameObject == player) // 플레이어랑 충돌하면 해당 오브젝트 삭제 후 아이템 즉시 사용
        {
            ItemManager.UsingItem = true;
            ItemManager.itemName = gameObject.name.Substring(0, gameObject.name.Length - 7); // 해당 오브젝트 네임 뒤 (clone) 빼고 전달

            if (gameObject.tag == "Food") ItemManager.foodCnt--;
            else ItemManager.itemCnt--;
            Destroy(gameObject);
        }
        

    }

    private void OnCollisionStay(Collision col)
    {
        if (col.gameObject == obstacle)
        {
            this.transform.position = new Vector3(Random.Range(-15, 15), 1, Random.Range(-15, 15));
        }
    }
}