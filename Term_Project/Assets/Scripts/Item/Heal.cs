using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 즉시 회복 */
class Heal : Buff
{
    public Heal()
    {
        elapsedTime = 1f;
        currentTime = 0f;
        onTrigger = true;
        index = itemIndex;
    }

    public override void TriggerItem()
    {
        onTrigger = false;

        // 각 인덱스 비교
        if (index == (int)FoodID.HAMBURGER) Status.HP += 10;
        else if (index == (int)FoodID.PIZZA) Status.HP += 13;
        else if (index == (int)FoodID.CAKE) Status.HP += 16;
        else if (index == (int)FoodID.APPLE) Status.HP += 8;
        else if (index == (int)FoodID.ICECREAM) Status.HP += 5;

        if (Status.HP > Status.MAX_HP) Status.HP = Status.MAX_HP;
    }

    public override void RunTime()
    {
        currentTime += Time.deltaTime;
    }

    public override void EndRun()
    {
        ItemManager.Instance.usingItemArr[index] = false;
        ItemManager.Instance.elapsedTimeArr[index] = 0.0f;
    }
}
