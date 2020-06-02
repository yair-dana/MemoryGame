using System;


namespace MemoryGame
{
    public class User
    {
        private string m_Name;
        private int m_NumOfPoints = 0;
        private bool m_IsHuman;
        private ComputerAI m_AI;

        public User(string i_Name, bool i_IsHuman, int i_MaxKeyToRemebers)
        {
            m_IsHuman = i_IsHuman;
            if (i_IsHuman)
            {
                m_Name = i_Name;
                m_AI = null;
            }
            else
            {
                m_Name = "COMPUTER";
                m_AI = new ComputerAI(i_MaxKeyToRemebers);
            }
        }

        public int Points
        {
            get
            {
                return m_NumOfPoints;
            }

            set
            {
                m_NumOfPoints = value;
            }
        }

        public string Name
        {
            get
            {
                return m_Name;
            }

            set
            {
                m_Name = value;
            }
        }

        public bool IsHuman
        {
            get
            {
                return m_IsHuman;
            }
        }

        public ComputerAI AI
        {
            get
            {
                return m_AI;
            }
        }
    }
}
