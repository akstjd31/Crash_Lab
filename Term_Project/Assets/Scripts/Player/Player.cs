using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static float moveSpeed = 10.0f;
    public float rotateSpeed = 10.0f;
    public const float MAX_SPEED = 10.0f;
    public Animator playerAnim;
    float h, v;
    public static float playTime = 0f;
    public static bool isRun = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        playTime += Time.deltaTime;

        isGather();
    }

    void FixedUpdate()
    {
        if (!QuestFlowerCollection.getFlower)
            Movement();
    }

    private void Movement()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        if (h != 0 || v != 0)
        {
            isRun = true;
            playerAnim.SetBool("isRun", true);
        }
        else
        {
            isRun = false;
            playerAnim.SetBool("isRun", false);
        }

        Vector3 dir = new Vector3(h, 0, v);

        if (!(h == 0 && v == 0))
        {
            transform.position += dir * moveSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotateSpeed);
        }
    }

    // 채집하기
    void isGather()
    {
        if (QuestManager.gatherArea && Input.GetMouseButtonDown(0) && !isRun)
        {
            playerAnim.SetTrigger("isGather");
            QuestFlowerCollection.getFlower = true;
        }
    }
}
