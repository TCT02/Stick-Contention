using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    //Variables 
    int damage;
    float boomRadius;
    float lifeTime; //time before the grenade explodes
    float speed; //speed of the intial grenade throw
    float blastPower;
    Vector3 nadePosition;

    private Rigidbody rb;
    private SphereCollider boomZone;

    void Start()
    {
        //Weapon stats
        damage = 40;
        boomRadius = 7;
        lifeTime = 2f; //fuse time before explosion
        speed = 10;
        blastPower = 300;

        rb = GetComponent<Rigidbody>();
        boomZone = GetComponent<SphereCollider>();

        //One time force on the grenade
        rb.velocity = transform.up * speed;

        Invoke("explode", lifeTime);                

    }

    private void Update()
    {
        //Track the position of the grenade to the determine direction of applied explosion force
        nadePosition = transform.position;
        
    }

    void explode()
    {// handles the explosion of the grenade
        print("BOOM");

        boomZone.radius = boomRadius;
        Destroy(gameObject);
    }

    //Will run after the specified amount of seconds in Invoke("selfDestruct", time)
    //Destroys the grenade if it has not hit anything to clear clutter.

    void OnCollisionEnter(Collision coll)
    {
        GameObject collidedWith = coll.gameObject;
        //Check for what the explosion hit and react accordingly

        //If it hits a player (stick figure), deal damage and self destruct
        if (collidedWith.CompareTag("Stick"))
        {
            //Inflict HP damage to target
            collidedWith.GetComponent<StickFigure>().CurrHP -= damage;

            //rb.AddForce(new Vector3(-jumpPower, 1000, 0));
            //If the nade is right, displace left
            if (nadePosition.x > collidedWith.transform.position.x)
            { 
                collidedWith.GetComponent<Rigidbody>().AddForce(new Vector3(-blastPower, 100, 0));
            }
            else //Otherwise displace right
            {
                collidedWith.GetComponent<Rigidbody>().AddForce(new Vector3(blastPower, 100, 0));
            }

        }

    }
}
