using System;
namespace Algos
{
    public class TicTacToe
    {

        /** Initialize your data structure here. */
        int[] rowPtr;
        int[] colPtr;
        int fwdDig;
        int revDig;
        int boardSize;
        public TicTacToe(int n)
        {
            rowPtr = new int[n];
            colPtr = new int[n];
            fwdDig = 0;
            revDig = 0;
            boardSize = n;
        }

        /** Player {player} makes a move at ({row}, {col}).
            @param row The row of the board.
            @param col The column of the board.
            @param player The player, can be either 1 or 2.
            @return The current winning condition, can be either:
                    0: No one wins.
                    1: Player 1 wins.
                    2: Player 2 wins. */
        public int Move(int row, int col, int player)
        {
            player = player == 2 ? -1 : 1;
            if (row == col)
            {
                fwdDig += player;
            }
            if (row + col == boardSize - 1)
            {
                revDig += player;
            }
            colPtr[col] += player;
            rowPtr[row] += player;
            if (IsWinner(fwdDig) || IsWinner(revDig) || IsWinner(colPtr[col]) || IsWinner(rowPtr[row]))
            {
                return player > 0 ? 1 : 2;
            }

            return 0;
        }

        private bool IsWinner(int n)
        {
            return Math.Abs(n) == boardSize;
        }
    }

}
