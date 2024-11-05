using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(AudioSource))]


public class Boot : MonoBehaviour
{
    //Variables 
    float lifeTime; //time before the grenade explodes
    float speed; //speed of the intial grenade throw
    float power;
    Vector3 bootPosition;
    bool hasCollided = false;
    private Rigidbody rb;
    AudioSource soundPlayer;
    [SerializeField] AudioClip hit;

    void Start()
    {
        hasCollided = false;
        soundPlayer = GetComponent<AudioSource>();
        soundPlayer.clip = hit;
        //Weapon stats
        lifeTime = 3f; //fuse time before explosion
        speed = 15;
        power = 400;

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
        if (collidedWith.CompareTag("Stick") && hasCollided == false)
        {
            soundPlayer.Play();
            hasCollided = true;
            //If the boot is right, displace left
            if (bootPosition.x > collidedWith.transform.position.x)
            {
                print("Bonk");
                collidedWith.GetComponent<Rigidbody>().AddForce(new Vector3(-power, 100, 0));
                Destroy(gameObject);
            }
            else //Otherwise displace right
            {
                print("Bonk");
                collidedWith.GetComponent<Rigidbody>().AddForce(new Vector3(power, 100, 0));
                Destroy(gameObject);
            }

        }

    }
}
