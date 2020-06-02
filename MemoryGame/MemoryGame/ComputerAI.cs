using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MemoryGame
{
    public class ComputerAI
    {
        private readonly int r_MaxKeyToRemember;
        private Dictionary<int, MemoryCell> m_Memory;

        public ComputerAI(int i_NumKeysToRemember)
        {
            r_MaxKeyToRemember = i_NumKeysToRemember;
            m_Memory = new Dictionary<int, MemoryCell>();
        }

        public void ChooseCardsComputer(Board i_Board, out string o_IndexOfFirstCard, out string o_IndexOfSecondCard)
        {
            string firstIndex = null, secondIndex = null;
            bool isAPairFound = searchForAnUnexposedMatchInMemory(ref firstIndex, ref secondIndex, i_Board);
            int RowIndex, ColIndex;
            if (!isAPairFound)
            {
                firstIndex = chooseARandomIndex(i_Board, out RowIndex, out ColIndex);
                int cardKey = i_Board.GameBoard[RowIndex, ColIndex].Key.Value;
                AddIndexToMemory(cardKey, firstIndex);
                isAPairFound = searchForIdentityCard(cardKey, firstIndex, ref secondIndex);
            }

            if (!isAPairFound)
            {
                do
                {
                    secondIndex = chooseARandomIndex(i_Board, out RowIndex, out ColIndex);
                }
                while (secondIndex == firstIndex);

                AddIndexToMemory(i_Board.GameBoard[RowIndex, ColIndex].Key.Value, secondIndex);
            }

            o_IndexOfFirstCard = firstIndex;
            o_IndexOfSecondCard = secondIndex;
        }

        public void AddIndexToMemory(int i_Key, string i_IndexOfCardInBoard)
        {
            if (!m_Memory.ContainsKey(i_Key))
            {
                if (m_Memory.Count == r_MaxKeyToRemember)
                {
                    m_Memory.Remove(m_Memory.Keys.First());
                }

                MemoryCell newCell = new MemoryCell(i_Key);
                newCell.FirstCardIndex = i_IndexOfCardInBoard;
                m_Memory.Add(i_Key, newCell);
            }
            else if (m_Memory[i_Key].FirstCardIndex != i_IndexOfCardInBoard)
            {
                MemoryCell updateCell = m_Memory[i_Key];
                updateCell.SecondCardIndex = i_IndexOfCardInBoard;
                m_Memory[i_Key] = updateCell;
            }
        }

        private string chooseARandomIndex(Board i_Board, out int o_RowIndex, out int o_ColIndex)
        {
            Random rnd = new Random();
            do
            {
                o_RowIndex = rnd.Next(i_Board.Row);
                o_ColIndex = rnd.Next(i_Board.Col);
            }
            while (i_Board.IsCardExposed(o_RowIndex, o_ColIndex));

            return Board.ParseIndexCard(o_RowIndex, o_ColIndex);
        }

        private bool searchForIdentityCard(int i_Key, string i_CardIndex, ref string o_IdentitiyCardIndex)
        {
            bool res = false;
            if (m_Memory.ContainsKey(i_Key))
            {
                MemoryCell cellToCheck = m_Memory[i_Key];
                if (cellToCheck.FirstCardIndex == i_CardIndex && cellToCheck.SecondCardIndex != null)
                {
                    o_IdentitiyCardIndex = cellToCheck.SecondCardIndex;
                    res = true;
                }
                else if (cellToCheck.SecondCardIndex == i_CardIndex && cellToCheck.FirstCardIndex != null)
                {
                    o_IdentitiyCardIndex = cellToCheck.FirstCardIndex;
                    res = true;
                }
                else
                {
                    o_IdentitiyCardIndex = null;
                }
            }

            return res;
        }

        private bool searchForAnUnexposedMatchInMemory(ref string io_FirstCardIndex, ref string io_SecondCardIndex, Board i_Board)
        {
            bool res = false;
            Dictionary<int, MemoryCell>.ValueCollection memoryCells = m_Memory.Values;
            foreach (MemoryCell memoryToSeach in memoryCells)
            {
                if (memoryToSeach.FirstCardIndex != null & memoryToSeach.SecondCardIndex != null)
                {
                    if (!i_Board.IsCardExposed(memoryToSeach.FirstCardIndex) && !i_Board.IsCardExposed(memoryToSeach.SecondCardIndex))
                    {
                        io_FirstCardIndex = memoryToSeach.FirstCardIndex;
                        io_SecondCardIndex = memoryToSeach.SecondCardIndex;
                        res = true;
                        break;
                    }
                }
            }

            return res;
        }
    }
}
