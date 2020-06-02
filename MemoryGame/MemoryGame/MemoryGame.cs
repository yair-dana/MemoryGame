using System;

namespace MemoryGame
{
    public class MemoryGame
    {
        private User m_UserPlayer1;
        private User m_UserPlayer2;
        private Board m_Board;
        private eGameType m_GameType;
        private eGameTurn m_GameTurn;

        public enum eGameType
        {
            AgainstAnotherUser = 1,
            AgainstTheComputer = 2
        }

        public enum eGameTurn
        {
            FirstUser = 1,
            SecondUser = 2
        }

        public MemoryGame(string i_FirstUserName, string i_SecondUserName, bool i_IsFirstUserHuman, bool i_IsSecondUserHuman, MemoryGame.eGameType i_GameType, int i_Lenght, int i_Width)
        {
            m_GameType = i_GameType;
            m_GameTurn = eGameTurn.FirstUser;
            m_Board = new Board(i_Lenght, i_Width);
            m_UserPlayer1 = new User(i_FirstUserName, i_IsFirstUserHuman, 0);
            m_UserPlayer2 = new User(i_SecondUserName, i_IsSecondUserHuman, m_Board.MaxPairCards / 2);
        }

        public Board Board
        {
            get
            {
                return m_Board;
            }
        }

        public eGameType Type
        {
            get
            {
                return m_GameType;
            }
        }

        public eGameTurn Turn
        {
            get
            {
                return m_GameTurn;
            }

            set
            {
                m_GameTurn = value;
            }
        }

        public User PlayerOne
        {
            get
            {
                return m_UserPlayer1;
            }
        }

        public User PlayerTwo
        {
            get
            {
                return m_UserPlayer2;
            }
        }

        public void ExposedCard(string i_CardIndex)
        {
            m_Board.ExposedCard(i_CardIndex);
        }

        public void HideCard(string i_CardIndex)
        {
            m_Board.HideCard(i_CardIndex);
        }

        public void UpdateComputerAI(string i_CardIndex)
        {
            if (m_UserPlayer2.IsHuman == false)
            {
                int cardKey = Board.GetCardKey(m_Board, i_CardIndex);
                m_UserPlayer2.AI.AddIndexToMemory(cardKey, i_CardIndex);
            }
        }

        public bool IsCardsMatch(string i_FirstCardIndex, string i_SecondCardIndex)
        {
            return Board.IsAMatchFound(m_Board, i_FirstCardIndex, i_SecondCardIndex);
        }

        public bool IsTheGameOver()
        {
            return m_Board.MaxPairCards == m_Board.NumOfExposedPairCards;
        }

        public void IncreasePoints()
        {
            if (m_GameTurn == eGameTurn.FirstUser)
            {
                m_UserPlayer1.Points++;
            }
            else
            {
                m_UserPlayer2.Points++;
            }

            m_Board.NumOfExposedPairCards++;
        }

        public User DetermineWinner()
        {
            if (m_UserPlayer1.Points > m_UserPlayer2.Points)
            {
                return m_UserPlayer1;
            }
            else if (m_UserPlayer1.Points < m_UserPlayer2.Points)
            {
                return m_UserPlayer2;
            }
            else
            {
                return null;
            }
        }
    }
}
