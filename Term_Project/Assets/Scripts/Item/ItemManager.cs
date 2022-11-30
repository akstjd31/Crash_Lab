using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum FoodID
{ 
    HAMBURGER = 0, PIZZA = 1, CAKE, APPLE, ICECREAM
};

enum BuffID
{
    SPEED_UP = 5, DECREASE_HP_TIME = 6, INCREASE_MAX_HP
};

public class ItemManager : MonoBehaviour
{
    public string itemName;                 // 먹은 아이템의 이름을 전달받기 위한 변수
    public bool UsingItem = false;          // 아이템사용여부
    public bool[] UsingItemArr;             // 아이템 중복 여부 판단을 위한 변수
    public float[] elapsedTimeArr;          // 아이템 효과 지속여부 확인을 위한 변수
    public int foodCnt = 0, buffCnt = 0;    // 아이템 갯수
    public GameObject[] items;              // 아이템 오브젝트
    public int numOfFood = 10;              // 소환될 아이템(음식)의 최대 갯수
    public int numOfItem = 5;               // 소환될 아이템(버프)의 최대 갯수

    int pastItemIndex;                      // 이전 아이템 인덱스
    private Buff[] buff;                    // 버프 추상 클래스
    private static ItemManager instance = null;

    // Start is called before the first frame update
    void Awake()
    {
        // 초기화
        UsingItemArr = new bool[items.Length];
        elapsedTimeArr = new float[items.Length];
        buff = new Buff[items.Length];

        for (int i = 0; i < items.Length; i++)
        {
            elapsedTimeArr[i] = 0.0f;
            buff[i] = null;
        }

        if (instance == null)
        {
            instance = this;

        }
    }

    public static ItemManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    /* Setter */
    public void SetItemName(string name)
    {
        this.itemName = name;
    }

    // Update is called once per frame
    void Update()
    {
        CheckItem();
        useItem();

        Spawn();
    }

    /* 아이템 스폰 */
    private void Spawn() 
    {
        if (foodCnt < numOfFood)
        {
            int randFoodNumber = Random.Range((int)FoodID.HAMBURGER, (int)FoodID.ICECREAM + 1);
            Instantiate(items[randFoodNumber], new Vector3(Random.Range(-110, 105), 4, Random.Range(-68, 100)), Quaternion.identity);

            foodCnt++;
        }
        if (buffCnt < numOfItem)
        {
            int randItemNumber = Random.Range((int)BuffID.SPEED_UP, (int)BuffID.INCREASE_MAX_HP + 1);
            Instantiate(items[randItemNumber], new Vector3(Random.Range(-110, 105), 4, Random.Range(-68, 100)), Quaternion.identity);

            buffCnt++;
        }
    }

    /* 전달받은 아이템 이름과 사용여부를 판단하고 객체 전달 */
    private void CheckItem()
    {
        if (UsingItem)
        {
            // 현재 먹은 아이템이 있으면 일단 저장
            if (Buff.itemIndex != 0) pastItemIndex = Buff.itemIndex;
            UsingItem = false;

            int i;
            for (i = 0; i < items.Length; i++)
            {
                // item 스크립트에서 전달받은 아이템명을 비교
                if (items[i].name == itemName) break;
            }

            if (i != items.Length) Buff.itemIndex = i;

            // 이전 아이템과 현재 아이템이 다르면서 이전 아이템 효과가 남아있지 않은경우
            if (!(pastItemIndex == Buff.itemIndex && UsingItemArr[pastItemIndex])) 
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

                    case (int)BuffID.SPEED_UP:
                        buff[i] = new SpeedUp();
                        break;

                    case (int)BuffID.DECREASE_HP_TIME:
                        buff[i] = new DecreaseHPSpeed();
                        break;

                    case (int)BuffID.INCREASE_MAX_HP:
                        buff[i] = new IncreaseMaxHP();
                        break;
                }
            }
        }

        // 아이템 사용 중 여부
        if (NotUsingItem()) UsingItem = false;
    }

    /* foodCnt 감소 */
    public void DecreaseFoodCount()
    {
        foodCnt--;
    }

    /* buffCnt 감소 */
    public void DecreaseBuffCount()
    {
        buffCnt--;
    }

    /* 아이템 사용유무를 실시간으로 판단 */
    private bool NotUsingItem()  
    {
        for (int j = 0; j < items.Length; j++) if (UsingItemArr[j]) return false;
        return true;
    }

    /* 아이템 사용 */
    private void useItem()
    {
        // 기존 아이템 효과가 남아있는지 확인
        for (int i = 0; i < items.Length; i++) 
        {
            if (UsingItemArr[i]) buff[i].method();
        }

        // 아이템 사용
        if (UsingItemArr[Buff.itemIndex]) 
        {
            buff[Buff.itemIndex].method();
        }
    }
}

/* 버프 추상 클래스 */
abstract class Buff
{
    public float elapsedTime;           // 지속시간
    public float currentTime;           // 현재(시작)시간
    public bool onTrigger;              // 효과적용을 위한 변수
    public int index;
    public static int itemIndex;        // 어떤 아이템인지 ID를 받아오기위한 변수
    public abstract void TriggerItem(); // 아이템 효과적용
    public abstract void RunTime();     // 지속시간 계산
    public abstract void EndRun();      // 마무리 작업

    /* 아이템 작업 수행 */
    public void method()
    {
        if (onTrigger) TriggerItem();
        else if (currentTime < elapsedTime) RunTime();
        else EndRun();
    }
}

/* 즉시 회복 효과(푸드) */
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
        ItemManager.Instance.UsingItemArr[index] = false;
        ItemManager.Instance.elapsedTimeArr[index] = 0.0f;
    }
}

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
        ItemManager.Instance.UsingItemArr[index] = false;
        ItemManager.Instance.elapsedTimeArr[index] = 0.0f;
    }
}

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
        ItemManager.Instance.UsingItem = false;
        ItemManager.Instance.UsingItemArr[index] = false;
        ItemManager.Instance.elapsedTimeArr[index] = 0.0f;
        Status.useItem = false;
    }
}

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
        ItemManager.Instance.UsingItem = false;
        ItemManager.Instance.UsingItemArr[index] = false;
        ItemManager.Instance.elapsedTimeArr[index] = 0.0f;
        Status.MAX_HP = 100;

        if (Status.HP > Status.MAX_HP) Status.HP = Status.MAX_HP;
    }
}
