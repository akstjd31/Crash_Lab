using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum FoodID
{ 
    HAMBURGER = 0, PIZZA = 1, CAKE, APPLE, ICECREAM
};

enum ItemID
{
    SPEED_UP = 5, DECREASE_HP_TIME = 6, INCREASE_MAX_HP
};

public class ItemManager : MonoBehaviour
{
    public static string itemName; // 복제된 아이템의 명칭
    public static bool UsingItem = false; // 아이템 사용 여부
    public static bool[] UsingItemArr; // 특정 아이템 사용 여부
    public static float[] elapsedTimeArr; // 아이템 지속 시간 
    public static int foodCnt = 0, itemCnt = 0;
    public GameObject[] items;
    public int numOfFood = 10; // 생성할 최대 음식 갯수
    public int numOfItem = 5; // 생성할 최대 버프아이템 갯수
    

    int pastItemIndex;
    Buff[] buff;
    // Start is called before the first frame update
    void Awake()
    {
        /* 초기화 */
        UsingItemArr = new bool[items.Length];
        elapsedTimeArr = new float[items.Length];
        buff = new Buff[items.Length];
        //buff = new Buff[items.Length];

        for (int i = 0; i < items.Length; i++)
        {
            UsingItemArr[i] = false;
            elapsedTimeArr[i] = 0.0f;
            buff[i] = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckItem();
        useItem();

        Spawn();
    }

    /// //////////////////////////////////////////////////////////////////
    /// forest
    /// X좌표 -80, 65
    /// Y좌표 5
    /// Z좌표 -25, 145
    //////////////////////////////////////////////////////////////////////
    private void Spawn() // 아이템 생성 메소드
    {
        if (foodCnt < numOfFood)
        {
            int randFoodNumber = Random.Range((int)FoodID.HAMBURGER, (int)FoodID.ICECREAM + 1);
            Instantiate(items[randFoodNumber], new Vector3(Random.Range(-15, 15), 1, Random.Range(-15, 15)), Quaternion.identity);

            foodCnt++;
        }
        if (itemCnt < numOfItem)
        {
            int randItemNumber = Random.Range((int)ItemID.SPEED_UP, (int)ItemID.INCREASE_MAX_HP + 1);
            Instantiate(items[randItemNumber], new Vector3(Random.Range(-15, 15), 1, Random.Range(-15, 15)), Quaternion.identity);

            itemCnt++;
        }
    }

    private void CheckItem() // 현재 먹은 아이템이 어떤 아이템인지 판별
    {
        if (UsingItem)
        {
            if (Buff.itemIndex != 0) pastItemIndex = Buff.itemIndex;
            UsingItem = false;

            int i;
            for (i = 0; i < items.Length; i++)
            {
                if (items[i].name == itemName) break;
            }

            Buff.itemIndex = i;
            if (!(pastItemIndex == Buff.itemIndex && UsingItemArr[pastItemIndex])) // 현재 먹은 아이템이 전에 먹었던 아이템하고 일치하면서 쿨타임이 아닐 때
            {
                UsingItemArr[i] = true;
                switch (i)
                {
                    case (int)FoodID.HAMBURGER:
                    case (int)FoodID.PIZZA:
                    case (int)FoodID.CAKE:
                    case (int)FoodID.APPLE:
                    case (int)FoodID.ICECREAM:
                        buff[i] = new Heal();
                        break;

                    case (int)ItemID.SPEED_UP:
                        buff[i] = new SpeedUp();
                        break;

                    case (int)ItemID.DECREASE_HP_TIME:
                        buff[i] = new DecreaseHPSpeed();
                        break;

                    case (int)ItemID.INCREASE_MAX_HP:
                        buff[i] = new IncreaseMaxHP();
                        break;
                }
            }
        }

        if (NotUsingItem()) UsingItem = false;
    }

    private bool NotUsingItem() // 플레이어에서 아이템 효과가 있는지 확인
    {
        for (int j = 0; j < items.Length; j++) if (UsingItemArr[j]) return false;
        return true;
    }

    private void useItem()
    {
        for (int i = 0; i < items.Length; i++) // 전에 먹었던 아이템 효과나 쿨타임이 남았는지 확인
        {
            if (UsingItemArr[i]) buff[i].method(); // if (UsingItemArr[i] && i != Buff.ItemIndex) buff[i].method();
        }

        if (UsingItemArr[Buff.itemIndex]) // 새로운 아이템을 먹었을 때
        {
            buff[Buff.itemIndex].method();
        }
    }
}

abstract class Buff // 공통적인 버프 추상 클래스
{
    public float coolTime; // 쿨 타임
    public float elapsedTime; // 지속 시간
    public bool onTrigger; // 아이템 적용효과를 처음 한 번만 실행하기 위한 변수
    public int index;
    public static int itemIndex; // 해당 아이템의 인덱스 
    public abstract void TriggerItem(); // 아이템 작동
    public abstract void RunTime(); // 런타임 계산

    public abstract void EndRun(); // 변경 값 초기화 작업

    public void method()
    {
        if (onTrigger) TriggerItem();
        else if (elapsedTime < coolTime) RunTime();
        else EndRun();
    }
}

class Heal : Buff
{
    public Heal()
    {
        coolTime = 1f;
        elapsedTime = 0f;
        onTrigger = true;
        index = itemIndex;
    }

    public override void TriggerItem()
    {
        onTrigger = false;
        /* 음식에 따른 회복력 비교 */
        if (index == (int)FoodID.HAMBURGER) Status.HP += 10;
        else if (index == (int)FoodID.PIZZA) Status.HP += 13;
        else if (index == (int)FoodID.CAKE) Status.HP += 16;
        else if (index == (int)FoodID.APPLE) Status.HP += 8;
        else if (index == (int)FoodID.ICECREAM) Status.HP += 5;

        if (Status.HP > Status.MAX_HP) Status.HP = Status.MAX_HP;
    }

    public override void RunTime()
    {
        elapsedTime += Time.deltaTime;
    }

    public override void EndRun()
    {
        ItemManager.UsingItemArr[index] = false;
        ItemManager.elapsedTimeArr[index] = 0.0f;
    }
}

/* 속도 아이템 */
class SpeedUp : Buff
{
    public SpeedUp()
    {
        coolTime = 3f;
        elapsedTime = 0f;
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
        elapsedTime += Time.deltaTime;
    }

    public override void EndRun()
    {
        Player.moveSpeed = Player.MAX_SPEED;
        ItemManager.UsingItemArr[index] = false;
        ItemManager.elapsedTimeArr[index] = 0.0f;
    }
}

/* 체력 감소속도를 늦추는 아이템 */
class DecreaseHPSpeed : Buff
{
    public DecreaseHPSpeed()
    {
        coolTime = 5f;
        elapsedTime = 0f;
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
        elapsedTime += Time.deltaTime;
    }
    public override void EndRun()
    {
        ItemManager.UsingItem = false;
        ItemManager.UsingItemArr[index] = false;
        ItemManager.elapsedTimeArr[index] = 0.0f;
        Status.useItem = false;
    }
}

/* 최대체력 제한을 100에서 150으로 늘려주는 아이템  */
class IncreaseMaxHP : Buff
{
    public IncreaseMaxHP()
    {
        coolTime = 10f;
        elapsedTime = 0f;
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
        elapsedTime += Time.deltaTime;
    }
    public override void EndRun()
    {
        ItemManager.UsingItem = false;
        ItemManager.UsingItemArr[index] = false;
        ItemManager.elapsedTimeArr[index] = 0.0f;
        Status.MAX_HP = 100;

        if (Status.HP > Status.MAX_HP) Status.HP = Status.MAX_HP;
    }
}
