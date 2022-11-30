using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyObj : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Destroying();
    }

    void Destroying()
    {
        if (SceneManager.GetSceneByName("Gameover").isLoaded || SceneManager.GetSceneByName("Gameclaer").isLoaded)
        {
            Destroy(gameObject);
        }
    }
}
