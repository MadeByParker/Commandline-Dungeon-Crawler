
using System;
using System.Collections.Generic;
using System.IO;

namespace Crawler
{
    /**
     * The main class of the Dungeon Crawler Application
     * 
     * You may add to your project other classes which are referenced.
     * Complete the templated methods and fill in your code where it says "Your code here".
     * Do not rename methods or variables which already exist or change the method parameters.
     * You can do some checks if your project still aligns with the spec by running the tests in UnitTest1
     * 
     * For Questions do contact us!
     */
    public class CMDCrawler
    {
        /**
         * use the following to store and control the next movement of the yser
         */
        public enum PlayerActions { NOTHING, NORTH, EAST, SOUTH, WEST, PICKUP, ATTACK, QUIT };//an enum to hold all actions
        private PlayerActions action = PlayerActions.NOTHING; //action which we set to as we do each key press
        private bool inputType = false; //to see if the input method should be readline or key
        char[][] map = new char[0][];//default jagged array for the map
        private char[][] playableMap; //a jagged array to store current map
        private string mapPath = @"../../../maps/"; //string variable to store map path
        private bool mapSelected = false; //bool to see if a map has been selected
        private bool GameRun = false; //to see if game is running
        private int goldCounter = 0; //integer to store how much gold we collect
        private int monsterCounter = 0;//integer to store how many monsters are there on the map
        private int monsterHealth = 10; //integer for monster's health
        private float playerHealth = 20.0f; //float integer for player's health 
        private string playerAction = string.Empty; //it converts the enum action into a string which displays on the cmd what action the user is performing
        private bool gameReplay = false;//this is the bool for if the player wins it triggers the replay feature
        private bool playerDead = false;//this is the bool for if the player dies in the game it triggers the replay feature
        /**
         * tracks if the game is running
         */
        private bool active = true;

        /**
         * Reads user input from the Console
         * 
         * Please use and implement this method to read the user input.
         * 
         * Return the input as string to be further processed
         * 
         */


        private string ReadUserInput()
        {
            //your code here
            string input = string.Empty; //input is empty

            if (inputType) //if its true then we should input characters for movement
            {
                input = Console.ReadKey().KeyChar.ToString(); //readkey accepts next character pressed and then we convert it to a string and then return it
                return input;
            }
            else//else do this 2 part select map process
            {
                if (gameReplay == true)//if the replay bool is true then ask the user if they want to play or not, after winning
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Woohoo! You Won!\n\nDo you want to play again?\n\nType Yes to start again or type No to quit the game!");
                    input = Console.ReadLine();
                    return input.Trim();
                }
                if (playerDead == true)//if the player dies it set the bool to true and asks if the user if they want to continue
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ouch, you died!\n\nDo you want to play again?\n\nType Yes to start again or type No to quit the game!");// then it asks if they want to play again
                    input = Console.ReadLine();
                    return input.Trim();
                }
                if (!mapSelected)//first ask what map the user wants to play if a map is not selected
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("What map do you want to play? \n\nType 'load Simple.map' to load the Simple Map\n\nOr type 'load Advanced.map' to load the Advanced Map\n\n");
                    input = Console.ReadLine();//reads the user's input on a line
                    return input.Trim();//returns the trimmed version in case there was a mistyped space at the end
                }
                else//if a map has been selected and loaded then the user has to type play to start playing, reads the line of user input and return it to the console
                {
                    Console.ForegroundColor = ConsoleColor.Gray;//write a preview of the map to the user 
                    for (int lines = 0; lines < map.Length; lines++)
                    {
                        Console.WriteLine(new string(map[lines]));
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Map Loaded...\nType 'play' to Start \n\n");
                    input = Console.ReadLine();
                    return input.Trim();
                }
            }
        }

        /**
         * Processed the user input string
         * 
         * takes apart the user input and does control the information flow
         *  * initializes the map ( you must call InitializeMap)
         *  * starts the game when user types in Play
         *  * sets the correct playeraction which you will use in the GameLoop
         */
        public void ProcessUserInput(string input)
        {
            //your code here

            try//try matching user input to these else throw an ioexception
            {
                switch (input)//switch statement checking what the user has entered
                {
                    case "load Simple.map"://if the user wants to load the simple map it will clear the console and load the simple map 
                        if (!GameRun) //if the game is not running then load the simple map
                        {
                            InitializeMap("Simple.map");
                        }
                        break;
                    case "load Advanced.map": //case where if the user wants to load the advanced mapand load the advanced map

                        if (!GameRun) //if the game is not running then load the advanced map
                        {
                            InitializeMap("Advanced.map");
                        }
                        break;
                    case "play": //case where if the user types in play then it will check if there's a map selected and if the game is not running then it will run the game and change the input tpye to characters only
                        if (mapSelected && !GameRun)
                        {
                            GameRun = true;
                            inputType = true;

                        }
                        else if (GameRun == true)//if its already true then the action is nothing
                        {
                            action = PlayerActions.NOTHING;
                        }
                        break;
                    case "W": if (GameRun) action = PlayerActions.NORTH; break;//if the user presses w key then set action to when it should move north
                    case "A": if (GameRun) action = PlayerActions.WEST; break;//if the user presses a key then set action to when it should move west
                    case "S": if (GameRun) action = PlayerActions.SOUTH; break;//if the user presses s key then set action to when it should move south
                    case "D": if (GameRun) action = PlayerActions.EAST; break;//if the user presses d key then set action to when it should move east
                    case "E": if (GameRun) action = PlayerActions.PICKUP; break;//if the user presses e key then set action to when it should move pickup the gold
                    case "QUIT": if (GameRun) action = PlayerActions.QUIT; break;//if the user types quit then set action to when it should move quit the game instantly
                    case "Q": if (GameRun) action = PlayerActions.QUIT; break;//if the user presses q key then set action to when it should move quit the game instantly
                    case " ": if (GameRun) action = PlayerActions.ATTACK; break;//if the user presses the spacebar then set action to when it should attack the monsters
                    case "Yes"://if the user types yes to play again resets the replay and dead bool variables
                        gameReplay = false;
                        playerDead = false;
                        break;
                    case "No": active = false; break;
                    default: //any other key will present this error message 
                        Console.WriteLine("Oops, try again!\n\n");
                        action = PlayerActions.NOTHING;
                        break;
                }
            }
            catch (IOException)//catches an IOException if something goes wrong
            {

            }
        }

        void PlayAgain() //function to see if the user wants to play again or not, used when the player dies or gets to the exit
        {
            playerAction = "Attacked by Monster";// feedback saying you're being attacked by the monster
            playerHealth -= 2.5f; //part of the monster attack, each time the user passes or walks into a monster, they take damage
            if (playerHealth == 0) //then if the player's health reaches 0 then the game ends, so it resets everything 
            {
                playerDead = true;//tells the program that the player is dead
                mapSelected = false; //unselects the map
                GameRun = false; //stops the game loop
                inputType = false; //converts input type from character to line                        
                monsterCounter = 0;
                playerHealth = 20.0f;//resets player health
                monsterHealth = 0;//resets monster's health
                goldCounter = 0; //resets the gold counter
            }
            if (gameReplay == true)//same here if the player hits the exit
            {
                mapSelected = false;
                GameRun = false;
                inputType = false; //converts input type from character to line      
                monsterCounter = 0;
                playerHealth = 20.0f;
                monsterHealth = 10;
                goldCounter = 0;
            }
        }
        /**
         * The Main Game Loop. 
         * It updates the game state.
         * 
         * This is the method where you implement your game logic and alter the state of the map/game
         * use playeraction to determine how the character should move/act
         * the input should tell the loop if the game is active and the state should advance
         */
        public void GameLoop(bool active)
        {
            // Your code here

            if (GameRun)//if the game is running try to clear the console every frame else catch an IO exception
            {
                try
                {
                    Console.Clear();
                }
                catch (IOException)
                {

                }
            }

            if (GameRun)//if the game is running then it should do all of this
            {
                int[] playerpos = GetPlayerPosition(); //gets the player position from GetPlayerPosition
                int xpos = playerpos[0]; //first number is the player's x coordinate
                int ypos = playerpos[1]; //second integer is the player's y coordinate
                int action = GetPlayerAction();//gets the int of action from GetPlayerAction

                if (playableMap[ypos - 1][xpos] == 'M')//when the monster is one above the player it will do this
                {
                    PlayAgain();//triggers the monster attack function
                    playerAction = "Attacked by Monster";
                    if (action == 6)//if the user presses the spacebar then it will attack the monster
                    {
                        playerAction = "Attacking Monster!";//converts the action enum to string so it displays in the console
                        monsterHealth -= 5;//each time player attacks the monster it loses 5 health
                        if (monsterHealth == 0) //if the monster's health is 0 
                        {
                            monsterCounter--;//monster count decreases by 1
                            playerHealth += 2.5f; //players regenerate health to recover for the next fight as a reward
                            playerAction = "Killed Monster!";//it will display this message once defeated the monster
                            playableMap[ypos - 1][xpos] = '.';//then it'll replace it with a period
                            monsterHealth = 10;//resets the monster health if there's more than one monster
                        }
                    }
                }

                if (playableMap[ypos + 1][xpos] == 'M')//same for when the monster is below the player
                {
                    PlayAgain();
                    playerAction = "Attacked by Monster";
                    if (action == 6)
                    {
                        playerAction = "Attacking Monster!";
                        monsterHealth -= 5;
                        if (monsterHealth == 0)
                        {
                            monsterCounter--;
                            playerHealth += 2.5f;
                            playerAction = "Killed Monster!";
                            playableMap[ypos + 1][xpos] = '.';
                            monsterHealth = 10;
                        }
                    }
                }

                if (playableMap[ypos][xpos + 1] == 'M')//same for when the monster is right of the player
                {
                    PlayAgain();
                    playerAction = "Attacked by Monster";
                    if (action == 6)
                    {
                        playerAction = "Attacking Monster!";
                        monsterHealth -= 5;
                        if (monsterHealth == 0)
                        {
                            monsterCounter--;
                            playerHealth += 2.5f;
                            playerAction = "Killed Monster!";
                            playableMap[ypos][xpos + 1] = '.';
                            monsterHealth = 10;
                        }
                    }
                }
                if (playableMap[ypos][xpos - 1] == 'M')//same for when the monster is left of the player
                {
                    PlayAgain();
                    playerAction = "Attacked by Monster";
                    if (action == 6)
                    {
                        playerAction = "Attacking Monster!";
                        monsterHealth -= 5;
                        if (monsterHealth == 0)
                        {
                            monsterCounter--;
                            playerHealth += 2.5f;
                            playerAction = "Killed Monster!";
                            playableMap[ypos][xpos - 1] = '.';
                            monsterHealth = 10;
                        }
                    }
                }



                if (action == 1)//when the user presses the w key
                {
                    playerAction = "North";//it will say you moving up

                    if(playableMap[ypos - 1][xpos] == 'G')//if they walk over the 'G' then their gold counter increases
                    {
                        goldCounter++;
                    }
                    
                    
                    if (playableMap[ypos - 1][xpos] == 'E')//if the next tile is the exit tile then the game will end
                    {
                        playableMap[ypos][xpos] = '.';
                        playableMap[ypos - 1][xpos] = '@';
                        GameRun = false;
                        gameReplay = true;
                        PlayAgain();//recalls the ending function and proceeds as the condition is true as action is the quitting action
                    }

                    if (playableMap[ypos - 1][xpos] != '#' && playableMap[ypos - 1][xpos] != 'M' && playableMap[ypos - 1][xpos] != 'E')//when the next tile is not a border,gold or a monster 
                    //then the player moves up and replaces the tile the player was on with a period 
                    {
                        playableMap[ypos][xpos] = '.';
                        playableMap[ypos - 1][xpos] = '@';
                    }
                    else
                    {
                        playableMap[ypos][xpos] = '@';
                        action = 0;
                    }
                }

                if (action == 3)//same for when moving down
                {
                    playerAction = "South";

                    if (playableMap[ypos + 1][xpos] == 'G')
                    {
                        goldCounter++;
                    }

                    if (playableMap[ypos + 1][xpos] == 'E')
                    {
                        playableMap[ypos][xpos] = '.';
                        playableMap[ypos - 1][xpos] = '@';
                        GameRun = false;
                        gameReplay = true;
                        PlayAgain();
                    }
                    if (playableMap[ypos + 1][xpos] != '#' && playableMap[ypos + 1][xpos] != 'M' && playableMap[ypos + 1][xpos] != 'E')
                    {

                        playableMap[ypos][xpos] = '.';
                        playableMap[ypos + 1][xpos] = '@';
                    }
                    else
                    {
                        playableMap[ypos][xpos] = '@';
                        action = 0;
                    }
                }


                if (action == 2)//same for when moving right
                {
                    playerAction = "East";

                    if (playableMap[ypos][xpos + 1] == 'G')
                    {
                        goldCounter++;
                    }

                    if (playableMap[ypos][xpos + 1] == 'E')
                    {
                        playableMap[ypos][xpos] = '.';
                        playableMap[ypos - 1][xpos] = '@';
                        gameReplay = true;
                        GameRun = false;
                        PlayAgain();
                    }

                    if (playableMap[ypos][xpos + 1] != '#' && playableMap[ypos][xpos + 1] != 'M' && playableMap[ypos][xpos + 1] != 'E')
                    {
                        playableMap[ypos][xpos] = '.';

                        playableMap[ypos][xpos + 1] = '@';
                    }
                    else
                    {
                        playableMap[ypos][xpos] = '@';
                        action = 0;
                    }
                }

                if (action == 4)//and finally for when moving left
                {
                    playerAction = "West";

                    if (playableMap[ypos][xpos - 1] == 'G')
                    {
                        goldCounter++;
                    }

                    if (playableMap[ypos][xpos - 1] == 'E')
                    {
                        playableMap[ypos][xpos] = '.';
                        playableMap[ypos - 1][xpos] = '@';
                        gameReplay = true;
                        GameRun = false;
                        PlayAgain();
                    }

                    if (playableMap[ypos][xpos - 1] != '#' && playableMap[ypos][xpos - 1] != 'M' && playableMap[ypos][xpos - 1] != 'E')
                    {
                        playableMap[ypos][xpos] = '.';

                        playableMap[ypos][xpos - 1] = '@';
                    }
                    else
                    {
                        playableMap[ypos][xpos] = '@';
                        action = 0;
                    }
                }

                if (playableMap[ypos - 1][xpos] == 'G')//this is when the place above the player is a piece of gold
                {
                    playerAction = "Press E to Pickup Gold!";//tells them to use the E key to pick the gold up
                    if (action == 5)//when the player presses the e key to pickup gold
                    {
                        playerAction = "Picked up Gold!";//it will tell them that they picked it up
                        playableMap[ypos - 1][xpos] = '.';//replace the tile with a period
                        goldCounter++;//increment the counter for how much gold you found
                    }
                }

                if (playableMap[ypos + 1][xpos] == 'G')//same for when the monster is below the player
                {
                    playerAction = "Press E to Pickup Gold!";
                    if (action == 5)
                    {
                        playerAction = "Picked up Gold!";
                        playableMap[ypos + 1][xpos] = '.';
                        goldCounter++;
                    }
                }

                if (playableMap[ypos][xpos + 1] == 'G')//same for when the monster is right of the player
                {
                    playerAction = "Press E to Pickup Gold!";
                    if (action == 5)
                    {
                        playerAction = "Picked up Gold!";
                        playableMap[ypos][xpos + 1] = '.';
                        goldCounter++;
                    }
                }

                if (playableMap[ypos][xpos - 1] == 'G')//same for when the monster is left of the player
                {
                    playerAction = "Press E to Pickup Gold!";
                    if (action == 5)
                    {
                        playerAction = "Picked up Gold!";
                        playableMap[ypos][xpos - 1] = '.';
                        goldCounter++;
                    }
                }

                Console.WriteLine(Environment.NewLine);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("  Gold: " + goldCounter + "  Action: " + playerAction);//prints the gold counter and player action
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("  Player Health: " + playerHealth);//writes to console the current 
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("  Monster Health: " + monsterHealth + "  Monster Left: " + monsterCounter);//writes to console each monster health and monster count
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("  Controls: W - Up, A - Left, S - Down, D - Right,\n  When next to the 'G' and press E or walk over it to pick up gold! \n  Spacebar - Attack, Q - Quit, \n" +
                        "  Make sure to have CAPS on, Good Luck!\n");//writes to console the controls

                for (int count = 0; count < map.Length; count++) //for each character line in the map write it as a new string line in the cmd each time the map updates
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(new string(playableMap[count]));
                }
            }
        }

        /**
        * Map and GameState get initialized
        * mapName references a file name 
        * 
        * Create a private object variable for storing the map in Crawler and using it in the game.
        */
        public bool InitializeMap(String mapName)
        {
            bool initSuccess = false; //bool for test 

            //Your code here

            try//it will try to load the map
            {
                string[] currentMap = new string[] { };//string array declaration to store each line
                currentMap = System.IO.File.ReadAllLines(mapPath + mapName); //puts the map contents into a string array by reading all lines in the file

                map = new char[currentMap.Length][]; //creates a 2d array for storing the original map into 
                playableMap = new char[currentMap.Length][]; //playableMap is set outside Initialisemap which is a jagged array and it stores the current map for when its overwritten

                for (int i = 0; i < currentMap.Length; i++) //for each line of the array put it into the first index of mapData and then convert it to a char array - array to a 2d array
                {
                    map[i] = currentMap[i].ToCharArray();//this stores the original map
                    playableMap[i] = currentMap[i].ToCharArray();//this stores the playable map
                }
                mapSelected = true;//set map selected to true so the program knows we got a map selected
                initSuccess = true;//sets initsuccess to true
                for (int y = 0; y < map.Length; y++)//for each column and row for the playable map it will search for the M which is the monsters and then increment the monster counter
                {
                    for (int x = 0; x < map[y].Length; x++)
                    {
                        if (playableMap[y][x] == 'M')
                        {
                            monsterCounter++;
                        }
                    }
                }
            }
            catch (IOException)//if there's an IOException error then it will return initsuccess straight away which is false
            {
                initSuccess = false;
                return initSuccess;
            }
            return initSuccess;//otherwise return initSuccess which is true
        }

        /**
         * Returns a representation of the currently loaded map
         * before any move was made.
         */
        public char[][] GetOriginalMap()
        {
            // Your code here

            return map;//return the original map to compare with the updating map
        }

        /*
         * Returns the current map state 
         * without altering it 
         */
        public char[][] GetCurrentMapState()
        {
            // the map should be map[y][x]


            // Your code here

            for (int i = 0; i < playableMap.Length; i++) //for each line for the map length it will print the playable map which gets updated every move and return it
            {
                Console.WriteLine(playableMap);//write each line of the map to the console
            }

            return playableMap;//return the updating map
        }

        /**
         * Returns the current position of the player on the map
         * 
         * The first value is the x corrdinate and the second is the y coordinate on the map
         */
        public int[] GetPlayerPosition()
        {
            int[] position = { 0, 0 };//default starting position

            // Your code here

            for (int y = 0; y < map.Length; y++)//for each column and row for the playable map it will search for the S which is starting point and then return the player icon '@'
            {
                for (int x = 0; x < map[y].Length; x++)
                {
                    if (playableMap[y][x] == '@' || playableMap[y][x] == 'S')
                    {
                        position[0] = x;//sets the player position at the default
                        position[1] = y;
                    }
                }
            }
            return position;//returns the player's position
        }

        /**
        * Returns the next player action
        * 
        * This method does not alter any internal state
        */
        public int GetPlayerAction()
        {
            //Your code here

            int action = (int)this.action;
            return (int)action;//returns the int of each action
        }


        public bool GameIsRunning()
        {
            bool running = false;
            // Your code here 

            if (GameRun == true)//If the game is running set bools active and running to true
            {
                running = true;
                active = true;
            }
            if (active == false)//if the active bool is false set running to false
            {
                running = false;
            }
            return running;//return bool running
        }

        /**
         * Main method and Entry point to the program
         * ####
         * Do not change! 
        */
        static void Main(string[] args)
        {
            CMDCrawler crawler = new CMDCrawler();
            string input = string.Empty;

            Console.WriteLine("Welcome to the Commandline Dungeon!" + Environment.NewLine +
                "May your Quest be filled with riches!" + Environment.NewLine);

            // Loops through the input and determines when the game should quit
            while (crawler.active && crawler.action != PlayerActions.QUIT)
            {
                Console.WriteLine("Your Command: ");
                input = crawler.ReadUserInput();
                Console.WriteLine(Environment.NewLine);

                crawler.ProcessUserInput(input);

                crawler.GameLoop(crawler.active);
            }
            Console.WriteLine("See you again" + Environment.NewLine +
                "In the CMD Dungeon! ");
        }
    }
}