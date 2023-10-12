using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    damageable damageable;
    GameOverScence gameOver;

    private void Awake()
    {
        damageable = GetComponent<damageable>();
        gameOver = GetComponent<GameOverScence>();
    }

    public void Update() 
    {
        if(damageable.IsAlive != true)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
