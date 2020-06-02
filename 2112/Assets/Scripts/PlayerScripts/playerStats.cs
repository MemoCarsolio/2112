using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerStats : MonoBehaviour
{
    public int health;
    public int mana;
    public int dashChargeMax;
    public int dashCharge;
    public int lvl;
    public bool dead;
    //Attacks
    public int damageLR; //Long Range Damage
    public GameObject blood;
   
    


    // Start is called before the first frame update
    void Start()
    {
        health = 100;
        mana = 10;
        dashChargeMax = 100;    
        dashCharge = 100;
        damageLR = 5;
        dead = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(int dmg){
        health -= dmg;
        Instantiate(blood, transform.position, Quaternion.identity);
        if (health <= 0)
        {
            dead = true;
            die();
        }
    }
    void die(){
        Destroy(gameObject);
    }
    

}
