using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radish : MonoBehaviour
{
    float rotSpeed = 30f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime); // ???
    }

    // ?¡À???? ?????? ?????????? ?? ????
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            QuestRabbit.getRadish = true;
            Destroy(gameObject);
        }
            
    }

    // ?????? ????? ??? ???? ???
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            //gameObject.transform.position = new Vector3(Random.Range(-10, 10), 1.5f, Random.Range(-10, 10));
            this.gameObject.transform.position = new Vector3(Random.Range(-110, 105), 4, Random.Range(-68, 100));
        }
    }
}
