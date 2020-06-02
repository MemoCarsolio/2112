using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float speed = 0f;
    public Rigidbody2D rb;

    void Start()
    {
      rb.velocity = transform.up * speed;
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D hitInfo){

        EnemyScript enemy = hitInfo.GetComponent<EnemyScript>();
        if (enemy != null)
        {
            
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
