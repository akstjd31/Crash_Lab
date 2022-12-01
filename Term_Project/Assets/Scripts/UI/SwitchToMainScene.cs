using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchToMainScene : MonoBehaviour
{
    void GoMain()
    {
        SceneManager.LoadScene("Main");
        StaticSettings();
    }

    void StaticSettings()
    {
        Player.isRiding = false; // 차에 탑승한 채로 죽고 다시 게임을 시작하면 엔진소리가 나는 것을 방지
        CarController.coolTimeStart = false; // 변수 제거를 위한 정적 변수 초기화
        QuestFlowerCollection.gatherArea = false;
        QuestFlowerCollection.getFlower = false;
        FindNPC.NPCGetRabbit = false;
        Status.HP = 100;
        Status.MAX_HP = 100;
        Status.useItem = false;
    }

    // 메인으로가는버튼 함수
    public void OnClickMainButton()
    {
        GoMain();
    }
}
