using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private float rotateSpeed = 30f;    // 회전 속도
    void Start()
    {
    }

    void Update()
    {
        // 회전
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }

    /* 플레이어가 아이템을 획득하면 bool값과 이름을 전달하고 다른 곳에서 다시 소환 */
    private void OnTriggerEnter(Collider other) 
    {
       if (other.gameObject.tag == "Player")
        {
            string itemName = gameObject.name.Substring(0, gameObject.name.Length - 7);
            ItemManager.Instance.usingItem = true;
            ItemManager.Instance.SetItemName(itemName);

            if (gameObject.tag == "Food")
            {
                SoundManager.Instance.PlayOnFoodSound();
                ItemManager.Instance.DecreaseFoodCount();
            }
            else
            {
                SoundManager.Instance.PlayOnBuffSound(itemName);
                ItemManager.Instance.DecreaseBuffCount();
            }
                Destroy(gameObject);
        }
    }

    /* 아이템 소환 위치 결정 */
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            if (gameObject.tag == "Food") ItemManager.Instance.DecreaseFoodCount();
            else ItemManager.Instance.DecreaseBuffCount();
            Destroy(gameObject);
        }
    }
}