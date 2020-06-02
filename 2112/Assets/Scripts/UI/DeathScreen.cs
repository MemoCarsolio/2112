using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    public playerStats player;
    public static bool lost;
    public GameObject lostMenuUI;
    public GameObject dashBar;
    public GameObject hpbar;
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        if(player.dead){
            dashBar.SetActive(false);
            hpbar.SetActive(false);
            StartCoroutine(startLost());
        }
        
    }
    
    IEnumerator startLost(){
        yield return new WaitForSeconds(3f);
        lostGame();
        yield break;
    }
    void lostGame(){
        lostMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void mainMenu(){
        Time.timeScale = 1f;
        lostMenuUI.SetActive(false);
        SceneManager.LoadScene("Menu");
   }
}
