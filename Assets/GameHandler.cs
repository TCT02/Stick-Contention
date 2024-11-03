using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    //Prefab placeholders for characters
    public GameObject blueStick;
    public GameObject redStick;

    //Dedicated reference to each player's character
    public GameObject playerOne;
    public GameObject playerTwo;

    //The spawn locations of the two players
    public GameObject blueSpawn;
    public GameObject redSpawn;

    //Game status checks
    public bool gameStarted;
    public bool p1Active;
    public bool p2Active;
    public bool restarting;

    //PlayerLists
    ArrayList redList = new ArrayList();
    ArrayList blueList = new ArrayList();
    ArrayList playerList = new ArrayList();

    void Start()
    {
        //Invoke("spawnPlayers", 1);
        restarting = false;
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
            // print("Some Visual for GAME START");

            //If one of the two team lists are empty, the opposing team wins
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

            
            if (playerOne.GetComponent<Action>().finished == true)
            { //If player 1 has finished their turn, disable them and enable player 2

                print("Player 2's Turn has begun");
                playerOne.GetComponent<Action>().enabled = false;

                ////Turn on P2
                if (p1Active == true)
                {
                    p1Active = false;
                    playerTwo.GetComponent<Action>().enabled = true;
                    playerTwo.GetComponent<Action>().beginTurn();
                    p2Active = true;
                }           

            }
            if (playerTwo.GetComponent<Action>().finished == true)
            { //If player 2 has finished their turn, disable them and enable player 1

                playerTwo.GetComponent<Action>().enabled = false;               
                print("Player 1's Turn has begun");
                if (p2Active == true)
                {
                    //Turn on P1
                    p2Active = false;
                    playerOne.GetComponent<Action>().enabled = true;
                    playerOne.GetComponent<Action>().beginTurn();
                    p1Active = true;
                }

            }         

        }
        else if (gameStarted == false && restarting == false) 
        {
            //Code to reset the map for replay 
            restarting = true;
            Invoke("resetMap", 3);
        }

    }
    void resetMap()
    { //Reloads the scene that holds the map.
        SceneManager.LoadScene("Map1");
    }

    //Code something to pass control between players. (Disable the script of the player not acting)
    //When the player status finished == true, disable their script, enable the other.

    void spawnPlayers()
    {
        //Spawns the prefab blue stick figure from assets on the blue spawn point
        Instantiate(blueStick, blueSpawn.transform.position + new Vector3(0, 3, 0), Quaternion.Euler(0, 0, 0));

        //Add playerOne to the blue player list
        blueList.Add(playerOne);
        playerList.Add(playerOne);

        //Assign player with the reference to their prefab that was spawned on the map
        playerOne = GameObject.Find("BlueStickFigure(Clone)");

        //Spawns the prefab red stick figure from assets on the red spawn point
        Instantiate(redStick, redSpawn.transform.position + new Vector3(0, 3, 0), Quaternion.Euler(0, 0, 0));
   
        //Add playerTwo to the red player list
        redList.Add(playerTwo);
        playerList.Add(playerOne);

        p1Active = true;
        p2Active = false;

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
