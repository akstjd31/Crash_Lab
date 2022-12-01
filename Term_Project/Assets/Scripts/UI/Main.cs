using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    void DoExitGame()
    {
        Application.Quit();
    }

    /* Start 버튼 클릭 */
    public void OnClickStartButton()
    {
        SceneManager.LoadScene("Forest");
    }

    /* Quit 버튼 클릭 */
    public void OnClickQuitButton()
    {
        DoExitGame();
    }
}
