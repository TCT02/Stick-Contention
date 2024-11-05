using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using TMPro;

[RequireComponent(typeof(AudioSource))]
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
    [SerializeField] GameObject blueMoveBar;
    [SerializeField] GameObject redMoveBar;

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

    //Component objects
    Action p1CompA;
    Action p2CompA;
    StickFigure p1CompSF;
    StickFigure p2CompSF;
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

        p1CompA = playerOne.GetComponent<Action>();
        p2CompA = playerTwo.GetComponent<Action>();
        p1CompSF = playerOne.GetComponent<StickFigure>();
        p2CompSF = playerTwo.GetComponent<StickFigure>();

        //Announces match start
        //Invoke("startingText", 1);
        startingText();

    }

    // Update is called once per frame
    void Update()
    {
        //If the player characters are declared dead by some condition,
        //destroy them and remove them from the arraylist.

        if (playerOne && p1CompSF.isDead == true)
        {
            blueList.RemoveAt(0);
            Destroy(playerOne);
            //print("Number of Blue Players " + blueList.Count);
        }
        if (playerTwo && p2CompSF.isDead == true)
        {
            redList.RemoveAt(0);
            Destroy(playerTwo);
            //print("Number of Red Players " + redList.Count);
        }
        if (gameStarted) //Checks win conditions and other while the game is active
        {
 
                //Determine HP bar size based on each character's (CurrHP/MaxHP) * 10
                //Updates Movement resource bar
                if (p1CompSF.CurrHP >= 0 ) 
                {
                    blueHPBar.transform.localScale = new Vector3((p1CompSF.CurrHP / p1CompSF.MaxHP) * 10, 1, 0.1f);
                    blueHPNum.text = (p1CompSF.CurrHP + "/" + p1CompSF.MaxHP);
                }
                else //Handle negative HP displays
                {
                    blueHPBar.transform.localScale = new Vector3((0 / p1CompSF.MaxHP) * 10, 1, 0.1f);
                    blueHPNum.text = (0 + "/" + p1CompSF.MaxHP);
                }


                //Finds the percent progress of a player's movemeter and converts it into a bar visual. 
                float BProgressRight = (((playerOne.transform.position.x - p1CompA.initialPos.x) / (p1CompA.finalPos.x - p1CompA.initialPos.x)));
                float BProgressLeft = (((playerOne.transform.position.x - p1CompA.initialPos.x) / (p1CompA.finalPosL.x - p1CompA.initialPos.x)));
                if (BProgressRight > BProgressLeft)
                {
                    blueMoveBar.transform.localScale = new Vector3(Mathf.Abs((1 - BProgressRight) * 10), 1, 0.1f);
                }
                else
                {
                    blueMoveBar.transform.localScale = new Vector3(Mathf.Abs((1 - BProgressLeft) * 10), 1, 0.1f);
                }


                //Determine HP bar size based on each character's (CurrHP/MaxHP) * 10
                //Updates Movement resource bar
                if (p1CompSF.CurrHP >= 0)
                {
                    redHPBar.transform.localScale = new Vector3((p2CompSF.CurrHP / p2CompSF.MaxHP) * 10, 1, 0.1f);
                    redHPNum.text = (p2CompSF.CurrHP + "/" + p2CompSF.MaxHP);
                }
                else //Handle negative HP displays
                {
                    redHPBar.transform.localScale = new Vector3((0 / p2CompSF.MaxHP) * 10, 1, 0.1f);
                    redHPNum.text = (0 + "/" + p2CompSF.MaxHP);
                }

                

                //Finds the percent progress of a player's movemeter and converts it into a bar visual. 
                float RProgressRight = (((playerTwo.transform.position.x - p2CompA.initialPos.x) / (p2CompA.finalPos.x - p2CompA.initialPos.x)));
                float RProgressLeft = (((playerTwo.transform.position.x - p2CompA.initialPos.x) / (p2CompA.finalPosL.x - p2CompA.initialPos.x)));
                if (RProgressRight > RProgressLeft)
                {
                    redMoveBar.transform.localScale = new Vector3(Mathf.Abs((1 - RProgressRight) * 10), 1, 0.1f);
                }
                else
                {
                    redMoveBar.transform.localScale = new Vector3(Mathf.Abs((1 - RProgressLeft) * 10), 1, 0.1f);
                }

            

            //If one of the two team lists are empty, the opposing team wins
            if (redList.Count == 0)
            {
                print("Blue Team has won!");
                gameStarted = false;

                //Display text for Blue  winning
                announceBig(blueWinStr, blue);
            }
            else if (blueList.Count == 0)
            {
                print("Red Team has won!");
                gameStarted = false;

                //Display text for Red winning
                announceBig(redWinStr, red);
            }           

            //Passes control to each player
            //When a player ends their turn, the status finished is set to true. Disable their script, enable the other.
            if (p1CompA.finished == true)
            { //If player 1 has finished their turn, disable them and enable player 2

                
                showMPText(false, red);
                showAPText(false, red);
         
                Invoke("givePlrTwoTurn", 2);

            }
            if (p2CompA.finished == true)
            { //If player 2 has finished their turn, disable them and enable player 1

                showMPText(false, blue);
                showAPText(false, blue);

                Invoke("givePlrOneTurn", 2);

            }

            //When moveMode is on and the player's script is active, display the movement UI
            if (p1CompA.moveMode == true && p1CompA.enabled == true)
            {
                showMPText(true, blue);
                showAPText(false, blue);
            }
            else if (p2CompA.moveMode == true && p2CompA.enabled == true)
            {
                showMPText(true, red);
                showAPText(false, red);
            }
            //When moveMode is off and the player's script is active, display the action UI
            else if (p1CompA.moveMode == false && p1CompA.enabled == true)
            {
                showAPText(true, blue);
                showMPText(false, blue);
            }
            else if (p2CompA.moveMode == false && p2CompA.enabled == true)
            {
                showAPText(true, red);
                showMPText(false, red);
            }
            

            


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
    
    void givePlrOneTurn()
    {

        p2CompA.enabled = false;
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
    void givePlrTwoTurn()
    {

        p1CompA.enabled = false;
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

    //Game start text
    void startingText()
    {
        textBoard.text = matchStartStr;
        Invoke("clearTextBoard", 3); //Clears text in 3 seconds
       
        announce(blueTurnStr, blue);

    }
    //Announce any message in a specific color
    void announce(string message, Color color)
    {
        winText.color = color;
        winText.text = message;
        Invoke("clearText", 2);

    }
    void announceBig(string message, Color color)
    {
        textBoard.color = color;
        textBoard.text = message;
        Invoke("clearText", 2);

    }
    //Clear any messages
    void clearText()
    {
        textBoard.text = "";
        winText.text = "";

    }

    void clearTextBoard()
    {
        textBoard.text = "";

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
