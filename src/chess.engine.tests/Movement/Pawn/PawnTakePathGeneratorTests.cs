using System.Linq;
using board.engine.Actions;
using chess.engine.Extensions;
using chess.engine.Game;
using chess.engine.Movement.Pawn;
using chess.engine.tests.Builders;
using NUnit.Framework;
using Shouldly;

namespace chess.engine.tests.Movement.Pawn
{
    [TestFixture]
    public class PawnTakePathGeneratorTests : ChessPathGeneratorTestsBase
    {
        // TODO: These are the old right path gen tests, more & better tests needed
        private PawnTakePathGenerator _gen;

        [SetUp]
        public new void SetUp()
        {
            _gen = new PawnTakePathGenerator();
        }
        [Test]
        public void PathsFrom_generates_empty_list_when_on_right_edge()
        {
            _gen.PathsFrom("H2".ToBoardLocation(), (int)Colours.White).Count().ShouldBe(1);
            _gen.PathsFrom("A7".ToBoardLocation(), (int)Colours.Black).Count().ShouldBe(1);
        }

        [Test]
        public void PathsFrom_generates_take()
        {
            var pieceLocation = "B2".ToBoardLocation();
            var paths = _gen.PathsFrom(pieceLocation, (int)Colours.White).ToList();

            var ep = new ChessPathBuilder().From(pieceLocation)
                .To("C3", (int)DefaultActions.TakeOnly)
                .Build();

            PathsShouldContain(paths, ep, Colours.White);
            paths.Count().ShouldBe(2);
        }


        [Test]
        public void PathsFrom_generates_all_pawn_promotions()
        {
            var startLocation = "B7".ToBoardLocation();
            var whitePaths = _gen.PathsFrom(startLocation, (int)Colours.White).ToList();
            whitePaths.Count().ShouldBe(8);


            foreach (var chessPieceName in new[]{ChessPieceName.Knight, ChessPieceName.Bishop, ChessPieceName.Rook, ChessPieceName.Queen})
            {
                PathsShouldContain(whitePaths, new ChessPathBuilder().From(startLocation)
                    .ToUpdatePiece("A8", chessPieceName, DefaultActions.UpdatePieceWithTake)
                    .Build(), Colours.White);

                PathsShouldContain(whitePaths, new ChessPathBuilder().From(startLocation)
                    .ToUpdatePiece("C8", chessPieceName, DefaultActions.UpdatePieceWithTake)
                    .Build(), Colours.White);
            }
        }
    }
}