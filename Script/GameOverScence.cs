using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScence : MonoBehaviour
{
    damageable damageable;
    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -1);
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -2);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void SuperEnemy()
    {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }


}
