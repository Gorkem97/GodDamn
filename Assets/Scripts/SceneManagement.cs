using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public void OpenScene(int whichScene)
    {
        SceneManager.LoadScene(whichScene);
    }
    public void OpenSceneName(string whichScene)
    {
        SceneManager.LoadScene(whichScene);
    }
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
