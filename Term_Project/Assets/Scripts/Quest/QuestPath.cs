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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player && Player.isRiding)
        {
            PathManager.Instance.pathCnt--;
            PathManager.Instance.CalPathTime();
            PathManager.Instance.NextPath();
            if (PathManager.Instance.pathCnt == 0) QuestManager.Instance.questClear = true;
            Destroy(gameObject);
        }
    }
}
