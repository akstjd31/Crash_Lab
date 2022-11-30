using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchToMainScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    
    void GoMain()
    {
        SceneManager.LoadScene("Main");
        Player.isRiding = false; // 차를 타고 있는 상태에서 죽을 수도 있기 때문
        CarController.coolTimeStart = false; // 변수 제거를 위한 정적 변수 초기화
    }

    // 메인으로가는버튼 함수
    public void OnClickMainButton()
    {
        GoMain();
    }
}
