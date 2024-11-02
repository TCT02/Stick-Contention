using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Action : MonoBehaviour
{   
    //Movement Variables
    public float speed;
    public float defSpeed = 7;

    public float jumpPower;
    public float defjumpPower = 500;
    public float moveRange = 5f;

    Vector3 groundPos = new Vector3(0, 0, 0);
    Vector3 initialPos = new Vector3(0, 0, 0);
   

    public bool facingRight;

    public bool canJump = false;
    public bool onGround = false;

    //Action Mode variables
    public bool moveMode = false; //Indicates movement phase
    public bool attackMode = false; //Indicates attack phase (for attacks)
    public bool finished = false; //Indicates finished action phase for this player.

    public bool gunMode = false; //Indicates 
    public bool grenadeMode = false; //Indicates
    public bool bootMode = false; //Indicates
    
    public Texture2D cursorArrow;
    public Texture2D cursorSniper;
    public Texture2D cursorGrenade;
    public Texture2D cursorBoot;

    public GameObject bulletPrefab;
    public GameObject bullet;
    public Vector3 activeBP;

    private Rigidbody rb;
    //Vector3 pos;
    // Gun pointing variables.
    Vector3 mousePos;
    float angle;

    // Start is called before the first frame update
    void Start()
    {

        //Set Movement Parameters
        speed = defSpeed;
        jumpPower = defjumpPower;
        moveRange = 5;

        //Initializes all modes
        beginTurn(); 
        /*
        initialPos = transform.position;
        moveMode = true;
        actionMode = false;
        finished = false; 
        */

        //Checks
        onGround = true;

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() //Movement and Attack controls are handled here
    {
        /*
        Vector3 difference;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        difference = (mousePos - transform.position);
        difference.Normalize();
        angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg - 90f;
        //angle = Mathf.Atan2(Input.mousePosition.y - transform.position.y, Input.mousePosition.x - transform.position.x) * Mathf.Rad2Deg - 90f;


        if (false) 
        { 
            print("Bang bang");
            //Change cursor to a Crosshair
            Cursor.SetCursor(cursorSniper, Vector2.zero, CursorMode.ForceSoftware);

            //code to aim gun
            Instantiate(bulletPrefab, gameObject.transform.position + new Vector3(0, 3, 0), Quaternion.Euler(0,0,angle));
            bullet = GameObject.Find("Bullet(Clone)");

            if (facingRight == true)
            {
                //GameObject BPR = gameObject.GetComponentInChildren<"BulletPointR">();
                activeBP = transform.Find("BulletPointR").position;
                bullet.transform.position = activeBP;
            }
            else //if facing left instead,
            {
                activeBP = transform.Find("BulletPointL").position;
                bullet.transform.position = activeBP;
            }

            gunMode = false; //delete this later since it can be done in endTurn()

            //endTurn();
        }
        */
        if (((transform.position.x <= initialPos.x + moveRange) && (transform.position.x >= initialPos.x - moveRange)) && moveMode == true)
        { //Movement Controls 
            if (Input.GetKey(KeyCode.D)) //Rightward movement
            {

                facingRight = true;
                if (!onGround) //When airborne, disable horizontal controls
                {
                    transform.position += Vector3.right * 0 * Time.deltaTime;

                }
                else //Otherwise, the character is grounded and can move
                {
                    transform.position += Vector3.right * speed * Time.deltaTime;
                }

            }
            if (Input.GetKey(KeyCode.A)) //Leftward movement
            {

                facingRight = false;
                if (!onGround) //When airborne, disable horizontal controls
                {
                    transform.position += Vector3.left * 0 * Time.deltaTime;
                }
                else //Otherwise, the character is grounded and can move
                {
                    transform.position += Vector3.left * speed * Time.deltaTime;
                }

            }
            if (Input.GetKeyDown(KeyCode.Space) && canJump == true) //Jump action
            {

                //print("Space is being pressed");

                if (facingRight == true) //When facing right, add a force right
                {
                    rb.AddForce(new Vector3(jumpPower, 1000, 0));
                }
                else if (facingRight == false) //When facing left, add a force left
                {
                    rb.AddForce(new Vector3(-jumpPower, 1000, 0));
                }

                Invoke("disableJump", 1);

            }
            if (Input.GetKeyDown(KeyCode.T)) //force end movement mode 
            {
                moveMode = false;
            }

        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            moveMode = false;
            attackMode = false;
        }
        if (moveMode == false) //if(finished == false) //Alternate condition which turns on combat controls.
        { //Attack Controls
            
            //print(attackMode);
            //display some kind of UI?
            if (Input.GetKeyDown(KeyCode.Alpha1) && attackMode == false) //Begin Gun targetting
            {
                attackMode = true;
                gunMode = true;
                print("1 was pressed");                       

            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && attackMode == false) //Begin Grenade targetting
            {
                attackMode = true;
                print("2 was pressed");

                //Change cursor to a Grenade
                Cursor.SetCursor(cursorGrenade, Vector2.zero, CursorMode.ForceSoftware);

                //code to aim grenade

                //endTurn();
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) && attackMode == false) //Begin Boot targetting
            {
                attackMode = true;
                print("3 was pressed");

                //Change cursor to a Boot
                Cursor.SetCursor(cursorBoot, Vector2.zero, CursorMode.ForceSoftware);

                //code to aim boot
     

                //endTurn();
            }
            if (Input.GetKeyDown(KeyCode.Alpha4) && finished == false) //Back out of action choice
            {
                attackMode = false;
                //Add code to hide any visuals related to each action

                Cursor.SetCursor(cursorArrow, Vector2.zero, CursorMode.ForceSoftware);
                print("4 was pressed, choice reversed, choose your action");


            }
            if (Input.GetKeyDown(KeyCode.T) && finished == false) //Force Terminate turn
            {
                print("T was pressed");
                Cursor.SetCursor(cursorArrow, Vector2.zero, CursorMode.ForceSoftware);
                //endTurn();
            }

            if (gunMode == true)
            {
                //Change cursor to a Crosshair
                Cursor.SetCursor(cursorSniper, Vector2.zero, CursorMode.ForceSoftware);
                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 difference;
                    mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    difference = (mousePos - transform.position);
                    difference.Normalize();
                    angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg - 90f;

                    print("Bang bang");

                    //code to aim gun
                    Instantiate(bulletPrefab, gameObject.transform.position + new Vector3(0, 3, 0), Quaternion.Euler(0, 0, angle));
                    bullet = GameObject.Find("Bullet(Clone)");

                    if (facingRight == true)
                    {
                        //GameObject BPR = gameObject.GetComponentInChildren<"BulletPointR">();
                        activeBP = transform.Find("BulletPointR").position;
                        bullet.transform.position = activeBP;
                    }
                    else //if facing left instead,
                    {
                        activeBP = transform.Find("BulletPointL").position;
                        bullet.transform.position = activeBP;
                    }

                    gunMode = false; //delete this later since it can be done in endTurn()

                    //endTurn();
                }
            }

        }      

    }

    private void FixedUpdate()
    {
       
    }
    void beginTurn()
    {
        //Initialize turn for player.
        initialPos = transform.position; //Get new position to base movement distance.
        moveMode = true;
        attackMode = false;
        gunMode = false; //Indicates gun action
        grenadeMode = false; //Indicates grenade action
        bootMode = false; //Indicates boot action

        finished = false; //Indicates finished action phase for this player.

    }
    void endTurn()
    {
        //End player's turn and resets modes except for movement.
        //Movemode will be activated in game manager
        //Cursor is also reset to default.
        Cursor.SetCursor(cursorArrow, Vector2.zero, CursorMode.ForceSoftware);
        attackMode = false;
        gunMode = false; //Indicates gun action
        grenadeMode = false; //Indicates grenade action
        bootMode = false; //Indicates boot action

        finished = true; //Indicates finished action phase for this player.
    }


    void disableJump()
    {
       // print("Disabled Jump");
        canJump = false;
    }
    void enableJump()
    {
       // print("Enabled Jump");
        canJump = true;
        //onGround = true;

    }

    void OnCollisionEnter(Collision coll)
    {

        GameObject collidedWith = coll.gameObject;

        if (collidedWith.CompareTag("Ground"))
        {
            groundPos = collidedWith.transform.position;
            //speed = defSpeed;
            onGround = true;

            Invoke("enableJump", 1);
            //enableJump();

        }
        if (collidedWith.CompareTag("Stick"))
        {
            onGround = true;
        }

    }
    void OnCollisionExit(Collision coll)
    {
        GameObject collidedWith = coll.gameObject;

        if (collidedWith.CompareTag("Ground"))
        {
            onGround = false;
            //rb.AddForce(Vector3.down * 100);
            //canJump = false;
        }
        if (collidedWith.CompareTag("Stick"))
        {
            onGround = false;
        }
    }
}