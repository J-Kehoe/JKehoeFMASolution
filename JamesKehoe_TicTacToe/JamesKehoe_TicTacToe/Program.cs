using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JamesKehoe_TicTacToe
{
    class Program
    {

        //Establish the size of the board
        const int BOARD_WIDTH = 3, BOARD_HEIGHT = 3;

        //Initialise each player as a "Player" type
        static Player p1 = new Player("Player 1", "X");
        static Player p2 = new Player("Player 2", "O");

        //Set the current player to Player 1
        static Player currentPlayer = p1;

        //Set up 2d array for displaying game board
        static string[,] myBoard = new string[BOARD_WIDTH, BOARD_HEIGHT];

        
        static void Main(string[] args) {
        //Main function initialises the board, displays it on the screen, and prompts the first player to make a move
        //PlacePiece executes recursively until the game is complete

            Console.WriteLine("Welcome to Tic Tac Toe!\nHere's the current board:");
            InitBoard();
            WriteBoard();
            PlacePiece();

        }

        static void InitBoard() {
        //initialises the game board for a new game - each game piece is set to the default "."

            for (int i = 0; i < BOARD_HEIGHT; i++) {
                for (int j = 0; j < BOARD_WIDTH; j++) {
                    myBoard[j, i] = ".";
                }
            }
        }

        static void WriteBoard() {
        //Displays the current state of the game board

            Console.WriteLine("\n  1 2 3");

            for (int i = 0; i < BOARD_HEIGHT; i++) {
                Console.Write((i+1) + " ");
                for (int j = 0; j < BOARD_WIDTH; j++) {
                    Console.Write(myBoard[j, i] + " ");
                }
                Console.WriteLine("");
            }
            Console.WriteLine("");
        }

        static void SwapPlayer() {
        //Changes the Current Player

            if (currentPlayer == p1) {
                currentPlayer = p2;
            } else if (currentPlayer == p2) {
                currentPlayer = p1;
            }
        }

        static void PlacePiece() {
        //This function holds most of the game logic - it takes in an input from the active player and checks if it's a valid move.
        //If the move is valid, it checks if it is a winning move - if so the game ends. 
        //If not, the active player is swapped and PlayPiece is called again.

            //Prompt the current player to make a move
            Console.Write(currentPlayer.getName() + " enter a coord x,y to place your " + currentPlayer.getToken() + " or enter 'q' to give up: ");

            //Save the input as a variable
            string coords = Console.ReadLine();

            //Check if the player wants to quit the game,
            if (coords == "q") {
                Console.WriteLine("\nThanks for Playing!");
                Console.ReadKey();
            } else {
                //Make a regular expression to check that the move matches the correct format
                Regex r = new Regex("^(1|2|3),(1|2|3)$");

                if (r.IsMatch(coords)) {
                    //Save the first and third characters of the coords string as x and y coordinates
                    //Subtract one to account for array positioning starting at zero
                    int x = int.Parse(coords[0].ToString()) - 1;
                    int y = int.Parse(coords[2].ToString()) - 1;

                    //Check if the coords are already occupied
                    if (CheckValid(x, y)) {

                        //if the space is available, set the space as the current player's token
                        myBoard[x, y] = currentPlayer.getToken();
                        Console.WriteLine("\nMove accepted, here's the current board:");
                        WriteBoard();

                        // Check if a line has been formed, if so display the winner and close the game
                        if (CheckLine(x, y)) {
                            Console.WriteLine(currentPlayer.getName() + " wins!");
                            Console.ReadKey();
                        } else {
                            //If not, check if all spaces have been filled. 
                            //If they have, display the Draw message and close the game
                            if (CheckDraw()) {
                                Console.WriteLine("A Draw!");
                                Console.ReadKey();
                            //Otherwise, swap to the next player and prompt another move.
                            } else {
                                SwapPlayer();
                                PlacePiece();
                            }
                        }
                    //If the space selected is already occupied, tell the player and prompt another move
                    } else {
                        Console.WriteLine("\nOh no, a piece is already at this place! Try again...\n");
                        PlacePiece();
                    }
                //If the input is not a valid coordinate, tell the user and prompt another move
                } else {
                    Console.WriteLine("\nInvalid input! Please enter a coord in the format of x,y\n");
                    PlacePiece();
                }
            }
        }

        static bool CheckValid(int x, int y) {
        //This function checks if the space selected is already occupied

            //it checks if the given coordinates are already an X or O on the game board.
            //if they are, return false - the space is not valid
            //if not, return true - a piece can be placed here
            if (myBoard[x, y] == "X" || myBoard[x, y] == "O") {
                return false;
            } else {
                return true;
            }
        }

        static bool CheckLine(int x, int y) {
        //This function checks if there is a 3-piece line on the gameboard

            
            bool result = false;

            //Four checks are made, each one testing a different kind of line that could be made once a piece is set.
            //Firstly, it checks if a horizontal line has been made by analysing all pieces on the selected x coord
            //Then, it checks for a vertical line by doing the same with the y coord
            //It then runs a check for each of the two possible diagonal lines
            //If any of these lines contain the same piece in each space, the function will return true.
            if ((myBoard[x, 0] == currentPlayer.getToken() && myBoard[x, 1] == currentPlayer.getToken() && myBoard[x, 2] == currentPlayer.getToken()) ||
                (myBoard[0, y] == currentPlayer.getToken() && myBoard[1, y] == currentPlayer.getToken() && myBoard[2, y] == currentPlayer.getToken()) ||
                (myBoard[0, 0] == currentPlayer.getToken() && myBoard[1, 1] == currentPlayer.getToken() && myBoard[2, 2] == currentPlayer.getToken()) ||
                (myBoard[0, 2] == currentPlayer.getToken() && myBoard[1, 1] == currentPlayer.getToken() && myBoard[2, 0] == currentPlayer.getToken())) {
                result = true;
            }

            //return the result of the check as a boolean
            return result;
        }

        static bool CheckDraw() {
        //This function checks if the game board is full with no winner, resulting in a draw

            //Initialise a counter at zero
            int count = 0;

            //Cycle through the game board, adding to the counter if a space is occupied
            for (int i = 0; i < BOARD_WIDTH; i++) {
                for (int j = 0; j < BOARD_HEIGHT; j++) {
                    if (myBoard[i, j] != ".") {
                        count++;
                    }
                }
            }

            //If all spaces are full after the loop, return true - the board is full
            if (count == 9) {
                return true;
            } else {
                return false;
            }
        }

        private class Player {
        //This class represents the players of the game. Each player has a corresponding name and token.

            string name;
            string token;

            //Constructor function for the class
            public Player(string name, string token) {
                this.name = name;
                this.token = token;
            }

            public string getName() {
            //returns the corresponding name of a player
                return name;
            }

            public string getToken() {
            //returns the corresponding token of a player
                return token;
            }
        }
    }
}
