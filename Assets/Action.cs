using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//using UnityEngine.UIElements;

public class Action : MonoBehaviour
{   
    //Movement Variables
    float speed;
    float defSpeed = 7;
    
    float jumpPower;
    float defjumpPower = 500;
    float moveRange = 7f;

    Vector3 groundPos = new Vector3(0, 0, 0);
    Vector3 initialPos = new Vector3(0, 0, 0);
   

    public bool facingRight;

    bool canJump = false;
    bool onGround = false;

    //Action Mode variables
    bool moveMode = false; //Indicates movement phase
    bool attackMode = false; //Indicates attack phase (for attacks)
    public bool finished = false; //Indicates finished action phase for this player.

    bool gunMode = false; //Indicates 
    bool grenadeMode = false; //Indicates
    bool bootMode = false; //Indicates

    [SerializeField] Texture2D cursorArrow;
    [SerializeField] Texture2D cursorSniper;
    [SerializeField] Texture2D cursorGrenade;
    [SerializeField] Texture2D cursorBoot;

    //Prefab Variables
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject bullet;
    [SerializeField] Vector3 bulletPoint; //spawn location of bullets

    [SerializeField] GameObject grenadePrefab;
    [SerializeField] GameObject grenade;
    [SerializeField] Vector3 throwPoint; //spawn location of throwables

    [SerializeField] GameObject bootPrefab;
    GameObject boot;

    //Physics
    private Rigidbody rb;

    //Basic character Frames
    [SerializeField] Material idle;
    [SerializeField] Material walk1;
    [SerializeField] Material walk2;
    [SerializeField] Material gunStance;
    [SerializeField] Material grenadeStance;
    [SerializeField] Material bootStance;
    [SerializeField] MeshRenderer currSprite; 

    // Weapon Aiming variables.
    Vector3 mousePos;
    float angle;

    // Start is called before the first frame update
    void Start()
    {

        //Set Movement Parameters
        speed = defSpeed;
        jumpPower = defjumpPower;
        moveRange = 7;
        currSprite = transform.Find("Quad").GetComponent<MeshRenderer>();

        //Initializes all modes
        beginTurn(); 
        //Checks
        onGround = true;

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() //Movement and Attack controls are handled here
    {
        //Exit point of projectiles is always updated to a point offset from the character.
        bulletPoint = transform.Find("BulletPoint").position;
        throwPoint = transform.Find("ThrowPoint").position;
        //If A OR D are held down AND moveMode is active, play this 2 frame animation


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
                    transform.rotation = Quaternion.Euler(0, 0, 0);
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
                    transform.rotation = Quaternion.Euler(0, 180, 0);
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
            if (Input.GetKeyDown(KeyCode.S)) //force end movement mode 
            {
                //Turns off moveMode and goes to the following else statement.
                //rb.velocity = new Vector3(0, rb.velocity.y, 0);
                moveMode = false;
                //attackMode = false;
                print("Movemode terminated by choice");
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

            //If the mouse is left of the character, change orientation.            
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (mousePos.x > gameObject.transform.position.x)
            { //If character is facing right do..
                facingRight = true;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            { //If character is facing left do..
                facingRight = false;
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
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
                grenadeMode = true;
                print("2 was pressed");

                //Change cursor to a Grenade
                Cursor.SetCursor(cursorGrenade, Vector2.zero, CursorMode.ForceSoftware);

                //endTurn();
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) && attackMode == false) //Begin Boot targetting
            {
                attackMode = true;
                print("3 was pressed");

                //Change cursor to a Boot
                Cursor.SetCursor(cursorBoot, Vector2.zero, CursorMode.ForceSoftware);   

                //endTurn();
            }
            if (Input.GetKeyDown(KeyCode.Alpha4) && finished == false) //Back out of action choice
            {
                attackMode = false;
                gunMode = false; //Indicates gun action
                grenadeMode = false; //Indicates grenade action
                //Add code to hide any visuals related to each action
                currSprite.material = idle;
                Cursor.SetCursor(cursorArrow, Vector2.zero, CursorMode.ForceSoftware);

                print("4 was pressed, choice reversed, choose your action");


            }
            if (Input.GetKeyDown(KeyCode.T) && finished == false) //Force Terminate turn
            {
                print("Turn Passed By Choice");
                Cursor.SetCursor(cursorArrow, Vector2.zero, CursorMode.ForceSoftware);
                endTurn();
            }

            if (gunMode == true)
            {
                //Change cursor to a Crosshair
                Cursor.SetCursor(cursorSniper, Vector2.zero, CursorMode.ForceSoftware);
                //Sprite swap to gun sprite
                currSprite.material = gunStance;

                //Shoot Action
                if (Input.GetMouseButtonDown(0))
                {
                    
                    //Calculate angle between mouse and the firing point 
                    //to determine the bullet trajectory.
                    Vector3 difference;
                    mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    difference = (mousePos - transform.position);
                    difference.Normalize();
                    angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg - 90f;

                    print("Bang bang");

                    //code to aim gun
                    Instantiate(bulletPrefab, gameObject.transform.position + new Vector3(0, 3, 0), Quaternion.Euler(0, 0, angle));
                    bullet = GameObject.Find("Bullet(Clone)");

                    bullet.transform.position = bulletPoint;

                    endTurn();
                }
            }
            else if (grenadeMode == true)
            {

            }
            else if (bootMode == true)
            {
                
            }


        }      

    }
    
    private void FixedUpdate()
    {
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && moveMode == true)
        {
            //currSprite = walk1;
            //Invoke("walkCycle", .5f);

        }
        if ((Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D)) && moveMode == true)
        {
            //currSprite = idle;
        }

    }
   
    void walkCycle()
    {
        transform.Find("Quad").GetComponent<MeshRenderer>().material = walk2;
    }

    public void beginTurn()
    {
        //Initialize turn for player.
        initialPos = transform.position; //Get new position to base movement distance.
        finished = false; //Indicates finished action phase for this player.
        moveMode = true;
        attackMode = false;
        gunMode = false; //Indicates gun action
        grenadeMode = false; //Indicates grenade action
        bootMode = false; //Indicates boot action     

    }
    void endTurn()
    {
        //End player's turn and resets modes except for movement.
        //Movemode will be activated in game manager
        //Cursor is also reset to default.
        Cursor.SetCursor(cursorArrow, Vector2.zero, CursorMode.ForceSoftware);
        moveMode = false;
        attackMode = false;
        gunMode = false; //Indicates gun action
        grenadeMode = false; //Indicates grenade action
        bootMode = false; //Indicates boot action
        currSprite.material = idle; //Reset sprite to idle sprite.

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