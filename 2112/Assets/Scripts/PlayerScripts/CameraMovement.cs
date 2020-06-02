using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player;
 
    public float smoothing;
    public Vector2 minPosition;
    public Vector2 maxPosition;
    public float Magnitude;
    public bool shake;
    public int shakeTime;
    private int shakeTimeCounter;

    // Start is called before the first frame update
    void Start()
    {
        shakeTimeCounter = 0;
        shake = false;
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
     //transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);   

    if (player != null)
    {
        if (transform.position != player.position)
        {
            Vector3 targetPosition = new Vector3(player.transform.position.x,player.transform.position.y,transform.position.z);
            if (shake)
            {
                float x = Random.Range(-1f,1f) * Magnitude;
                float y = Random.Range(-1f,1f) * Magnitude;

                targetPosition.x = Mathf.Clamp(targetPosition.x * x,minPosition.x,maxPosition.x);
                targetPosition.y = Mathf.Clamp(targetPosition.y * y,minPosition.y,maxPosition.y);
                shakeTimeCounter += 1;
                
            }else
            {
                //Clamping idk what this is yet
            targetPosition.x = Mathf.Clamp(targetPosition.x,minPosition.x,maxPosition.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y,minPosition.y,maxPosition.y);
                
            }
            if(shakeTimeCounter>=shakeTime){
                shake = false;
                shakeTimeCounter = 0;
            }

            transform.position = Vector3.Lerp(transform.position,targetPosition,smoothing);
            
        }

    }
    

    }
    



    //Corrutine that in the END IT DOESN'T EVEN MATTER
    public IEnumerator shakeCam(float Duration, float Magnitude){
       
        Vector3 targetPosition = new Vector3(player.transform.position.x,player.transform.position.y,transform.position.z);
        float elapsed = 0.0f;
        while(elapsed < Duration){
            float x = player.transform.position.x * Magnitude;
            float y = player.transform.position.y * Magnitude;

            targetPosition.x = Mathf.Clamp(x,minPosition.x,maxPosition.x);
            targetPosition.y = Mathf.Clamp(y,minPosition.y,maxPosition.y);

            transform.position = new Vector3(targetPosition.x,targetPosition.y, transform.position.z);
            elapsed += Time.deltaTime;
            yield return null;

        }
        
    }
}
