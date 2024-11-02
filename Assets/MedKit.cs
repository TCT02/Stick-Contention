using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MedKit : MonoBehaviour
{
    // Start is called before the first frame update
    private int healVal;
    private float rotateVal;

    void Start()
    {
        healVal = 20;
        rotateVal = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rotateVal += 2f;
        gameObject.transform.rotation = Quaternion.Euler(0, rotateVal, 0);
    }

    private void OnCollisionEnter(Collision coll)
    {
        GameObject collidedWith = coll.gameObject;

        if (collidedWith.CompareTag("Stick"))
        {
            int currHP = collidedWith.GetComponent<StickFigure>().CurrHP;
            int maxHP = collidedWith.GetComponent<StickFigure>().MaxHP;
            if (currHP + healVal > maxHP)
            {
                collidedWith.GetComponent<StickFigure>().CurrHP = maxHP;
            }
            else
            {
                collidedWith.GetComponent<StickFigure>().CurrHP += healVal;
            }
            Destroy(gameObject);
            
        }
    }
}
