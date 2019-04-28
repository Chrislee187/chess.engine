﻿using System.Linq;
using Chess.Common.Tests.Helpers;
using CSharpChess;
using CSharpChess.System;
using NUnit.Framework;

namespace Chess.Common.Tests.BoardBuilderTests
{
    [TestFixture]
    public class default_boards : BoardAssertions
    {
        [Test]
        public void empty_board()
        {
            var board = BoardBuilder.EmptyBoard;

            Assert.True(board.Pieces.All(p => p.Piece.Equals(PiecesFactory.Blank)));
        }

        [Test]
        public void newgame_board()
        {
            var board = BoardBuilder.NewGame;

            AssertNewGameBoard(board);
        }

        [Test]
        public void custom_board_can_be_built_using_onechar_notation()
        {
            var asOneChar = 
                "rnbqkbnr" +
                "pppppppp" +
                "........" +
                "........" +
                "........" +
                "........" +
                "PPPPPPPP" +
                "RNBQKBNR";

            var board = BoardBuilder.CustomBoard(asOneChar, Colours.White);

            AssertNewGameBoard(board);
        }

    }
}