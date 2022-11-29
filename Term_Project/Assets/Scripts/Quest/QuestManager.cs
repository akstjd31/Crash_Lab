using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum QuestID
{
    NPC = 0, Coin, Rabbit, Flower, Car, Path
}

public class QuestManager : MonoBehaviour
{
    private int questId;
    public int coinCnt = 0;
    [SerializeField] private GameObject[] questItem;
    private bool questItemRecall = false;
    public bool questClear = false;
    public float pathTime = 15.0f; // 다음 경로까지 남은 시간
    public float playTime = 0.0f;

    [SerializeField] private GameObject radish;

    private static QuestManager instance = null;

    void Awake() // 초기화
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        questId = 0;
    }

    public static QuestManager Instance
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

    public int GetQuestID() // 현재 퀘스트의 ID를 반환
    {
        return questId;
    }

    void NextQuest() // 다음 퀘스트로 넘어가기
    {
        if (questId == 0) questId += 4;
        else questId++;

        if (questId == 4) SceneManager.LoadScene("City");
        else if (questId == 6) SceneManager.LoadScene("Gameclear");
    }

    void CheckQuest() // 현재 퀘스트가 완료되었다면 다음 퀘스트로 넘어감.
    {
        if (QuestClear(questId))
        {
            NextQuest();

            questClear = false;
        }
    }

    bool QuestClear(int id) // id에 해당하는 퀘스트가 완료했다면 true
    {
        if (questClear)
        {
            questItemRecall = false;
            switch (id)
            {
                case (int)QuestID.NPC: break;
                case (int)QuestID.Coin:
                    coinCnt = 0;
                    break;
                case (int)QuestID.Rabbit:
                    FindNPC.NPCGetRabbit = false;
                    break;
                case (int)QuestID.Flower:
                    QuestFlowerCollection.getFlower = false;
                    break;
                case (int)QuestID.Car: break;
                case (int)QuestID.Path:break;
            }
            return true;
        }
        return false;
    }

    void StartQuest() // 임시로 게임을 시작한지 5초가 지나면 퀘스트 부여
    {
        ControlObject();
    }

    void ControlObject() // 퀘스트와 관련된 오브젝트들 수행
    {
        if (!questItemRecall)
        {
            if(GetQuestID() == (int)QuestID.Coin)
            {
                int maxCoin = 10;
                for (int i = 0; i < maxCoin; i++) Spawn();
            } else Spawn();
            questItemRecall = true;
        }

        switch (questId)
        {
            case (int)QuestID.Path:
                pathTime -= Time.deltaTime;
                break;
        }
    }

    void Spawn()
    {
        if (GetQuestID() < (int)QuestID.Car) //Instantiate(questItem[questId], new Vector3(Random.Range(-10, 10), questItem[questId].transform.position.y, Random.Range(-10, 10)), Quaternion.identity);
        {
            if (GetQuestID() == (int)QuestID.Flower) Instantiate(questItem[questId], new Vector3(Random.Range(-110, 105), 2f, Random.Range(-68, 100)), Quaternion.identity);
            else Instantiate(questItem[questId], new Vector3(Random.Range(-110, 105), 4f, Random.Range(-68, 100)), Quaternion.identity);
        }
        
    }

    void Update()
    {
        playTime += Time.deltaTime;
        StartQuest();
        CheckQuest();

        if (SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().buildIndex == 3) // 게임오버 : 2
        {
            Destroy(gameObject);
        }
    }
}
