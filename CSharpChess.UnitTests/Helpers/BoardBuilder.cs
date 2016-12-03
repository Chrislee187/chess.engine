﻿using CSharpChess.TheBoard;

namespace CSharpChess.UnitTests.Helpers
{
    public static class BoardBuilder
    {
        public static ChessBoard EmptyBoard => new ChessBoard(false);

        public static ChessBoard NewGame => new ChessBoard(true);

        public static ChessBoard CustomBoard(string boardInOneCharNotation, Chess.Colours toPlay)
        {
            var customboard = ChessBoardHelper.OneCharBoardToBoardPieces(boardInOneCharNotation);
            var board = new ChessBoard(customboard, toPlay);
            return board;
        }

    }
}