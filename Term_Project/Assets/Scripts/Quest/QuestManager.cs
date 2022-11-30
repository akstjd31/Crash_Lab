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
    private int questId;                             // 퀘스트 ID
    public int coinCnt = 0;                          // 동전 갯수
    [SerializeField] private GameObject[] questItem; // 퀘스트에 사용되는 오브젝트
    private bool questItemRecall = false;            // 아이템 소환
    public bool questClear = false;                  // 퀘스트 클리어
    public float pathTime = 15.0f;                   // 다음 경로까지 남은 시간
    public float playTime = 0.0f;                    // 플레이 시간

    [SerializeField] private GameObject radish;

    private static QuestManager instance = null;

    void Awake() // 싱글톤
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

    /* Getter */
    public int GetQuestID()
    {
        return questId;
    }

    /* 다음 퀘스트로 넘어가기 */
    void NextQuest()
    {
        if (questId == 0) questId += 4;
        else questId++;
        if (questId == 4) SceneManager.LoadScene("City");
        else if (questId == 6) SceneManager.LoadScene("Gameclear");
    }

    /* 현재 퀘스트의 클리어 유무 확인 및 다음 퀘스트 진행 */
    void CheckQuest() // 
    {
        if (QuestClear(questId))
        {
            NextQuest();

            questClear = false;
        }
    }

    /* id에 해당하는 퀘스트 완료 bool값 반환 + 이전 퀘스트 초기화(퀘스트 클리어가 true가 되는 것을 방지) */
    bool QuestClear(int id) // 
    {
        if (questClear)
        {
            questItemRecall = false;
            switch (id)
            {
                case (int)QuestID.Coin:
                    coinCnt = 0;
                    break;
                case (int)QuestID.Rabbit:
                    FindNPC.NPCGetRabbit = false;
                    break;
                case (int)QuestID.Flower:
                    QuestFlowerCollection.getFlower = false;
                    break;
            }
            return true;
        }
        return false;
    }

    /* 퀘스트 시작 */
    void StartQuest()
    {
        ControlObject();
    }

    /* 퀘스트 오브젝트의 스폰 or 시간 관리 */
    void ControlObject()
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

    /* 각 오브젝트 스폰 */
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
        if (pathTime <= 0.0f) SceneManager.LoadScene("Gameover"); // 통과시간이 음수가 되면 게임 오버
        Destroying(); // 파괴

        playTime += Time.deltaTime;
        StartQuest(); // 퀘스트 시작
        CheckQuest(); // 클리어 유무 확인 및 초기화
    }

    void Destroying()
    {
        if (SceneManager.GetSceneByName("Gameover").isLoaded || SceneManager.GetSceneByName("Gameclaer").isLoaded)
        {
            Destroy(gameObject);
        }
    }
}
