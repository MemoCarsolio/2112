using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveSpawner : MonoBehaviour
{

    public enum  spawnState{
        spawning,
        wait,
        counting,
        done
    }
   

    [System.Serializable]//To be able to visualize in Unity
    public class Wave{
       public string name;
       public Transform enemy;
       public int count;
       public float rate;   
     }
     public bool won;
     public Wave[] waves; //Array of waves
     public Transform[] spawnPoints;
     private int nextWave = 0;
     public float timeBetweenWaves = 5f;
     private float waveCountDown;
     private float searchCountDown = 1f;
     private spawnState state = spawnState.counting;

     //Stuff to call out wave
     public bool needText;
     public string placeName;
     public GameObject text;
     public TextMeshProUGUI placeText;


    void Start(){
         //placeText = FindObjectOfType<TextMeshPro>();
        waveCountDown = timeBetweenWaves;
        needText = true;
        won = false;

    }
    void Update() {


        if (!won)
        {
         if (state == spawnState.wait) //If all contitions are right we start counting down to a new round
        {
            if (!anyAlive()){
                waveDone();
                needText = true;
                return;
            }
            else{
                return;
            }
        }

        if (waveCountDown <= 0 )
        {
           if(state != spawnState.spawning){
               StartCoroutine(spawnWave(waves[nextWave]));
           }
        }
        else
        {
            if (needText)
            {
                StartCoroutine(showText());
            }
            waveCountDown -= Time.deltaTime;
        }   
        }
        
        
    }
    void waveDone(){
        

        
        state = spawnState.counting;
        waveCountDown = timeBetweenWaves;
        if (nextWave + 1 > waves.Length - 1){
            won = true;
        }else{
            nextWave++;
        }

        

    }

    //This function validates if there is still enemies available
    bool anyAlive(){
        searchCountDown -= Time.deltaTime;
        if (searchCountDown <= 0f){
            searchCountDown = 1f;
            
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }
        return true;

    }

    IEnumerator spawnWave(Wave _w){
        state = spawnState.spawning;
        for (int i = 0; i < _w.count; i++)
        {
            spawnEnemy(_w.enemy);
            yield return new WaitForSeconds( 1f/_w.rate);
        }


        state = spawnState.wait;

        yield break;
    }
    IEnumerator showText(){

        text.SetActive(true);
        placeText.text = waves[nextWave].name;
        yield return new WaitForSeconds(3f);
        text.SetActive(false);
        needText = false;
    }

    void spawnEnemy(Transform _enemy){

        
        
        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(_enemy, _sp.position, _sp.rotation);
    }
}
