using System;


namespace MemoryGame
{
    public class UI
    {
        private const string k_WrongMsg = "Please Try Again.";

        public static void RunGame()
        {
            Console.WriteLine(" ======== Welcome to Our Memory Game ======== ");
            int width, lenght;
            bool isUserOneHuman = true, isUserTwoHuman = false;
            string userName1 = GetUserName();
            string userName2 = null;
            MemoryGame.eGameType gameType = ChooseAGameType();
            if (gameType == MemoryGame.eGameType.AgainstAnotherUser)
            {
                userName2 = GetUserName();
                isUserTwoHuman = true;
            }

            GetBoardSize(out width, out lenght);
            MemoryGame game = new MemoryGame(userName1, userName2, isUserOneHuman, isUserTwoHuman, gameType, lenght, width);
            PlayGame(game);

            bool anotherGame = isTostartAnotherGame();
            if (anotherGame)
            {
                Ex02.ConsoleUtils.Screen.Clear();
                RunGame();
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Thank You and have a nice day :)");
                System.Threading.Thread.Sleep(3000);
            }
        }

        private static void PlayGame(MemoryGame i_Game)
        {
            while (!i_Game.IsTheGameOver())
            {
                gameRound(i_Game);
                if (i_Game.Turn == MemoryGame.eGameTurn.FirstUser)
                {
                    i_Game.Turn = MemoryGame.eGameTurn.SecondUser;
                }
                else
                {
                    i_Game.Turn = MemoryGame.eGameTurn.FirstUser;
                }
            }

            User Winner = i_Game.DetermineWinner();
            if (Winner == null)
            {
                PrintEqualPoints(i_Game.Board.MaxPairCards / 2, i_Game.Board);
            }
            else
            {
                PrintWinner(Winner.Name, Winner.Points, i_Game.Board);
            }
        }

        private static void gameRound(MemoryGame i_Game)
        {
            bool isUserFindMatch;
            if (i_Game.Turn == MemoryGame.eGameTurn.FirstUser)
            {
                userTurn(i_Game, i_Game.PlayerOne, out isUserFindMatch);
            }
            else
            {   // second user - Human Player or Computer Player
                if (i_Game.Type == MemoryGame.eGameType.AgainstAnotherUser)
                {
                    userTurn(i_Game, i_Game.PlayerTwo, out isUserFindMatch);
                }
                else
                {
                    computerTurn(i_Game, i_Game.PlayerTwo, out isUserFindMatch);
                }
            }

            if (isUserFindMatch && !i_Game.IsTheGameOver())
            {
                gameRound(i_Game);
            }
        }

        private static void computerTurn(MemoryGame i_Game, User i_Player, out bool o_IsAMacth)
        {
            string firstCardIndex, secondCardIndex;

            Ex02.ConsoleUtils.Screen.Clear();
            PrintUserDetails(i_Player.Name, i_Player.Points);
            PrintGameBoard(i_Game.Board);
            i_Player.AI.ChooseCardsComputer(i_Game.Board, out firstCardIndex, out secondCardIndex);
            i_Game.ExposedCard(firstCardIndex);
            printComputerSelection(i_Game.Board, firstCardIndex, i_Player);
            i_Game.ExposedCard(secondCardIndex);
            printComputerSelection(i_Game.Board, secondCardIndex, i_Player);
            checkMatchingCardsAndUpdateBoard(i_Game, firstCardIndex, secondCardIndex, out o_IsAMacth);
        }

        private static void userTurn(MemoryGame i_Game, User i_Player, out bool o_IsAMacth)
        {
            string firstCardIndex, secondCardIndex;
            userSelectACard(i_Game, i_Player, out firstCardIndex);
            userSelectACard(i_Game, i_Player, out secondCardIndex);
            checkMatchingCardsAndUpdateBoard(i_Game, firstCardIndex, secondCardIndex, out o_IsAMacth);
        }

        private static void checkMatchingCardsAndUpdateBoard(MemoryGame i_Game, string i_FirstCardIndex, string i_SecondCardIndex, out bool io_IsAMacth)
        {
            io_IsAMacth = i_Game.IsCardsMatch(i_FirstCardIndex, i_SecondCardIndex);
            if (io_IsAMacth)
            {
                PrintGoodMatch();
                i_Game.IncreasePoints();
            }
            else
            {
                PrintBadMatch();
                i_Game.Board.HideCard(i_FirstCardIndex);
                i_Game.Board.HideCard(i_SecondCardIndex);
            }
        }

        private static void userSelectACard(MemoryGame i_Game, User i_Player, out string o_CardIndex)
        {
            Ex02.ConsoleUtils.Screen.Clear();
            PrintUserDetails(i_Player.Name, i_Player.Points);
            PrintGameBoard(i_Game.Board);
            ChooseACard(i_Game.Board, out o_CardIndex);
            i_Game.ExposedCard(o_CardIndex);
            printUserSelection(i_Game.Board, i_Player);

            if (i_Game.Type == MemoryGame.eGameType.AgainstTheComputer)
            {
                i_Game.UpdateComputerAI(o_CardIndex);
            }
        }

        // ========== Getting Inputs Functions From User And Priniting Functions ========== // 
        public static string GetUserName()
        {
            string userName = null;
            string msg = string.Format("Please Enter The User Name");
            Console.WriteLine(msg);
            userName = Console.ReadLine();
            while (userName.Length == 0)
            {
                Console.WriteLine("Wrong! you didn't put a name");
                Console.WriteLine(msg);
                userName = Console.ReadLine();
            }

            return userName;
        }

        public static MemoryGame.eGameType ChooseAGameType()
        {
            string msg = string.Format(
@"Please choose a Game Type:
Press 1 - To Play Against Another User
Press 2 - To Play Against The Computer");
            int gameType;

            Console.WriteLine(msg);
            bool isANum = int.TryParse(Console.ReadLine(), out gameType);
            while (!isANum || !Validations.GameTypeRange(gameType))
            {
                Console.WriteLine("Wrong! You should choose 1 or 2. {0}{1}", System.Environment.NewLine, k_WrongMsg);
                isANum = int.TryParse(Console.ReadLine(), out gameType);
            }

            return (MemoryGame.eGameType)gameType;
        }

        private static int getWidthOrLength()
        {
            int sizeBoard;
            bool isAValidNum = int.TryParse(Console.ReadLine(), out sizeBoard);
            while (!isAValidNum)
            {
                Console.WriteLine("Wrong! You should choose number Betwween {0} to {1}{2}{3}", Validations.k_MinBoardSize, Validations.k_MaxBoardSize, System.Environment.NewLine, k_WrongMsg);
                isAValidNum = int.TryParse(Console.ReadLine(), out sizeBoard);
            }

            return sizeBoard;
        }

        public static void GetBoardSize(out int o_Width, out int o_Lenght)
        {
            string msg = "Please choose you board size: min 4X4 and Max 6X6";
            Console.WriteLine(msg);

            do
            {
                Console.Write("Choose Width: ");
                o_Width = getWidthOrLength();
                Console.Write("Choose Length: ");
                o_Lenght = getWidthOrLength();
                if (!Validations.GameBoardSize(o_Lenght, o_Width))
                {
                    Console.WriteLine("Wrong! You Board Size should Min 4X4 and Max 6X6. {0}", k_WrongMsg);
                }

                if (!Validations.GameBoardEven(o_Lenght, o_Width))
                {
                    Console.WriteLine("Wrong! Your Board Size should be even. {0}", k_WrongMsg);
                }
            }
            while (!Validations.GameBoardEven(o_Lenght, o_Width) || !Validations.GameBoardSize(o_Lenght, o_Width));
        }

        public static void ChooseACard(Board i_Board, out string o_IndexCard)
        {
            string msg = "Please Choose A Card by the following exmpale 'A4' Or 'Q' to Quit the game";
            Console.WriteLine(msg);
            string inputStr = null;
            do
            {
                inputStr = Console.ReadLine();
                inputStr = inputStr.Trim();
                if (Validations.ExitKey(inputStr))
                {
                    Console.WriteLine("You decided to quit the game.{0}Exit...", System.Environment.NewLine);
                    System.Threading.Thread.Sleep(2000);
                    System.Environment.Exit(1);
                }
                else if (!Validations.RangeCard(inputStr, i_Board))
                {
                    exceptionsRangeMessages(inputStr);
                }
                else if (i_Board.IsCardExposed(inputStr))
                {
                    Console.WriteLine("Wrong! The Card is already exposed. {0}", k_WrongMsg);
                }
            }
            while (!Validations.RangeCard(inputStr, i_Board) || i_Board.IsCardExposed(inputStr));
            o_IndexCard = inputStr;
        }

        private static void exceptionsRangeMessages(string i_StrToCheck)
        {
            if (i_StrToCheck.Length != 2 || !char.IsUpper(i_StrToCheck[0]) || !char.IsDigit(i_StrToCheck[1]))
            {
                if (i_StrToCheck.Length != 2)
                {
                    Console.WriteLine("Wrong! you should choose Uppercase letter and number. {0}", k_WrongMsg);
                }
                else
                {
                    if (!char.IsUpper(i_StrToCheck[0]))
                    {
                        Console.WriteLine("Wrong! you should choose Uppercase letter. for Exmpale: 'A2'");
                    }
                    else if (!char.IsDigit(i_StrToCheck[1]))
                    {
                        Console.WriteLine("Wrong! you should choose a number. for Exmpale: 'A2'");
                    }

                    Console.WriteLine(k_WrongMsg);
                }
            }
            else
            {
                Console.WriteLine("Wrong! Your choice is out of board range. {0}", k_WrongMsg);
            }
        }

        public static void PrintWinner(string i_UserName, int i_Points, Board i_Board)
        {
            string msg = string.Format("The Winner is {0}, with {1} Points", i_UserName, i_Points);
            Ex02.ConsoleUtils.Screen.Clear();
            PrintGameBoard(i_Board);
            Console.WriteLine(msg);
        }

        public static void PrintEqualPoints(int i_Points, Board i_Board)
        {
            string msg = string.Format("The Game Over with equality of {0} Points.", i_Points);
            Ex02.ConsoleUtils.Screen.Clear();
            PrintGameBoard(i_Board);
            Console.WriteLine(msg);
        }

        public static void PrintGoodMatch()
        {
            Console.WriteLine("---GOOD MATCH!!!---");
            System.Threading.Thread.Sleep(2000);
        }

        public static void PrintBadMatch()
        {
            Console.WriteLine("Sorry... Wrong Match!");
            System.Threading.Thread.Sleep(2000);
        }

        public static void PrintUserDetails(string i_UserName, int i_Points)
        {
            Console.WriteLine("This is {0}'s turn: ", i_UserName);
            Console.WriteLine("{0} has {1} points", i_UserName, i_Points);
        }

        public static void PrintGameBoard(Board i_Board)
        {
            string dividerLine = new string('=', i_Board.Col * 6);
            int numOfRow = 1;

            printColLetters(i_Board.Col);
            for (int row = 0; row <= i_Board.Row * 2; row++)
            {
                if (row % 2 == 0)
                {
                    Console.WriteLine(dividerLine);
                }
                else
                {
                    Console.Write(" {0} |", numOfRow);
                    printARowFromBoard(i_Board, i_Board.Col, numOfRow);
                    numOfRow++;
                }
            }
        }

        private static void printColLetters(int i_Col)
        {
            Console.Write("   ");
            char ColLetter = 'A';
            for (int col = 0; col < i_Col; col++)
            {
                Console.Write("  {0}  ", ColLetter);
                ColLetter++;
            }

            Console.Write(System.Environment.NewLine);
        }

        private static void printARowFromBoard(Board i_Board, int i_Col, int i_Row)
        {
            for (int colIndex = 0; colIndex < i_Col; colIndex++)
            {
                if (i_Board.GameBoard[i_Row - 1, colIndex].IsExposed)
                {
                    Console.Write("  {0} ", i_Board.GameBoard[i_Row - 1, colIndex].Value);
                }
                else
                {
                    Console.Write("    ");
                }

                Console.Write("|");
            }

            Console.Write(System.Environment.NewLine);
        }

        private static void printUserSelection(Board i_Board, User i_Player)
        {
            Ex02.ConsoleUtils.Screen.Clear();
            PrintUserDetails(i_Player.Name, i_Player.Points);
            PrintGameBoard(i_Board);
        }

        private static void printComputerSelection(Board i_Board, string i_CardIndex, User i_Player)
        {
            Ex02.ConsoleUtils.Screen.Clear();
            PrintUserDetails(i_Player.Name, i_Player.Points);
            if (i_CardIndex != null)
            {
                Console.WriteLine();
                Console.WriteLine("Computer Choose Card {0}", i_CardIndex);
            }

            PrintGameBoard(i_Board);
            System.Threading.Thread.Sleep(1000);
        }

        private static bool isTostartAnotherGame()
        {
            string anotherGame = null;
            Console.WriteLine();
            Console.WriteLine("Do You Want To Start Another Game?");
            do
            {
                Console.Write("Press Y or N: ");
                anotherGame = Console.ReadLine();
                if (!Validations.CheckAnotherGame(anotherGame))
                {
                    Console.WriteLine("Wrong Input. {0}", k_WrongMsg);
                }
            }
            while (!Validations.CheckAnotherGame(anotherGame));
            return anotherGame == Validations.k_AnotherGame;
        }
    }
}
