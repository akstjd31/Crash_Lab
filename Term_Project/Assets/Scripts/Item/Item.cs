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

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            if (gameObject.tag == "Food") ItemManager.foodCnt--;
            else ItemManager.itemCnt--;
            Destroy(gameObject);

            Debug.Log(transform.position);
        }
    }
}