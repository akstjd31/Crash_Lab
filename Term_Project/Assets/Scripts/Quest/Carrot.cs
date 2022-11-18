using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrot : MonoBehaviour
{
    float rotSpeed = 30f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            QuestRabbit.getRadish = true;
            Destroy(gameObject);
        }
            
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            gameObject.transform.position = new Vector3(Random.Range(-10, 10), 1.5f, Random.Range(-10, 10));
        }
    }
}
