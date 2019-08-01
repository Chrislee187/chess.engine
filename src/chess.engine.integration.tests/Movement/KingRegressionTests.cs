using board.engine.Movement;
using board.engine.tests.Movement;
using chess.engine.Extensions;
using chess.engine.Game;
using NUnit.Framework;

namespace chess.engine.integration.tests.Movement
{
    [TestFixture]

    public class KingsCantMoveNextToEachOtherTests : ValidationTestsBase
    {
        [Test]
        public void Regression_Kings_cant_move_next_to_each_other_white()
        {
            var board = new ChessBoardBuilder()
                .Board("  K k   " +
                       "        " +
                       "        " +
                       "        " +
                       "        " +
                       "        " +
                       "        " +
                       "        "
                );

            var boardState = ChessFactory.CustomChessGame(board.ToGameSetup(), Colours.White).BoardState;

            var from = "c8".ToBoardLocation();
            var to = "d8".ToBoardLocation();
            var move = BoardMove.Create(from, to, (int)ChessMoveTypes.KingMove);
            var item = boardState.GetItem(from);

            Assert.False(item.Paths.ContainsMoveTo(to), "kings cannot move next to each other");

        }

        [Test]
        public void Regression_Kings_cant_move_next_to_each_other_black()
        {
            var board = new ChessBoardBuilder()
                .Board("  K k   " +
                       "        " +
                       "        " +
                       "        " +
                       "        " +
                       "        " +
                       "        " +
                       "        "
                );

            var boardState = ChessFactory.CustomChessGame(board.ToGameSetup(), Colours.Black).BoardState;

            var from = "e8".ToBoardLocation();
            var to = "d8".ToBoardLocation();
            var move = BoardMove.Create(from, to, (int)ChessMoveTypes.KingMove);
            var item = boardState.GetItem(from);
            Assert.False(item.Paths.ContainsMoveTo(to), "kings cannot move next to each other");

        }
    }
}