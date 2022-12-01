using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* HP가 줄어드는 속도 저하 */
class DecreaseHPSpeed : Buff
{
    public DecreaseHPSpeed()
    {
        elapsedTime = 5f;
        currentTime = 0f;
        onTrigger = true;
        index = itemIndex;
    }

    public override void TriggerItem()
    {
        onTrigger = false;
        Status.useItem = true;
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
        Status.useItem = false;
    }
}
