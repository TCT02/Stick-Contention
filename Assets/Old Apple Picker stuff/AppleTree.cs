 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;

public class AppleTree : MonoBehaviour
{
    [Header("Inscribed")]
    // a
    // Prefab for instantiating apples
    public GameObject applePrefab;

    // Speed at which the AppleTree moves
    public float speed = 1f;

    // Distance where AppleTree turns around
    public float leftAndRightEdge = 10f;

    // Chance that the AppleTree will change directions
    public float changeDirChance = 0.1f;

    // Seconds between Apples instantiations
    public float appleDropDelay = 1f;

    void Start()
    {
        // Start dropping apples
        Invoke("DropApple", 2f);
        // b
    }

    void DropApple() {
        // b
         GameObject apple = Instantiate<GameObject>(applePrefab); // c
         apple.transform.position = transform.position;
        // d
         Invoke("DropApple", appleDropDelay);
        // e
    }


    void Update()
    {
        // Basic Movement

        Vector3 pos = transform.position;
        // b
        pos.x += speed * Time.deltaTime;
        // c
        transform.position = pos;


        // Changing Direction
        if (pos.x < -leftAndRightEdge)
        {
            // a
            speed = Mathf.Abs(speed); // Move right
                                      // b
        }
        else if (pos.x > leftAndRightEdge)
        {
            // c
            speed = -Mathf.Abs(speed); // Move left
                                       // c
        }
        else if (Random.value < changeDirChance)
        {
            speed *= 1;
        }

    }

  
}


