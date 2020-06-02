using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;


//This is a state machine that dictates what the enemy should or should not be doing
public enum EnemyState{
  idle,
  patrol, //Done
  pursue, //Done
  keepDistance, //Done
  won
}

public class EnemyScript : MonoBehaviour
{

    //Lua Script Set Up
    private string luaText; //This sting will containg the Lua Code
    private Script luaScript; //This scrip is what we will be able to access the Lua code from
    private DynValue changeX, changeY;//Values of the point we aquire from the lua script
    //Basic Information of the Enemy
    private Rigidbody2D enemyRB; 
    private Vector2 position;
    private int speed;
    public int startHp;
    private int hp;
    private float enemyX;
    private float enemyY;
    public GameObject blood;
    //State variable
    public EnemyState currentState;
    //Extra info for AI
    private PlayerMovement player;
    private playerStats pstats;
    private Vector2 change; //direction of the Movement of AI
    private int maxDiference; //To calculate how many points of diference needed for path finding
    private int keepDDistance;//How far away from the player Enemy has to be
    //This small section is for the patroling AI
    private int patrolLimit;//Limit of steps in that direction
    private int patrolTurn;//
    private float pdirX;//
    private float pdirY;// 
    private int viewDistance;//This variable is used so when the player is detected 
    //Attack Variables
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletForce = 20f;
    public int lrco; //   [Long Range Cool Off] This is to make sure they don't shoot super fast
    public int lrcoStart;



    // Start is called before the first frame update
    void Start()
    {

        player = FindObjectOfType<PlayerMovement>();
        pstats = FindObjectOfType<playerStats>();

        //First the Enemy Set up
        enemyRB = GetComponent<Rigidbody2D>();
        speed = 5;
        startHp = 25;
        hp = startHp;
        currentState = EnemyState.patrol;
        //Data that will be used by the script for AI behaviour
        maxDiference = 10;
        keepDDistance = 7;
        //Lua scripting set up
        luaScript = new Script();
        loadLua();
        luaScript.DoString( luaText );//2 hours to realize I need this, because I took it away at one point and everything broke


        
        //Attacking Set up
        lrcoStart = 50;
        lrco = lrcoStart;
        //Patrol Set up
        patrolLimit = 20;
        pdirX = 1; //This two will keep on changing
        pdirY = 0; //Im just putting this values to test
        patrolTurn = 0;
        viewDistance = 10;

         
    

        
    }

    // Update is called once per frame
    void Update()
    {
         enemyX = enemyRB.position.x;
         enemyY = enemyRB.position.y;
         behaviourState();

         if (pstats.dead)
         {
            currentState = EnemyState.won;
          
         }
    }
    void FixedUpdate() {
        
        

        if (currentState == EnemyState.pursue)
        {
            calcRotation();
            longRangeAttack();
            pathFinding();
        }else if (currentState == EnemyState.keepDistance)
        {   
            calcRotation();
            longRangeAttack();
            keepDistance();
        }else if (currentState == EnemyState.patrol || currentState == EnemyState.won)
        {
            patrol();
        }
       
        
    }
    public void takeDamage(string dmg){


        if (dmg == "Long Range")
        {
           hp -= pstats.damageLR; 
           Instantiate(blood, transform.position, Quaternion.identity);

        }


        
        if (hp <= 0)
        {
            death();
        }
    }
    void death(){
        Destroy(gameObject);
    }

    void loadLua(){
        
        string filePath = System.IO.Path.Combine( Application.dataPath, "StreamingAssets/luas.lua");
        luaText= System.IO.File.ReadAllText(filePath);
    
    }
    void behaviourState(){
        //At first we are gonna get some information with help from our script
        float minDistance = 0f;

        if (currentState != EnemyState.won)
        {
            DynValue dynMinDistance = luaScript.Call( luaScript.Globals["minDistance"],enemyX,enemyY,player.x,player.y);
            minDistance = (float)dynMinDistance.Number;
             if (minDistance <= viewDistance)
        {
            
            if (hp > startHp/2)
            {
                currentState = EnemyState.pursue;
            }
            else if (hp <= startHp/2)
            {
                currentState = EnemyState.keepDistance;
                speed = 10;
            }
            
        }
        }
    
       

    }

    void pathFinding(){
        
        changeX = luaScript.Call( luaScript.Globals["pathFinding"], enemyX, enemyY,player.x, player.y,maxDiference,"x");
        changeY = luaScript.Call( luaScript.Globals["pathFinding"], enemyX, enemyY,player.x, player.y,maxDiference,"y"); 

       changePoint();


        moveEnemy();
    }
    void keepDistance(){
        changeX = luaScript.Call( luaScript.Globals["keepDistance"], enemyX,player.x, keepDDistance);
        changeY = luaScript.Call( luaScript.Globals["keepDistance"], enemyY,player.y, keepDDistance);

        changePoint();
        moveEnemy();
        
    }
    void patrol(){
        if (patrolTurn >= patrolLimit)
        {
            pdirX = Random.Range(-1,2); 
            pdirY = Random.Range(-1,2);  

            patrolLimit = Random.Range(10,100);
            patrolTurn = 0;
        }
        changeX = luaScript.Call( luaScript.Globals["patrol"], enemyX,pdirX, patrolTurn, patrolLimit);
        changeY = luaScript.Call( luaScript.Globals["patrol"], enemyY,pdirY, patrolTurn, patrolLimit);


        
        patrolTurn += 1;
        changePoint();
        moveEnemy();


    }

    void moveEnemy(){
        enemyRB.MovePosition(enemyRB.position + change.normalized * speed * Time.fixedDeltaTime);
    }
    void changePoint(){
        change.x = (float)changeX.Number;
        change.y = (float)changeY.Number;
    }
    void longRangeAttack(){
        if (lrco >= lrcoStart)
        {
            GameObject b = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            //Rigidbody2D rb = b.GetComponent<Rigidbody2D>();   
            lrco = 0;
        }
        lrco += 1;
    }

    void calcRotation(){
        Vector2 lookDir = player.myRigidbody.position - enemyRB.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90;
        enemyRB.rotation = angle;
    }


    


}
