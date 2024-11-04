using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using TMPro;

public class Game : MonoBehaviour
{
    //Prefab placeholders for characters
    [SerializeField] GameObject blueStick;
    [SerializeField] GameObject redStick;

    //Dedicated reference to each player's character
    [SerializeField] GameObject playerOne;
    [SerializeField] GameObject playerTwo;

    //The spawn locations of the two players
    [SerializeField] GameObject blueSpawn;
    [SerializeField] GameObject redSpawn;

    //Game status checks
    [SerializeField] bool gameStarted;
    [SerializeField] bool p1Active;
    [SerializeField] bool p2Active;
    bool restarting;

    //PlayerLists
    ArrayList redList = new ArrayList();
    ArrayList blueList = new ArrayList();
    ArrayList playerList = new ArrayList();

    //Text objects and variables
    [SerializeField] TextMeshPro textBoard; //For announcements like Match Start
    [SerializeField] TextMeshPro winText;
    [SerializeField] TextMeshPro movePText;
    [SerializeField] TextMeshPro moveIText;
    [SerializeField] TextMeshPro actionPText;
    [SerializeField] TextMeshPro actionIText;
    [SerializeField] TextMeshPro blueHPNum;
    [SerializeField] TextMeshPro redHPNum;
    //HP Bar object
    [SerializeField] GameObject blueHPBar;
    [SerializeField] GameObject redHPBar;

    //Turn indicator
    [SerializeField] GameObject turnIndicator;

    //Message presets
    string matchStartStr;
    string blueWinStr;
    string redWinStr;
    string blueTurnStr;
    string redTurnStr;

    //Color presets
    [SerializeField] Color blue; //Default blue team color
    [SerializeField] Color red; //Default red team color

    //Reference to StickFigure Script in each player
    float plrOneHP;
    float plrTwoHP;

    void Start()
    {
        //Initialize
        restarting = false;

        //Find Spawn Locations
        blueSpawn = GameObject.Find("BlueSpawn");
        redSpawn = GameObject.Find("RedSpawn");

        //Initialize text messages
        matchStartStr = "MATCH START, FIGHT";
        blueWinStr = "Blue Team has won!";
        redWinStr = "Red Team has won!";
        blueTurnStr = "Blue Team Phase";
        redTurnStr = "Red Team Phase";

        //Spawns players and starts the match
        spawnPlayers();
        gameStarted = true;

        //Announces match start
        //Invoke("startingText", 1);
        startingText();

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
            //print("Number of Blue Players " + blueList.Count);
        }
        if (playerTwo && playerTwo.GetComponent<StickFigure>().isDead == true)
        {
            redList.RemoveAt(0);
            Destroy(playerTwo);
            //print("Number of Red Players " + redList.Count);
        }
        if (gameStarted) //Checks win conditions while the game is active
        {

            //If one of the two team lists are empty, the opposing team wins
            if (redList.Count == 0)
            {
                print("Blue Team has won!");
                gameStarted = false;

                //Display text for Blue  winning
                announce(blueWinStr, blue);
            }
            else if (blueList.Count == 0)
            {
                print("Red Team has won!");
                gameStarted = false;

                //Display text for Red winning
                announce(redWinStr, red);
            }

            

            //Passes control to each player
            //When a player ends their turn, the status finished is set to true. Disable their script, enable the other.
            if (playerOne.GetComponent<Action>().finished == true)
            { //If player 1 has finished their turn, disable them and enable player 2
          
                playerOne.GetComponent<Action>().enabled = false;
                showMPText(false, red);
                showAPText(false, red);
                ////Turn on P2
                if (p1Active == true)
                {
                    print("Player 2's Turn has begun");

                    //Move an arrow prop or something above a character  to designate turns
                    //Add delay of about 1-3 seconds before turns are handed over.

                    announce(redTurnStr, red);

                    p1Active = false;

                    playerTwo.GetComponent<Action>().enabled = true;
                    playerTwo.GetComponent<Action>().beginTurn();
                    p2Active = true;

                    //Display Instructions for move phase
                    showMPText(true, red);
                }           

            }
            if (playerTwo.GetComponent<Action>().finished == true)
            { //If player 2 has finished their turn, disable them and enable player 1

                playerTwo.GetComponent<Action>().enabled = false;
                showMPText(false, blue);
                showAPText(false, blue);

                if (p2Active == true)
                {
                    print("Player 1's Turn has begun");
                    //Turn on P1
                    announce(blueTurnStr, blue);

                    //Move an arrow prop or something above a character  to designate turns
                    //Add delay of about 1-3 seconds before turns are handed over.

                    p2Active = false;

                    playerOne.GetComponent<Action>().enabled = true;
                    playerOne.GetComponent<Action>().beginTurn();
                    p1Active = true;

                    //Display Instructions for move phase
                    showMPText(true, blue);
                }

            }

            //When moveMode is on and the player's script is active, display the movement UI
            if (playerOne.GetComponent<Action>().moveMode == true && playerOne.GetComponent<Action>().enabled == true)
            {
                showMPText(true, blue);
                showAPText(false, blue);
            }
            else if (playerTwo.GetComponent<Action>().moveMode == true && playerTwo.GetComponent<Action>().enabled == true)
            {
                showMPText(true, red);
                showAPText(false, red);
            }
            //When moveMode is off and the player's script is active, display the action UI
            else if (playerOne.GetComponent<Action>().moveMode == false && playerOne.GetComponent<Action>().enabled == true)
            {
                showAPText(true, blue);
                showMPText(false, blue);
            }
            else if (playerTwo.GetComponent<Action>().moveMode == false && playerTwo.GetComponent<Action>().enabled == true)
            {
                showAPText(true, red);
                showMPText(false, red);
            }
            

            //Determine HP bar size based on each character's (CurrHP/MaxHP) * 10
            blueHPBar.transform.localScale = new Vector3((playerOne.GetComponent<StickFigure>().CurrHP / playerOne.GetComponent<StickFigure>().MaxHP) * 10, 1, 0.1f);
            redHPBar.transform.localScale = new Vector3((playerTwo.GetComponent<StickFigure>().CurrHP / playerTwo.GetComponent<StickFigure>().MaxHP) * 10, 1, 0.1f);

            print((playerOne.GetComponent<StickFigure>().CurrHP / playerOne.GetComponent<StickFigure>().MaxHP) * 10);

            //Set the HP number above the health bars.
            blueHPNum.text = (playerOne.GetComponent<StickFigure>().CurrHP +  "/" + playerOne.GetComponent<StickFigure>().MaxHP);
            redHPNum.text = (playerTwo.GetComponent<StickFigure>().CurrHP + "/" + playerTwo.GetComponent<StickFigure>().MaxHP);

        }
        else if (gameStarted == false && restarting == false) 
        { //When the game has ended via win condition, reload the scene after 3 seconds
          //Code to reset the map for replay 
            showAPText(false, red);
            showMPText(false, red);
            restarting = true;
            Invoke("resetMap", 3);
        }

    }
    void resetMap()
    { //Reloads the scene that holds the map.
        SceneManager.LoadScene("Map1");
    }

    
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

        print("Number of Blue Players " + blueList.Count);
        print("Number of Red Players " + redList.Count);


    }
    
    //Game start text
    void startingText()
    {
        textBoard.text = matchStartStr;
        Invoke("clearText", 3); //Clears text in 3 seconds
       
        announce(blueTurnStr, blue);

    }
    //Announce any message in a specific color
    void announce(string message, Color color)
    {
        winText.color = color;
        winText.text = message;
        Invoke("clearText", 3);

    }
    //Clear any messages
    void clearText()
    {
        textBoard.text = "";
        winText.text = "";

    }

    //Game instructions
    void showMPText(bool TF, Color color)
    {
        movePText.enabled = TF;
        moveIText.enabled = TF;

        movePText.color = color;
        moveIText.color = color;
    }
    void showAPText(bool TF, Color color)
    {
        actionPText.enabled = TF;
        actionIText.enabled = TF;

        actionPText.color = color;
        actionIText.color = color;

    }
    
}
