using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;

public class StickFigure : MonoBehaviour
{

    public int CurrHP;
    public int MaxHP;
    public bool isDead = false;

    //reference to opposing player
    public GameObject otherPlayer;

    // Start is called before the first frame update
    void Start()
    {
        CurrHP = 100;
        MaxHP = 100;
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrHP <= 0) 
        {
            isDead = true;
        }            
        
    }

    void OnCollisionEnter(Collision coll)
    {    
        GameObject collidedWith = coll.gameObject;

        //If the character falls into a killzone like water they will be out of the game.
        if (collidedWith.CompareTag("KillZone"))
        {                   
            CurrHP = 0;

        }
       
    }

    public void die()
    {
        
    }

}