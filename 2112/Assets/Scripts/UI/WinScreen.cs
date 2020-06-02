using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{

    public WaveSpawner spawn;
   
    public GameObject winMenuUI;
    public GameObject dashBar;
    public GameObject hpbar;
   
    void Update()
    {
        if (spawn.won)
        {
          wonGame();  
        }
    }
    void wonGame(){
        winMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }
    public void mainMenu(){
        Time.timeScale = 1f;
        winMenuUI.SetActive(false);
        SceneManager.LoadScene("Menu");
   }
}
