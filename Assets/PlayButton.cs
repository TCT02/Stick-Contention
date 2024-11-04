using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(AudioSource))]
public class PlayButton : MonoBehaviour
{
    AudioSource soundPlayer;
    [SerializeField] AudioClip buttonSound;
    void Start()
    {
        soundPlayer = GetComponent<AudioSource>();

    }
    // Update is called once per frame
    private void OnMouseDown()
    {
        soundPlayer.clip = buttonSound;
        soundPlayer.Play();
        loadMap();
    }

    private void OnMouseOver()
    {
        transform.localScale = new Vector3(6.5f, 2.5f, 1.5f);
    }

    private void OnMouseExit()
    {
        transform.localScale = new Vector3(6, 2, 1);
    }
    void loadMap()
    {
        SceneManager.LoadScene("Map1");
    }

}
