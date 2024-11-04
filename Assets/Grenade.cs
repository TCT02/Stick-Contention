using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Grenade : MonoBehaviour
{
    //Variables 
    int damage;
    float boomRadius;
    float lifeTime; //time before the grenade explodes
    float speed; //speed of the intial grenade throw
    float blastPower;
    Vector3 nadePosition;
    bool hasCollided = false;

    private Rigidbody rb;
    private SphereCollider boomZone;
    [SerializeField] Color blastColor;

    AudioSource soundPlayer;
    [SerializeField] AudioClip explosion;

    void Start()
    {
        hasCollided = false;
        soundPlayer = GetComponent<AudioSource>();
        soundPlayer.clip = explosion;
        //Weapon stats
        damage = 40;
        boomRadius = 7;
        lifeTime = 3f; //fuse time before explosion
        speed = 10;
        blastPower = 300;

        rb = gameObject.GetComponent<Rigidbody>();
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
        //transform.GetComponent<Material>().color = blastColor;
        soundPlayer.Play();
        
        transform.Find("Fuse").GetComponent<MeshRenderer>().enabled = false;
        transform.localScale = new Vector3(boomRadius, boomRadius, .1f); 
        
        rb.velocity = new Vector3(0, rb.velocity.y,0);
        
        Invoke("removeSelf", 1);
    }
    private void removeSelf()
    {
        Destroy(gameObject);
    }

    //Will run after the specified amount of seconds in Invoke("selfDestruct", time)
    //Destroys the grenade if it has not hit anything to clear clutter.

    void OnCollisionEnter(Collision coll)
    {
        GameObject collidedWith = coll.gameObject;
        //Check for what the explosion hit and react accordingly

        //If it hits a player (stick figure), deal damage and self destruct
        if (collidedWith.CompareTag("Stick") && hasCollided == false)
        {
            hasCollided = true;
            soundPlayer.clip = explosion;
            soundPlayer.Play();
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
