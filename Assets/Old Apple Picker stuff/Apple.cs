using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{

    // Update is called once per frame
    public static float bottomY = -20f;
    void Update()
    {
        if (transform.position.y < bottomY)
        {
            Destroy(this.gameObject);
            // c
         }
    }
}
