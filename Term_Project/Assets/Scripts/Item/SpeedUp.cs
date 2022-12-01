using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 이동 속도 증가 */
class SpeedUp : Buff
{
    public SpeedUp()
    {
        elapsedTime = 3f;
        currentTime = 0f;
        onTrigger = true;
        index = itemIndex;
    }

    public override void TriggerItem()
    {
        onTrigger = false;
        Player.moveSpeed = 15f;
    }

    public override void RunTime()
    {
        currentTime += Time.deltaTime;
    }

    public override void EndRun()
    {
        Player.moveSpeed = Player.MAX_SPEED;
        ItemManager.Instance.usingItem = false;
        ItemManager.Instance.usingItemArr[index] = false;
        ItemManager.Instance.elapsedTimeArr[index] = 0.0f;
    }
}