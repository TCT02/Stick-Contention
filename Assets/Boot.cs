using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boot : MonoBehaviour
{
    //Variables 
    float lifeTime; //time before the grenade explodes
    float speed; //speed of the intial grenade throw
    float power;
    Vector3 bootPosition;

    private Rigidbody rb;

    void Start()
    {
        //Weapon stats
        lifeTime = 3f; //fuse time before explosion
        speed = 10;
        power = 500;

        rb = GetComponent<Rigidbody>();

        //One time force on the grenade
        rb.velocity = transform.up * speed;

        Invoke("selfDestruct", lifeTime);

    }

    private void Update()
    {
        //Track the position of the grenade to the determine direction of applied explosion force
        bootPosition = transform.position;

    }

    void selfDestruct()
    {// handles the explosion of the grenade
        

        Destroy(gameObject);
    }

    //Will run after the specified amount of seconds in Invoke("selfDestruct", time)
    //Destroys the grenade if it has not hit anything to clear clutter.

    void OnCollisionEnter(Collision coll)
    {
        GameObject collidedWith = coll.gameObject;
        //Check for what the explosion hit and react accordingly

        //If it hits a player (stick figure), deal effects and self destruct
        if (collidedWith.CompareTag("Stick"))
        {

            //If the boot is right, displace left
            if (bootPosition.x > collidedWith.transform.position.x)
            {
                print("Bonk");
                collidedWith.GetComponent<Rigidbody>().AddForce(new Vector3(-power, 0, 0));
            }
            else //Otherwise displace right
            {
                print("Bonk");
                collidedWith.GetComponent<Rigidbody>().AddForce(new Vector3(power, 100, 0));
            }

        }

    }
}
