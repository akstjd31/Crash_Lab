using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel, leftSidePanel, rightSidePanel, miniPanel;  // 각 판넬들
    [SerializeField] private Text[] mainText;                                                 // 메인 판넬 텍스트
    [SerializeField] private Text leftSideText, playTimeText;                                 // 왼쪽 사이드, 오른쪽 사이드(플레이타임) 텍스트

    private static CanvasManager instance;

    private int minute;     // 분 계산을 위한 변수
    void Awake()
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
    public static CanvasManager Instance
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

    // Start is called before the first frame update
    void Start()
    {
        leftSideText.alignment = TextAnchor.MiddleCenter;
    }

    // Update is called once per frame
    void Update()
    {
        Destroying();
        PlayTimeText();
        MainText();
        LeftSideText();
        TimeSettings();
        TextSettings();
    }

    void Destroying()
    {
        if (SceneManager.GetSceneByName("Gameover").isLoaded || SceneManager.GetSceneByName("Gameclear").isLoaded)
        {
            Destroy(gameObject);
        }
    }

    /* 미니 판넬 액티브 비활성화 */
    public void MiniPanelOff()
    {
        miniPanel.SetActive(false);
    }

    /* 미니 판넬 액티브 활성화 */
    public void MiniPanelOn()
    {
        miniPanel.SetActive(true);
    }

    /* 왼쪽 판넬 액티브 비활성화 */
    public void LeftSidePanelOff()
    {
        leftSidePanel.SetActive(false);
    }

    /* 왼쪽 판넬 액티브 활성화 */
    public void LeftSidePanelOn()
    {
        leftSidePanel.SetActive(true);
    }

    /* MainPanel Getter */
    public GameObject GetMainPanel()
    {
        return mainPanel;
    }

    /* MiniPanel Getter */
    public GameObject GetMiniPanel()
    {
        return miniPanel;
    }

    /* leftSidePanel Getter */
    public GameObject GetLeftSidePanel()
    {
        return leftSidePanel;
    }

    /* 퀘스트 ID에 따른 텍스트 출력 */
    void MainText()
    {
        switch (QuestManager.Instance.GetQuestID())
        {
            case (int)QuestID.NPC:
                mainText[0].text = "NPC 찾기";
                break;
            case (int)QuestID.Coin:
                mainText[0].text = "NPC가 떨어트린 동전 줍기";
                mainText[1].text = "모은 동전 갯수 : " + QuestManager.Instance.coinCnt + " / 10";
                break;

            case (int)QuestID.Rabbit:
                mainText[0].text = "NPC가 토끼를 잡을 수 있도록 돕기";
                mainText[1].text = "떨어진 채소로 유인하자.";
                break;

            case (int)QuestID.Flower:
                mainText[0].text = "해바라기캐서 NPC 갖다주기";
                if (!QuestFlowerCollection.getFlower) mainText[1].text = "채집 필요";
                else mainText[1].text = "채집 완료";
                break;

            case (int)QuestID.Car:
                mainText[1].text = null;
                mainText[0].text = "차 찾아서 탑승하기";
                break;

            case (int)QuestID.Path:
                mainText[0].text = "차타고 정해진 경로 이동하기";
                mainText[1].text = "남은 경로 : " + PathManager.Instance.pathCnt;
                mainText[2].text = "다음 경로까지 남은 시간 : " + (int)QuestManager.Instance.pathTime + "초";
                break;
        }
    }

    /* 왼쪽 판넬 텍스트 출력 */
    void LeftSideText()
    {
        if (Player.isRiding)
            leftSideText.text = "F키 눌러서 하차하기";
        else if (!Player.isRiding)
            leftSideText.text = "F키 눌러서 승차하기";
    }

    /* 플레이 시간 텍스트 출력 및 분계산 */
    void PlayTimeText()
    {
        playTimeText.text = "플레이 시간\n" + minute + "분 " + (int)QuestManager.Instance.playTime + "초";
        if ((int)QuestManager.Instance.playTime / 60 >= 1)
        {
            minute++;
            QuestManager.Instance.playTime = 0.0f;
        }
    }

    /* 퀘스트 클리어했을 때 판넬 시간 초기화 */
    void TimeSettings()
    {
        if (QuestManager.Instance.questClear)
        {
            SoundManager.Instance.PlayOnMainPanelSound();
            GetMainPanel().GetComponent<MovePanel>().currentTime = 0.0f;
        }
    }

    /* 텍스트 세부 설정 */
    void TextSettings()
    {
        switch (QuestManager.Instance.GetQuestID())
        {
            case (int)QuestID.NPC:
                mainText[0].alignment = TextAnchor.MiddleCenter;
                break;

            case (int)QuestID.Coin:
                mainText[0].alignment = TextAnchor.UpperCenter;
                mainText[0].fontSize = 18;

                mainText[1].alignment = TextAnchor.LowerCenter;
                mainText[1].fontSize = 20;
                break;

            case (int)QuestID.Rabbit:
                mainText[0].fontSize = 14;
                mainText[0].alignment = TextAnchor.UpperCenter;
                break;

            case (int)QuestID.Flower:
                mainText[0].alignment = TextAnchor.UpperCenter;
                mainText[0].fontSize = 18;

                mainText[1].alignment = TextAnchor.MiddleCenter;
                mainText[1].fontSize = 20;
                break;

            case (int)QuestID.Car:
                mainText[0].fontSize = 20;
                break;

            case (int)QuestID.Path:
                mainText[0].fontSize = 16;
                mainText[0].alignment = TextAnchor.UpperCenter;

                mainText[1].fontSize = 16;
                mainText[1].alignment = TextAnchor.MiddleCenter;

                mainText[2].fontSize = 14;
                mainText[2].alignment = TextAnchor.LowerCenter;
                break;
        }
    }
}
