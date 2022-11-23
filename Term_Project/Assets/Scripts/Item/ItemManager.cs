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
    public static string itemName; // ������ �������� ��Ī
    public static bool UsingItem = false; // ������ ��� ����
    public static bool[] UsingItemArr; // Ư�� ������ ��� ����
    public static float[] elapsedTimeArr; // ������ ���� �ð� 
    public static int foodCnt = 0, itemCnt = 0;
    public GameObject[] items;
    private int numOfFood = 10; // ������ �ִ� ���� ����
    private int numOfItem = 5; // ������ �ִ� ���������� ����

    int pastItemIndex;
    private Buff[] buff;
    // Start is called before the first frame update
    void Awake()
    {
        /* �ʱ�ȭ */
        UsingItemArr = new bool[items.Length];
        elapsedTimeArr = new float[items.Length];
        buff = new Buff[items.Length];
        //buff = new Buff[items.Length];

        for (int i = 0; i < items.Length; i++)
        {
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
    /// X��ǥ -80, 65
    /// Y��ǥ 5
    /// Z��ǥ -25, 145
    //////////////////////////////////////////////////////////////////////
    private void Spawn() // ������ ���� �޼ҵ�
    {
        if (foodCnt < numOfFood)
        {
            int randFoodNumber = Random.Range((int)FoodID.HAMBURGER, (int)FoodID.ICECREAM + 1);
            Instantiate(items[randFoodNumber], new Vector3(Random.Range(-110, 105), 4, Random.Range(-68, 100)), Quaternion.identity);

            foodCnt++;
        }
        if (itemCnt < numOfItem)
        {
            int randItemNumber = Random.Range((int)ItemID.SPEED_UP, (int)ItemID.INCREASE_MAX_HP + 1);
            Instantiate(items[randItemNumber], new Vector3(Random.Range(-110, 105), 4, Random.Range(-68, 100)), Quaternion.identity);

            itemCnt++;
        }
    }

    private void CheckItem() // ���� ���� �������� � ���������� �Ǻ�
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
            if (!(pastItemIndex == Buff.itemIndex && UsingItemArr[pastItemIndex])) // ���� ���� �������� ���� �Ծ��� �������ϰ� ��ġ�ϸ鼭 ��Ÿ���� �ƴ� ��
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

    private bool NotUsingItem() // �÷��̾�� ������ ȿ���� �ִ��� Ȯ��
    {
        for (int j = 0; j < items.Length; j++) if (UsingItemArr[j]) return false;
        return true;
    }

    private void useItem()
    {
        for (int i = 0; i < items.Length; i++) // ���� �Ծ��� ������ ȿ���� ��Ÿ���� ���Ҵ��� Ȯ��
        {
            if (UsingItemArr[i]) buff[i].method(); // if (UsingItemArr[i] && i != Buff.ItemIndex) buff[i].method();
        }

        if (UsingItemArr[Buff.itemIndex]) // ���ο� �������� �Ծ��� ��
        {
            buff[Buff.itemIndex].method();
        }
    }
}

abstract class Buff // �������� ���� �߻� Ŭ����
{
    public float coolTime; // �� Ÿ��
    public float elapsedTime; // ���� �ð�
    public bool onTrigger; // ������ ����ȿ���� ó�� �� ���� �����ϱ� ���� ����
    public int index;
    public static int itemIndex; // �ش� �������� �ε��� 
    public abstract void TriggerItem(); // ������ �۵�
    public abstract void RunTime(); // ��Ÿ�� ���

    public abstract void EndRun(); // ���� �� �ʱ�ȭ �۾�

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
        /* ���Ŀ� ���� ȸ���� �� */
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

/* �ӵ� ������ */
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

/* ü�� ���Ҽӵ��� ���ߴ� ������ */
class DecreaseHPSpeed : Buff
{
    public DecreaseHPSpeed()
    {
        coolTime = 5f;
        elapsedTime = 0f;
        onTrigger = true;
        index = itemIndex;
    }

    // ������ ȿ��
    public override void TriggerItem()
    {
        onTrigger = false;
        Status.useItem = true;
    }
    // ���� �ð� ���
    public override void RunTime()
    {
        elapsedTime += Time.deltaTime;
    }

    // ���� �۾�
    public override void EndRun()
    {
        ItemManager.UsingItem = false;
        ItemManager.UsingItemArr[index] = false;
        ItemManager.elapsedTimeArr[index] = 0.0f;
        Status.useItem = false;
    }
}

/* �ִ�ü�� ������ 100���� 150���� �÷��ִ� ������  */
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
