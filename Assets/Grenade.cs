using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    // Start is called before the first frame update
    // Start is called before the first frame update
    int damage;
    float boomRadius;
    float lifeTime; //time before bullet self destructs naturally
    float speed; //speed of bullet

    private Rigidbody rb;
    private SphereCollider boomZone;

    void Start()
    {
        //Weapon stats
        damage = 40;
        boomRadius = 7;
        lifeTime = 2f; //fuse time before explosion
        speed = 10;

        rb = GetComponent<Rigidbody>();
        boomZone = GetComponent<SphereCollider>();

        //One time force on the grenade
        rb.velocity = transform.up * speed;

        Invoke("explode", lifeTime);                

    }

    // Update is called once per frame
    
    void explode()
    {
        print("BOOM");

        boomZone.radius = boomRadius;
        Destroy(gameObject);
    }

    //Will run after the specified amount of seconds in Invoke("selfDestruct", time)
    //Destroys the grenade if it has not hit anything to clear clutter.

    void OnCollisionEnter(Collision coll)
    {
        GameObject collidedWith = coll.gameObject;
        //Check for what the bullet hit and react accordingly

        //If it hits a player (stick figure), deal damage and self destruct
        if (collidedWith.CompareTag("Stick"))
        {
            collidedWith.GetComponent<StickFigure>().CurrHP -= damage;          
        }

    }
}
