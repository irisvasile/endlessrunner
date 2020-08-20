//using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Events : MonoBehaviour
{
    // Start is called before the first frame update
    public void ReplayGame()
    {
        SceneManager.LoadScene("Game");
    }

    // Update is called once per frame
    public void QuitGame()
    {
        Application.Quit();
    }
}
