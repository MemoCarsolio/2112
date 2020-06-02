using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBlueFB : MonoBehaviour
{
    public float speed = 40f;
    public Rigidbody2D rb;
    public int dmg;
    //Player Stats
   

    void Start()
    {
      rb.velocity = transform.up * speed;
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D hitInfo){

        playerStats player = hitInfo.GetComponent<playerStats>();
        
        if (player != null)
        {
           player.takeDamage(dmg);
            
        }
      StartCoroutine(death());

    }

    //Coroutine that waits until the bullet should dispear
    private IEnumerator death(){
      rb.velocity = transform.up * 0f;
      yield return new WaitForSeconds(1f);
      Destroy(gameObject);
    }
}
