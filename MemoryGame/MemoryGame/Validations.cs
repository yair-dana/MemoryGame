using System;


namespace MemoryGame
{
    public class Validations
    {
        internal const int k_MinBoardSize = 4;
        internal const int k_MaxBoardSize = 6;
        internal const string k_ExitKey = "Q";
        internal const char k_FirstColLetter = 'A';
        internal const char k_FirstRowNumber = '1';
        internal const string k_AnotherGame = "Y";
        internal const string k_NoAnotherGame = "N";

        public static bool GameTypeRange(int i_GameType)
        {
            bool res = false;

            foreach (int i in Enum.GetValues(typeof(MemoryGame.eGameType)))
            {
                if (i == i_GameType)
                {
                    res = true;
                    break;
                }
            }

            return res;
        }

        public static bool GameBoardEven(int i_Width, int i_Lenght)
        {
            return (i_Width * i_Lenght) % 2 == 0;
        }

        public static bool GameBoardSize(int i_Width, int i_Lenght)
        {
            return i_Width >= k_MinBoardSize && i_Width <= k_MaxBoardSize
                        && i_Lenght >= k_MinBoardSize && i_Lenght <= k_MaxBoardSize;
        }

        public static bool RangeCard(string i_IndexCard, Board i_Board)
        {
            return checkIfLenEqualToTwo(i_IndexCard) && checkLetterRange(i_IndexCard[0], i_Board)
                                            && checkNumberRange(i_IndexCard[1], i_Board);
        }

        private static bool checkIfLenEqualToTwo(string i_IndexCard)
        {
            return i_IndexCard.Length == 2;
        }

        private static bool checkLetterRange(char i_LetterToCheck, Board i_Board)
        {
            return i_LetterToCheck >= k_FirstColLetter && i_LetterToCheck < k_FirstColLetter + i_Board.Col;
        }

        private static bool checkNumberRange(char i_DigitToCheck, Board i_Board)
        {
            return i_DigitToCheck >= k_FirstRowNumber && i_DigitToCheck < k_FirstRowNumber + i_Board.Row;
        }

        public static bool ExitKey(string i_StrToCheck)
        {
            return i_StrToCheck == k_ExitKey;
        }

        public static bool CheckAnotherGame(string i_StrToCheck)
        {
            return i_StrToCheck == k_AnotherGame || i_StrToCheck == k_NoAnotherGame;
        }
    }
}
