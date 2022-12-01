using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip forestSound;
    [SerializeField] private AudioClip[] forestWalkSound;
    
    [SerializeField] private AudioClip foodSound;
    [SerializeField] private AudioClip[] buffSound;
    // 0 : SpeedUp
    // 1 : DecreaseHPSpeedm
    // 2 : IncreaseMaxHP

    [SerializeField] private AudioClip[] coinSound;
    [SerializeField] private AudioClip[] rabbitWalkSound;
    [SerializeField] private AudioClip radishSound;
    [SerializeField] private AudioClip flowerSound;
    [SerializeField] private AudioClip[] panelSound;
    // 0 : Main
    // 1 : Side

    [SerializeField] private AudioClip[] cityWalkSound;
    [SerializeField] private AudioClip citySound;
    [SerializeField] private AudioClip[] carSound;
    // 0 : CarEngine
    // 1 : CarClosedDoor

    [SerializeField] private AudioClip pathSound;

    private static SoundManager instance;
    private AudioSource audiosource;

    private float walkTime = 0.0f, rabbitWalkTime = 0.0f, digTime = 0.0f; // 해당 애니메이션 간격과 맞춰주기 위한 일정 시간 지연
    bool flag = false;
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

    public static SoundManager Instance
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
        audiosource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Destroying();

        // 기본은 숲 배경음악으로 설정되어있으나, City 씬으로 전환되었을 때 한번만 실행
        if (SceneManager.GetSceneByName("City").isLoaded && !flag) 
        {
            audiosource.clip = citySound;
            audiosource.loop = true;
            audiosource.volume = 0.8f;
            audiosource.Play();
            flag = true;
        }
    }

    void Destroying()
    {
        if (SceneManager.GetSceneByName("Gameover").isLoaded || SceneManager.GetSceneByName("Gameclear").isLoaded)
        {
            Destroy(this.gameObject);
        }
    }

    /* 맵에 따라 다른 플레이어의 이동 사운드 */
    public void PlayOnWalkSound()
    {
        int randNum = Random.Range(0, 3);

        walkTime += Time.deltaTime;

        if (walkTime > 0.3f)
        {
            if (SceneManager.GetSceneByName("Forest").isLoaded) audiosource.PlayOneShot(forestWalkSound[randNum]);
            else if (SceneManager.GetSceneByName("City").isLoaded) audiosource.PlayOneShot(cityWalkSound[randNum]);
            walkTime = 0.0f;
        }
    }

    /* 코인 먹는 사운드 */
    public void PlayOnCoinSound()
    {
        int randNum = Random.Range(0, 3);
        audiosource.PlayOneShot(coinSound[randNum]);
    }

    /* 아이템(음식)먹는 사운드 */
    public void PlayOnFoodSound()
    {
        audiosource.PlayOneShot(foodSound);
    }

    /* 아이템(버프)먹는 사운드 */
    public void PlayOnBuffSound(string name)
    {
        if (name == "SpeedUp") audiosource.PlayOneShot(buffSound[0]);
        else if (name == "DecreaseHPSpeed") audiosource.PlayOneShot(buffSound[1]);
        else if (name == "IncreaseMaxHP") audiosource.PlayOneShot(buffSound[2]);
    }

    /* 메인판넬 이동 사운드 */
    public void PlayOnMainPanelSound()
    {
        audiosource.PlayOneShot(panelSound[0]);
    }

    /* 사이드판넬 이동 사운드 */
    public void PlayOnSidePanelSound()
    {
        audiosource.PlayOneShot(panelSound[1]);
    }

    /* 토끼 이동 사운드 */
    public void PlayOnRabbitWalkSound()
    {
        int randNum = Random.Range(0, 2);

        rabbitWalkTime += Time.deltaTime;

        if (rabbitWalkTime > 1.03f)
        {
            audiosource.PlayOneShot(forestWalkSound[randNum]);
            rabbitWalkTime = 0.0f;
        }
    }

    /* 무(채소) 획득 사운드 */
    public void PlayOnRadishSound()
    {
        audiosource.PlayOneShot(radishSound);
    }

    /* 해바라기 캐는 사운드 */
    public void PlayOnFlowerSound()
    {
        while(digTime < 0.5f)
        {
            digTime += Time.deltaTime;
        }
        audiosource.PlayOneShot(flowerSound);
    }

    /* 차 시동 사운드 */
    public void PlayOnCarStartEngineSound()
    {
        audiosource.PlayOneShot(carSound[0]);
    }

    /* 차 문 닫는 사운드 */
    public void PlayOnCarClosingDoorSound()
    {
        audiosource.PlayOneShot(carSound[1]);
    }

    /* 경로를 통과할 때 나는 사운드 */
    public void PlayOnPathSound()
    {
        audiosource.PlayOneShot(pathSound);
    }

    public void PlayOnForestSound()
    {
        audiosource.clip = forestSound;
        audiosource.volume = 1.0f;
        audiosource.loop = true;
    }
}
