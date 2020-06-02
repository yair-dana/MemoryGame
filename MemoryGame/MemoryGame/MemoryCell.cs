using System;


namespace MemoryGame
{
    public struct MemoryCell
    {
        private int m_Key;
        private string m_FirstCardIndex;
        private string m_SecondCardIndex;

        public MemoryCell(int i_Key)
        {
            m_Key = i_Key;
            m_FirstCardIndex = null;
            m_SecondCardIndex = null;
        }

        public int Key
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

        public string FirstCardIndex
        {
            get
            {
                return m_FirstCardIndex;
            }

            set
            {
                m_FirstCardIndex = value;
            }
        }

        public string SecondCardIndex
        {
            get
            {
                return m_SecondCardIndex;
            }

            set
            {
                m_SecondCardIndex = value;
            }
        }
    }
}
