using System;
using System.Text;

namespace MemoryGame
{
    public class Board
    {
        private Card[,] m_GameBoard;
        private int m_Row;
        private int m_Col;
        private int m_NumOfExposedPairCards;
        private int m_MaxPairCards;

        public Board(int i_Row, int i_Col)
        {
            m_Row = i_Row;
            m_Col = i_Col;
            m_MaxPairCards = (i_Row * i_Col) / 2;
            m_NumOfExposedPairCards = 0;
            m_GameBoard = new Card[i_Row, i_Col];
            createBoardValues();
        }

        public int Row
        {
            get
            {
                return m_Row;
            }
        }

        public int Col
        {
            get
            {
                return m_Col;
            }
        }

        public Card[,] GameBoard
        {
            get
            {
                return m_GameBoard;
            }
        }

        public int NumOfExposedPairCards
        {
            get
            {
                return m_NumOfExposedPairCards;
            }

            set
            {
                m_NumOfExposedPairCards = value;
            }
        }

        public int MaxPairCards
        {
            get
            {
                return m_MaxPairCards;
            }
        }

        public static void ParseIndexCard(string i_Card, out int o_idxRow, out int o_idxCol)
        {
            o_idxRow = i_Card[1] - '1';
            o_idxCol = i_Card[0] - 'A';
        }

        public static string ParseIndexCard(int i_idxRow, int i_idxCol)
        {
            StringBuilder indexOfCard = new StringBuilder();
            indexOfCard.Append((char)((int)'A' + i_idxCol));
            indexOfCard.Append((char)((int)'1' + i_idxRow));
            return indexOfCard.ToString();
        }

        public static bool IsAMatchFound(Board i_Board, string i_FirstCardIndex, string i_SecondCardIndex)
        {
            int idxRowFirstCard, idxColFirstCard;
            int idxRowSecondCard, idxColSecondCard;
            ParseIndexCard(i_FirstCardIndex, out idxRowFirstCard, out idxColFirstCard);
            ParseIndexCard(i_SecondCardIndex, out idxRowSecondCard, out idxColSecondCard);
            Card firstCard = i_Board.GameBoard[idxRowFirstCard, idxColFirstCard];
            Card SecondCard = i_Board.GameBoard[idxRowSecondCard, idxColSecondCard];
            return Card.IsTheCardEqual(firstCard, SecondCard);
        }

        public static int GetCardKey(Board i_Board, string i_CardIndex)
        {
            int RowIndex, ColIndex;
            Board.ParseIndexCard(i_CardIndex, out RowIndex, out ColIndex);
            return i_Board.GameBoard[RowIndex, ColIndex].Key.Value;
        }

        private void createBoardValues()
        {
            char val = 'A';
            for (int key = 1; key <= m_MaxPairCards; key++)
            {
                chooseRandomPlaceAndPutValAndKey(key, val);
                chooseRandomPlaceAndPutValAndKey(key, val);
                val++;
            }
        }

        private void chooseRandomPlaceAndPutValAndKey(int i_Key, char i_Val)
        {
            Random rnd = new Random();
            int idxRow = 0;
            int idxCol = 0;
            do
            {
                idxRow = rnd.Next(m_Row);
                idxCol = rnd.Next(m_Col);
            }
            while (m_GameBoard[idxRow, idxCol].Key.HasValue);
            m_GameBoard[idxRow, idxCol].Value = i_Val;
            m_GameBoard[idxRow, idxCol].Key = i_Key;
        }

        public void ExposedCard(string i_IndexCard)
        {
            int RowIndex, ColIndex;
            ParseIndexCard(i_IndexCard, out RowIndex, out ColIndex);
            ExposedCard(RowIndex, ColIndex);
        }

        public void ExposedCard(int i_RowIndex, int i_ColIndex)
        {
            m_GameBoard[i_RowIndex, i_ColIndex].IsExposed = true;
        }

        public void HideCard(string i_IndexCard)
        {
            int RowIndex, ColIndex;
            ParseIndexCard(i_IndexCard, out RowIndex, out ColIndex);
            HideCard(RowIndex, ColIndex);
        }

        public void HideCard(int i_RowIndex, int i_ColIndex)
        {
            m_GameBoard[i_RowIndex, i_ColIndex].IsExposed = false;
        }

        public bool IsCardExposed(string i_IndexCard)
        {
            int idxRow, idxCol;
            Board.ParseIndexCard(i_IndexCard, out idxRow, out idxCol);
            return IsCardExposed(idxRow, idxCol);
        }

        public bool IsCardExposed(int i_RowIndex, int i_ColIndex)
        {
            return m_GameBoard[i_RowIndex, i_ColIndex].IsExposed;
        }
    }
}
