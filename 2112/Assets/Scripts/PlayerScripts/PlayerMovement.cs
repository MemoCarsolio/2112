using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public enum PlayerState{
  walk,
  dash
}

public class PlayerMovement : MonoBehaviour
{
    public PlayerState currentState; //State Machine
    public float speed;
    public Rigidbody2D myRigidbody; 
    private Vector2 change;
    public Camera cam;
    public float dashSpeed; //DASH
    private float dashTime; //DASH
    public float startDashTime; //DASH
    private int direction; //DASH
    public GameObject particlesDash; //DASH
    public bool dashed; //DASH
    public SpriteRenderer sRender; //DASH for now
    public CameraMovement camShake; //Shake of camera
    public infoBar dashbar; //DASH
    public infoBar hpbar; //HEALTH
    public playerStats player; //STATS
    public float x,y; //Coordinates that after HOURS realized need them

    public Pause PauseMenu;

    Vector2 mousePos;

    void Start()
    {
      currentState = PlayerState.walk;
      myRigidbody = GetComponent<Rigidbody2D>();
      direction = 0;
      dashed = true;
      dashTime = startDashTime;
    }
    void Update(){

        //All of this has to do with movement and walking
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        getDirection();

        

        if (Input.GetKeyDown(KeyCode.A))
            {
              
                direction = 1;
                 
            }
            else if(Input.GetKeyDown(KeyCode.D))
            {
                direction = 2;
                 
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                direction = 3;
                 
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                direction = 4;
                 
            }
            //This is for Dashing
            if(Input.GetKeyDown(KeyCode.LeftShift) && player.dashCharge == player.dashChargeMax){
                
                
                player.dashCharge = 0;
                currentState = PlayerState.dash;
                dashed = true;
            }


        //Ablity and Bar RegenS
        if (!Pause.GameisPaused)
        {
            regenAblities();
        setBars();
        }
        



        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    
    }
    
    void regenAblities(){
        if(player.dashCharge < player.dashChargeMax){
          player.dashCharge += 1;
        }




    }

    void setBars(){
      dashbar.setSlider(player.dashCharge);
      hpbar.setSlider(player.health);
    }

    void FixedUpdate()
    {

      if (currentState == PlayerState.walk) {
       MoveCharacter();
      }else if(currentState == PlayerState.dash){
        

       camShake.shake = true; 
        dash();
      }

        Vector2 lookDir = mousePos - myRigidbody.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90;
        myRigidbody.rotation = angle;



    }
  
    void dash(){
      if (dashed)
      {
        
        Instantiate(particlesDash, transform.position, Quaternion.identity);
        sRender.enabled = false;
        dashed = false;
         
      }
           
          if (dashTime <= 0)
            {
                
                sRender.enabled = true;
                dashTime = startDashTime;    
                myRigidbody.velocity = Vector2.zero;
                currentState = PlayerState.walk;
                Instantiate(particlesDash, transform.position, Quaternion.identity);

            }else{
                dashTime -= Time.deltaTime;
                

                if (direction == 1)
                {
                    myRigidbody.velocity = Vector2.left * dashSpeed;
                }else if (direction == 2)
                {
                    myRigidbody.velocity = Vector2.right * dashSpeed;
                }else if (direction == 3)
                {
                    myRigidbody.velocity = Vector2.up * dashSpeed;
                }else if (direction == 4)
                {
                    myRigidbody.velocity = Vector2.down * dashSpeed;
                }
            }

        


        
    }
    void getDirection(){
      if (Input.GetKeyDown(KeyCode.A))
            {
                direction = 1;
            }
            else if(Input.GetKeyDown(KeyCode.D))
            {
                direction = 2;
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                direction = 3;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                direction = 4;
            }
    }
    

    void UpdateAnimationWalk(){
     
    }

    void MoveCharacter()
    {
      
        myRigidbody.MovePosition(myRigidbody.position + change.normalized * speed * Time.fixedDeltaTime);

        x = myRigidbody.position.x;
        y = myRigidbody.position.y;

    }
}
