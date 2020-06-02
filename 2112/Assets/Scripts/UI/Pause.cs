using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public static bool GameisPaused;
    public GameObject pauseMenuUI;




    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(GameisPaused){
                Resume();
            }
            else
            {
                pauseGame();
            }
        }
        
    }
    public void Resume(){
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameisPaused = false;
    }
    void pauseGame(){
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameisPaused = true;
    }



    public void quitGame(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
   }
}
