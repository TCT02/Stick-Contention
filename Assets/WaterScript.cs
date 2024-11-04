using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WaterScript : MonoBehaviour
{
    AudioSource soundPlayer;
    [SerializeField] AudioClip splash;
    // Start is called before the first frame update
    void Start()
    {
        soundPlayer = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject collidedWith = collision.gameObject;

       // if (collidedWith.CompareTag("Stick"))
        //{
        //    soundPlayer.clip = splash;
          //  soundPlayer.Play();
       // }

    }

}
