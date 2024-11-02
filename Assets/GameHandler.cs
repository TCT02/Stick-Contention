using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Game : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject blueStick;
    public GameObject redStick;

    public GameObject playerOne;
    public GameObject playerTwo;

    public GameObject blueSpawn;
    public GameObject redSpawn;

    public bool gameStarted;

    ArrayList redList = new ArrayList();
    ArrayList blueList = new ArrayList();
    ArrayList playerList = new ArrayList();

    void Start()
    {
        //Invoke("spawnPlayers", 1);
        blueSpawn = GameObject.Find("BlueSpawn");
        redSpawn = GameObject.Find("RedSpawn");
        spawnPlayers();
 
    }

    // Update is called once per frame
    void Update()
    {
        //If the player characters are declared dead by some condition,
        //destroy them and remove them from the arraylist.

        if (playerOne && playerOne.GetComponent<StickFigure>().isDead == true)
        {
            blueList.RemoveAt(0);
            Destroy(playerOne);
            print("Number of Blue Players " + blueList.Count);
        }
        if (playerTwo && playerTwo.GetComponent<StickFigure>().isDead == true)
        {
            redList.RemoveAt(0);
            Destroy(playerTwo);
            print("Number of Red Players " + redList.Count);
        }
        if (gameStarted)
        {
            // print("checking win condition");

            if (redList.Count == 0)
            {
                print("Blue Team has won!");
                gameStarted = false;
                //Add code to change display text for winning
                //Add code to end and reset the game
            }
            else if (blueList.Count == 0)
            {
                print("Red Team has won!");
                gameStarted = false;
                //Add code to change display text for winning
                //Add code to end and reset the game
            }
        }
    }
    private void FixedUpdate()
    {
        
    }


    //Code something to pass control between players. (Disable the script of the player not acting)
    //When the player status finished == true, disable their script, enable the other.
    private void OnMouseDown()
    //private void OnKeyDown()
    {
        //Enable one player or the other's controls.
        if (playerOne.GetComponent<Action>().enabled)
        {
            playerOne.GetComponent<Action>().enabled = false;

            playerTwo.GetComponent<Action>().enabled = true;
        }      
        else if (playerTwo.GetComponent<Action>().enabled)
        {
            playerTwo.GetComponent<Action>().enabled = false;

            playerOne.GetComponent<Action>().enabled = true;
        }
           
      
    }

    void spawnPlayers()
    {
        //Spawns the prefab blue stick figure from assets on the blue spawn point
        Instantiate(blueStick, blueSpawn.transform.position + new Vector3(0, 3, 0), Quaternion.Euler(0, 0, -180));

        //Add playerOne to the blue player list
        blueList.Add(playerOne);
        playerList.Add(playerOne);

        //Assign player with the reference to their prefab that was spawned on the map
        playerOne = GameObject.Find("BlueStickFigure(Clone)");

        //Spawns the prefab red stick figure from assets on the red spawn point
        Instantiate(redStick, redSpawn.transform.position + new Vector3(0, 3, 0), Quaternion.Euler(0, 0, -180));
   
        //Add playerTwo to the red player list
        redList.Add(playerTwo);
        playerList.Add(playerOne);

        //Assign player with the reference to their prefab that was spawned on the map
        playerTwo = GameObject.Find("RedStickFigure(Clone)");

        //Provide a way for one character to reference the other one in code.
        //playerOne.GetComponent<StickFigure>().otherPlayer = playerTwo;
        //playerTwo.GetComponent<StickFigure>().otherPlayer = playerOne;

        print("Number of Blue Players " + blueList.Count);
        print("Number of Red Players " + redList.Count);

        gameStarted = true;
        print("MATCH START, FIGHT");

    }
    
}
