using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class StickFigure : MonoBehaviour
{

    public float CurrHP;
    public float MaxHP;
    public bool isDead = false;

    //Sound Effect
    public AudioClip heal;
    AudioSource sound;

    // Start is called before the first frame update
    void Start()
    {
        //Initialize Sound
        sound = GetComponent<AudioSource>();
        sound.clip = heal;

        //Initialize Status
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
        if (collidedWith.CompareTag("MedKit"))
        {
           sound.Play();
        }
       
    }

}
