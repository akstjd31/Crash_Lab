using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 최대 HP 증가  */
class IncreaseMaxHP : Buff
{
    public IncreaseMaxHP()
    {
        elapsedTime = 10f;
        currentTime = 0f;
        onTrigger = true;
        index = itemIndex;
    }

    public override void TriggerItem()
    {
        onTrigger = false;
        Status.MAX_HP = 150;
    }
    public override void RunTime()
    {
        currentTime += Time.deltaTime;
    }
    public override void EndRun()
    {
        ItemManager.Instance.usingItem = false;
        ItemManager.Instance.usingItemArr[index] = false;
        ItemManager.Instance.elapsedTimeArr[index] = 0.0f;
        Status.MAX_HP = 100;

        if (Status.HP > Status.MAX_HP) Status.HP = Status.MAX_HP;
    }
}
