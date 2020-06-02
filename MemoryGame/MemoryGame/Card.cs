using System;

namespace MemoryGame
{
    public struct Card
    {
        private int? m_Key;
        private char m_Value;
        private bool m_IsExposed;

        public Card(int i_Key, char i_Val)
        {
            m_Key = i_Key;
            m_Value = i_Val;
            m_IsExposed = false;
        }

        public int? Key
        {
            get
            {
                return m_Key;
            }

            set
            {
                m_Key = value;
            }
        }

        public char Value
        {
            get
            {
                return m_Value;
            }

            set
            {
                m_Value = value;
            }
        }

        public bool IsExposed
        {
            get
            {
                return m_IsExposed;
            }

            set
            {
                m_IsExposed = value;
            }
        }

        public static bool IsTheCardEqual(Card i_FirstCard, Card i_SecondCard)
        {
            return i_FirstCard.Key == i_SecondCard.Key;
        }
    }
}
