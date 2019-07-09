using System.Linq;
using board.engine.Movement;
using chess.engine.Extensions;
using chess.engine.Game;
using chess.engine.Movement.King;
using chess.engine.tests.Builders;
using NUnit.Framework;
using Shouldly;

namespace chess.engine.tests.Movement.King
{
    [TestFixture]
    public class KingNormalPathGeneratorTests : ChessPathGeneratorTestsBase
    {
        private KingNormalPathGenerator _gen;

        [SetUp]
        public new void SetUp()
        {
            _gen = new KingNormalPathGenerator();
        }

        [Test]
        public void PathsFrom_generates_all_directions()
        {
            var boardLocation = "E2".ToBoardLocation();
            var whitePaths = _gen.PathsFrom(boardLocation, (int) Colours.White).ToList();

            whitePaths.Count().ShouldBe(8);

            PathsShouldContain(whitePaths,
                new ChessPathBuilder().From("E2").To("E3", (int)ChessMoveTypes.KingMove).Build(), Colours.White);
            PathsShouldContain(whitePaths,
                new ChessPathBuilder().From("E2").To("F3", (int)ChessMoveTypes.KingMove).Build(), Colours.White);
            PathsShouldContain(whitePaths,
                new ChessPathBuilder().From("E2").To("F2", (int)ChessMoveTypes.KingMove).Build(), Colours.White);
            PathsShouldContain(whitePaths,
                new ChessPathBuilder().From("E2").To("F1", (int)ChessMoveTypes.KingMove).Build(), Colours.White);
            PathsShouldContain(whitePaths,
                new ChessPathBuilder().From("E2").To("E1", (int)ChessMoveTypes.KingMove).Build(), Colours.White);
            PathsShouldContain(whitePaths,
                new ChessPathBuilder().From("E2").To("D1", (int)ChessMoveTypes.KingMove).Build(), Colours.White);
            PathsShouldContain(whitePaths,
                new ChessPathBuilder().From("E2").To("D2", (int)ChessMoveTypes.KingMove).Build(), Colours.White);
            PathsShouldContain(whitePaths,
                new ChessPathBuilder().From("E2").To("D3", (int)ChessMoveTypes.KingMove).Build(), Colours.White);
        }
    }
}