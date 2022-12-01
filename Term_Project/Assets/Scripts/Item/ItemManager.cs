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
    public bool usingItem = false;          // 아이템사용여부
    public bool[] usingItemArr;             // 아이템 중복 여부 판단을 위한 변수
    public float[] elapsedTimeArr;          // 아이템 효과 지속여부 확인을 위한 변수
    public int foodCnt = 0, buffCnt = 0;    // 아이템 갯수
    public GameObject[] items;              // 아이템 오브젝트
    public int numOfFood;                   // 소환될 아이템(음식)의 최대 갯수
    public int numOfItem;                   // 소환될 아이템(버프)의 최대 갯수

    int pastItemIndex;                      // 이전 아이템 인덱스
    private Buff[] buff;                    // 버프 추상 클래스
    private static ItemManager instance = null;

    // Start is called before the first frame update
    void Awake()
    {
        // 초기화
        usingItemArr = new bool[items.Length];
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
        if (usingItem)
        {
            // 현재 먹은 아이템이 있으면 일단 저장
            if (Buff.itemIndex != 0) pastItemIndex = Buff.itemIndex;
            usingItem = false;

            int i;
            for (i = 0; i < items.Length; i++)
            {
                // item 스크립트에서 전달받은 아이템명을 비교
                if (items[i].name == itemName) break;
            }

            if (i != items.Length) Buff.itemIndex = i;

            // 이전 아이템과 현재 아이템이 다르면서 이전 아이템 효과가 남아있지 않은경우
            if (!(pastItemIndex == Buff.itemIndex && usingItemArr[pastItemIndex])) 
            {
                usingItemArr[i] = true;
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
        if (NotUsingItem()) usingItem = false;
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
        for (int j = 0; j < items.Length; j++) if (usingItemArr[j]) return false;
        return true;
    }

    /* 아이템 사용 */
    private void useItem()
    {
        // 기존 아이템 효과가 남아있는지 확인
        for (int i = 0; i < items.Length; i++) 
        {
            if (usingItemArr[i]) buff[i].method();
        }

        // 아이템 사용
        if (usingItemArr[Buff.itemIndex]) 
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


