using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 tpExit;
    void Start()
    {
        tpExit = transform.Find("Exit").position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision coll)
    {
        GameObject collidedWith = coll.gameObject;

        if (collidedWith.CompareTag("Stick"))
        {
            collidedWith.transform.position = tpExit;
        }

    }

}
