using board.engine.Movement;
using board.engine.tests.Movement;
using chess.engine.Extensions;
using chess.engine.Game;
using chess.engine.Movement.King;
using NUnit.Framework;

namespace chess.engine.integration.tests.Movement
{
    [TestFixture]

    public class KingCastleValidatorTests : ValidationTestsBase
    {
        private KingCastleValidator _validator;
        private readonly BoardMove _whiteInvalidKingCastle = new BoardMove("D1".ToBoardLocation(), "G1".ToBoardLocation(), (int)ChessMoveTypes.CastleKingSide);
        private readonly BoardMove _whiteKingSideCastle = new BoardMove("E1".ToBoardLocation(), "G1".ToBoardLocation(), (int)ChessMoveTypes.CastleKingSide);
        private readonly BoardMove _whiteQueenSideCastle = new BoardMove("E1".ToBoardLocation(), "C1".ToBoardLocation(), (int)ChessMoveTypes.CastleKingSide);

        [SetUp]
        public void Setup()
        {
            _validator = new KingCastleValidator(new ChessValidationSteps());
        }

        [Test]
        public void ValidateMove_fails_unless_king_is_in_starting_position()
        {
            var board = new ChessBoardBuilder()
                .Board("    k   " +
                       "        " +
                       "        " +
                       "        " +
                       "        " +
                       "        " +
                       "        " +
                       "   K   R"
                );

            var boardState = ChessFactory.CustomChessGame(board.ToGameSetup(), Colours.White).BoardState;

            Assert.False(_validator.ValidateMove(_whiteInvalidKingCastle, boardState), "Invalid castle move allowed");
        }
        [Test]
        public void ValidateMove_fails_unless_rook_is_in_starting_position()
        {
            var board = new ChessBoardBuilder()
                .Board("    k   " +
                       "        " +
                       "        " +
                       "        " +
                       "        " +
                       "        " +
                       "R       " +
                       "    K R "
                );

            var boardState = ChessFactory.CustomChessGame(board.ToGameSetup(), Colours.White).BoardState;

            Assert.False(_validator.ValidateMove(_whiteQueenSideCastle, boardState), "Invalid queen side castle move allowed");
            Assert.False(_validator.ValidateMove(_whiteKingSideCastle, boardState), "Invalid king side castle move allowed");
        }
        [Test]
        public void ValidateMove_fails_if_no_clear_path()
        {
            var board = new ChessBoardBuilder()
                .Board("    k   " +
                       "        " +
                       "        " +
                       "        " +
                       "        " +
                       "        " +
                       "        " +
                       "    K NR"
                );

            var boardState = ChessFactory.CustomChessGame(board.ToGameSetup(), Colours.White).BoardState;

            Assert.False(_validator.ValidateMove(_whiteKingSideCastle, boardState), "Invalid king side castle move allowed");
        }
        [Test]
        public void ValidateMove_fails_if_path_under_attack()
        {
            var board = new ChessBoardBuilder()
                .Board("    k   " +
                       "        " +
                       "        " +
                       "        " +
                       "     r  " +
                       "        " +
                       "        " +
                       "    K  R"
                );

            var boardState = ChessFactory.CustomChessGame(board.ToGameSetup(), Colours.White).BoardState;

            Assert.False(_validator.ValidateMove(_whiteKingSideCastle, boardState), "Invalid king side castle move allowed");
        }

        [Test]
        public void Regression_king_side_castle_bug()
        {
            var board = new ChessBoardBuilder()
                .Board(".rbqkbnr" +
                       "pppppppp" +
                       "n......." +
                       "........" +
                       "........" +
                       ".....NPB" +
                       "PPPPPP.P" +
                       "RNBQK..R"
                );

            var buildGame = ChessFactory.CustomChessGame(board.ToGameSetup(), Colours.White);
            var boardState = buildGame.BoardState;

            var msg = buildGame.Move(_whiteKingSideCastle.ToChessCoords());
            Assert.IsEmpty(msg, msg);

            Assert.False(boardState.IsEmpty("G1".ToBoardLocation()), $"No item at G1");
            var king = boardState.GetItem("G1".ToBoardLocation());
            Assert.That(king.Item.EntityType, Is.EqualTo((int) ChessPieceName.King), "king not moved correctly");

            Assert.NotNull(boardState.GetItem("F1".ToBoardLocation()), $"No item at F1");
            var rook = boardState.GetItem("F1".ToBoardLocation());
            Assert.That(rook.Item.EntityType, Is.EqualTo((int) ChessPieceName.Rook), "castle not moved correctly");
        }

        [Test]
        [Repeat(10000)]
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
        [Repeat(10000)]
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