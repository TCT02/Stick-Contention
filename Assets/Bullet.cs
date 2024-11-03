using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    int damage;
    int range;
    float lifeTime; //time before bullet self destructs naturally
    float speed; //speed of bullet
    public Vector3 direction;

    private Rigidbody rb;

    void Start()
    {
        damage = 20;
        lifeTime = .5f;
        speed = 30;
        //Bullet will begin not moving
        //direction should be immediately assigned the vector of the mouse location.

        rb = GetComponent<Rigidbody>();
        if (gameObject)
        {
            Destroy(gameObject, lifeTime);
        }   

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Moves the bullet upwards in the direction based on the orientation of the bullet.
        rb.velocity = transform.up * speed;
    }

    //Will run after the specified amount of seconds in Invoke("selfDestruct", time)
    //Destroys the bullet if it has not hit anything to clear clutter.

    void OnCollisionEnter(Collision coll)
    {
        GameObject collidedWith = coll.gameObject;
        //Check for what the bullet hit and react accordingly

        //If it hits a player (stick figure), deal damage and self destruct
        if (collidedWith.CompareTag("Stick"))
        {
            collidedWith.GetComponent<StickFigure>().CurrHP -= damage;
            Destroy(gameObject);
        }
        else // If it hits anything else, self destruct.
        {
           Destroy(gameObject);
        }


    }
}
